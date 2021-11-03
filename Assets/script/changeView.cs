using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeView : MonoBehaviour
{
    public Camera camera_one;
    public Camera camera_two;
   
    private void Start()
    {
        camera_one.enabled = true;
        camera_two.enabled = false;
    }
    private void Update() 
    {
        float mouseX = Input.GetAxis("Mouse X") * 1 ;
        float mouseY = Input.GetAxis("Mouse Y") * 1 ;
        if(Input.GetKeyDown(KeyCode.Q))  
        {
            Debug.Log("answer");
            camera_one.enabled = !camera_one.enabled;
            camera_two.enabled = !camera_two.enabled;
        }

        if (camera_one.enabled&&Input.GetMouseButton (2))
        {
            camera_one.transform.localRotation = camera_one.transform.localRotation * Quaternion.Euler(-mouseY, mouseX, 0);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            camera_one.transform.localRotation = Quaternion.Euler(90, 0, 0);
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (camera_two.fieldOfView <= 80)
                camera_two.fieldOfView += 2;
            if (camera_two.orthographicSize <= 20)
                camera_two.orthographicSize += 0.5F;
            if (camera_one.fieldOfView <= 80)
                camera_one.fieldOfView += 2;
            if (camera_one.orthographicSize <= 20)
                camera_one.orthographicSize += 0.5F;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (camera_two.fieldOfView > 20)
                camera_two.fieldOfView -= 2;
            if (camera_two.orthographicSize >= 1)
                camera_two.orthographicSize -= 0.5F;
            if (camera_one.fieldOfView > 20)
                camera_one.fieldOfView -= 2;
            if (camera_one.orthographicSize >= 1)
                camera_one.orthographicSize -= 0.5F;
        }
    }
}
