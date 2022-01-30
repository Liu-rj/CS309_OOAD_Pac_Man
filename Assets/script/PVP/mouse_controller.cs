using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse_controller : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera camera_now;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
         if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            for (int i = 0; i < 5; i++)
            {
                if (camera_now.fieldOfView <= 80)
                    camera_now.fieldOfView += 2;
                if (camera_now.orthographicSize <= 20) 
                    camera_now.orthographicSize += 0.5F;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            for (int i = 0; i < 5; i++)
            {
                if (camera_now.fieldOfView > 20)
                    camera_now.fieldOfView -= 2;
                if (camera_now.orthographicSize >= 1) 
                    camera_now.orthographicSize -= 0.5F;
            }
        }
    }
}
