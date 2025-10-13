using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   // Separate move amounts for horizontal and vertical movement
   public float MoveAmountX;
   public float MoveAmountY;
   
   // Public function to move the camera UP (North)
   public void MoveNorth()
   {
      transform.position += new Vector3(0, MoveAmountY, 0);
   }

   // Public function to move the camera DOWN (South)
   public void MoveSouth()
   {
      transform.position += new Vector3(0, -MoveAmountY, 0);
   }

   // Public function to move the camera RIGHT (East)
   public void MoveEast()
   {
      transform.position += new Vector3(MoveAmountX, 0, 0);
   }

   // Public function to move the camera LEFT (West)
   public void MoveWest()
   {
      transform.position += new Vector3(-MoveAmountX, 0, 0);
   }
}
