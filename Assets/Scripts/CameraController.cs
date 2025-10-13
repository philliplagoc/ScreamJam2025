using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   // Separate move amounts for horizontal and vertical movement
   public float MoveAmountX;
   public float MoveAmountY;

   public float GridWidth;
   public float GridHeight;

   private float m_minX, m_maxX, m_minY, m_maxY;

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
   }

   private void MoveCamera(Vector3 direction)
   {
      Vector3 newPosition = transform.position + direction;
      
      // Clamp X and Y values
      newPosition.x = Mathf.Clamp(newPosition.x, m_minX, m_maxX);
      newPosition.y = Mathf.Clamp(newPosition.y, m_minY, m_maxY);
      
      // Update camera's position
      transform.position = newPosition;
   }

   // Public function to move the camera UP (North)
   public void MoveNorth()
   {
      MoveCamera(new Vector3(0, MoveAmountY, -10));
   }

   // Public function to move the camera DOWN (South)
   public void MoveSouth()
   {
      MoveCamera(new Vector3(0, -MoveAmountY, -10));
   }

   // Public function to move the camera RIGHT (East)
   public void MoveEast()
   {
      MoveCamera(new Vector3(MoveAmountX, 0, -10));
   }

   // Public function to move the camera LEFT (West)
   public void MoveWest()
   {
      MoveCamera(new Vector3(-MoveAmountX, 0, -10));
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
