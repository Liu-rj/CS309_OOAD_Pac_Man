using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cv_main : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera[] cameras = new Camera[2];
    public int currentCamera = 0;
    private bool changeaxis;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject target;
    [SerializeField] private float distanceToTarget = 10;
    private Vector3 previousPosition;
    private void Start()
    {
        cameras[0].enabled = true;
        cameras[1].enabled = false;
        cam = cameras[1];
    }

    // Update is called once per frame
    private void Update() 
    {
        // if (changeaxis)
        // {
        //     float y = Input.GetAxis("Mouse Y");
        //     float x = Input.GetAxis("Mouse X");
        //     cameras[currentCamera].transform.localRotation
        //         = cameras[currentCamera].transform.localRotation * Quaternion.Euler(-y, 4*x, 0);
        //     cameras[currentCamera].transform.localEulerAngles =
        //         new Vector3 (cameras[1].transform.localEulerAngles.x,
        //             cameras[1].transform.localEulerAngles.y,0.0f); 
        // }


        if(Input.GetKeyDown(KeyCode.Q))
        {
            cameras[1].enabled = cameras[0].enabled;
            cameras[0].enabled = !cameras[1].enabled;
            currentCamera = 1 - currentCamera;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            for (int i = 0; i < 2; i++)
            {
                if (cameras[i].fieldOfView <= 80)
                    cameras[i].fieldOfView += 2;
                if (cameras[i].orthographicSize <= 20) 
                    cameras[i].orthographicSize += 0.5F;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            for (int i = 0; i < 2; i++)
            {
                if (cameras[i].fieldOfView > 20)
                    cameras[i].fieldOfView -= 2;
                if (cameras[i].orthographicSize >= 1) 
                    cameras[i].orthographicSize -= 0.5F;
            }
        }
        
        if (currentCamera==1)
        {
            
            // if (Input.GetMouseButtonDown(1))
            // {
            //     changeaxis = true;
            // }
            // if (Input.GetMouseButtonUp(1))
            // {
            //     changeaxis = false;
            // }
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
}
