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
        if(Input.GetKeyDown(KeyCode.Q))  
        {
            camera_one.enabled = !camera_one.enabled;
            camera_two.enabled = !camera_two.enabled;
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
