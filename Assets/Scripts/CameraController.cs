using System;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// Holds the data for each cell in the grid.
/// Includes the amount of wood the grid has and
/// the number of days till it "respawns" with wood.
/// If a cell's wood has been collected, it won't have any more wood for a set amount of time.
/// </summary>
public struct GridCell
{
   public int WoodAmount;
   public int DaysUntilRespawn;
}
public class CameraController : MonoBehaviour
{
   public static CameraController Instance;
   
   // Separate move amounts for horizontal and vertical movement
   public float MoveAmountX;
   public float MoveAmountY;

   public float GridWidth;
   public float GridHeight;

   [Header("Starting Position")] 
   public int StartCol;
   public int StartRow;

   private float m_minX, m_maxX, m_minY, m_maxY;

   private GridCell[,] m_gridData;
   private Vector2Int m_currentGridPosition;
   private int m_woodSpawnTimer = 4;  // How long it takes for wood to respawn at a cell once it's been collected

   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
      }
      else
      {
         Destroy(gameObject);
      }
   }

   private void Start()
   {
      // Calculate extents of the grid from the center
      float halfGridWidth = ((GridWidth - 1) / 2.0f) * MoveAmountX;
      float halfGridHeight = ((GridHeight - 1) / 2.0f) * MoveAmountY;

      // Set min/max bounds
      m_minX = -halfGridWidth;
      m_maxX = halfGridWidth;
      m_minY = -halfGridHeight;
      m_maxY = halfGridHeight;

      InitializeGrid();

      SetGridPosition(StartCol, StartRow);
   }

   private void InitializeGrid()
   {
      m_gridData = new GridCell[(int)GridWidth, (int)GridHeight];
      for (int x = 0; x < GridWidth; x++)
      {
         for (int y = 0; y < GridHeight; y++)
         {
            m_gridData[x, y] = new GridCell
            {
               WoodAmount = Random.Range(1, 4),
               DaysUntilRespawn = 0  // Not waiting to respawn initially
            };
         }
      }
   }
   
   /// <summary>
   /// GameManager will call this at the beginning of each day.
   /// </summary>
   public void AdvanceDay()
   {
      for (int x = 0; x < GridWidth; x++)
      {
         for (int y = 0; y < GridHeight; y++)
         {
            if (m_gridData[x, y].DaysUntilRespawn > 0)
            {
               m_gridData[x, y].DaysUntilRespawn--;

               if (m_gridData[x, y].DaysUntilRespawn == 0)
               {
                  // Respawn wood at this location
                  m_gridData[x, y].WoodAmount = Random.Range(1, 4);
                  Debug.Log($"Wood has respawned at cell ({x}, {y}).");
               }
            }
         }
      }
   }

   public void SetGridPosition(int column, int row)
   {
      m_currentGridPosition = new Vector2Int(column, row);
      
      float centerX = (GridWidth - 1) / 2.0f;
      float centerY = (GridHeight - 1) / 2.0f;

      float offsetX = column - centerX;
      float offsetY = row - centerY;

      float targetX = offsetX * MoveAmountX;
      float targetY = offsetY * MoveAmountY;

      transform.position = new Vector3(targetX, targetY, transform.position.z);
   }

   private void MoveCamera(Vector3 direction)
   {
      Vector3 originalPosition = transform.position;
      Vector3 newPosition = originalPosition + direction;
      
      // Clamp X and Y values
      newPosition.x = Mathf.Clamp(newPosition.x, m_minX, m_maxX);
      newPosition.y = Mathf.Clamp(newPosition.y, m_minY, m_maxY);
      
      // Update camera's position
      transform.position = newPosition;

      if (Vector3.Distance(originalPosition, transform.position) > 0.01f)
      {
         // Update integer grid position based on direction of movement
         if (direction.x > 0) m_currentGridPosition.x++;
         if (direction.x < 0) m_currentGridPosition.x--;
         if (direction.y > 0) m_currentGridPosition.y++;
         if (direction.y < 0) m_currentGridPosition.y--;
         
         if (GameManager.Instance != null)
            GameManager.Instance.UseStep();
         
         // Collect wood from specific cell we just moved to
         CollectWoodAtCurrentPosition();
      }
   }

   private void CollectWoodAtCurrentPosition()
   {
      int x = m_currentGridPosition.x;
      int y = m_currentGridPosition.y;

      if (m_gridData[x, y].WoodAmount > 0)
      {
         int woodToCollect = m_gridData[x, y].WoodAmount;
         Debug.Log($"Collected {woodToCollect} wood from cell ({x}, {y}).");
         
         InventoryManager.Instance.AddItem(ItemType.Wood, woodToCollect);
         
         // Set wood on this cell to 0 and restart timer
         m_gridData[x, y].WoodAmount = 0;
         m_gridData[x, y].DaysUntilRespawn = m_woodSpawnTimer;
      }
   }

   // Public function to move the camera UP (North)
   public void MoveNorth()
   {
      MoveCamera(new Vector3(0, MoveAmountY, 0));
   }

   // Public function to move the camera DOWN (South)
   public void MoveSouth()
   {
      MoveCamera(new Vector3(0, -MoveAmountY, 0));
   }

   // Public function to move the camera RIGHT (East)
   public void MoveEast()
   {
      MoveCamera(new Vector3(MoveAmountX, 0, 0));
   }

   // Public function to move the camera LEFT (West)
   public void MoveWest()
   {
      MoveCamera(new Vector3(-MoveAmountX, 0, 0));
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.yellow;

      float centerX = (GridWidth - 1) / 2.0f;
      float centerY = (GridHeight - 1) / 2.0f;

      for (int x = 0; x < GridWidth; x++)
      {
         for (int y = 0; y < GridHeight; y++)
         {
            float offsetX = x - centerX;
            float offsetY = y - centerY;

            // Calculate center of cell then draw wireframe box around it
            Vector3 cellCenter = new Vector3(offsetX * MoveAmountX, offsetY * MoveAmountY, 0);
            Gizmos.DrawWireCube(cellCenter, new Vector3(MoveAmountX, MoveAmountY, 0.1f));                        
         }
      }
   }
}
