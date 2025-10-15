using System;
using UnityEngine;

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

      SetGridPosition(StartCol, StartRow);
   }

   public void SetGridPosition(int column, int row)
   {
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
         if (GameManager.Instance != null)
            GameManager.Instance.UseStep();
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
