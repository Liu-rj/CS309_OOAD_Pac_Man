 using UnityEngine;
 using UnityEngine;
 using System.Collections;
 
 public class RotateAroundPacman : MonoBehaviour
 {
     [SerializeField] private Camera cam;
     [SerializeField] private GameObject target;
     [SerializeField] private float distanceToTarget = 10;
    
     private Vector3 previousPosition;
     
     
     void Update()
     {
         if (Input.GetMouseButtonDown(1))
         {
             previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
         }
         else if (Input.GetMouseButton(1))
         {
             Vector3 newPosition = cam.ScreenToViewportPoint(Input.mousePosition);
             Vector3 direction = previousPosition - newPosition;
            
             float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
             float rotationAroundXAxis = direction.y * 180; // camera moves vertically
            
             cam.transform.position = target.transform.position;
            
             cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
             cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World); // <— This is what makes it work!
            
             cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));
            
             previousPosition = newPosition;
         }
     }
 }