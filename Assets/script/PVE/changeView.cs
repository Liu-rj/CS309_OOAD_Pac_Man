using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeView : MonoBehaviour
{
    public Camera[] cameras = new Camera[3];
    public int currentCamera = 0;
    private void Start()
    {
        cameras[0].enabled = true;
        for (int i = 1; i < 3; i++)
        {
            cameras[i].enabled = false;
        }
    }
    
    private void FixedUpdate() 
    {
        // float mouseX = Input.GetAxis("Mouse X") * 1 ;
        // float mouseY = Input.GetAxis("Mouse Y") * 1 ;
        if(Input.GetKeyDown(KeyCode.Q))
        {
            cameras[0].enabled = currentCamera > 0;
            if (cameras[0].enabled)
            {
                cameras[currentCamera].enabled = false;
                currentCamera = 0;
            }
            else
            {
                cameras[1].enabled = true;
                currentCamera = 1;
            }
            Debug.Log(currentCamera);
        }

        if (currentCamera!=0)
        {
            for (int i = 0; i < 3; i++)
            {
                cameras[i].enabled = false;
            }
            if(Input.GetKeyDown(KeyCode.Alpha1)) {
                currentCamera = 1;
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentCamera = 2;
            }
            // else if(Input.GetKeyDown(KeyCode.Alpha3))
            // {
            //     currentCamera = 3;
            // }
            // else if(Input.GetKeyDown(KeyCode.Alpha4))
            // {
            //     currentCamera = 4;
            // }
            cameras[currentCamera].enabled = true;
        }
        
        

        // if (camera_one.enabled&&Input.GetMouseButton (2))
        // {
        //     camera_one.transform.localRotation = camera_one.transform.localRotation * Quaternion.Euler(-mouseY, mouseX, 0);
        // }
        // if(Input.GetKeyDown(KeyCode.E))
        // {
        //     camera_one.transform.localRotation = Quaternion.Euler(90, 0, 0);
        // }
        
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            for (int i = 0; i < 3; i++)
            {
                if (cameras[i].fieldOfView <= 80)
                    cameras[i].fieldOfView += 2;
                if (cameras[i].orthographicSize <= 20) 
                    cameras[i].orthographicSize += 0.5F;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                if (cameras[i].fieldOfView > 20)
                    cameras[i].fieldOfView -= 2;
                if (cameras[i].orthographicSize >= 1) 
                    cameras[i].orthographicSize -= 0.5F;
            }
        }
    }
}
