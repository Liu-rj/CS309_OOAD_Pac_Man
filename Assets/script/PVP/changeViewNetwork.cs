using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class changeViewNetwork : MonoBehaviour
{
    private Camera[] cameras = new Camera[2];
    public int currentCamera = 0;
    public Transform Pacman;
    private GameObject cameras_obj1;
    private GameObject cameras_obj2;
    private GameObject cameras_obj3;
    private Vector3 distance1;
    private Vector3 distance2;

    private bool go = false;

    public void Startnow()
    {
        cameras[0] = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameras[1] = GameObject.Find("Camera1").GetComponent<Camera>();

        cameras[0].transform.localPosition = new Vector3(13.5f, 36f, -15f);
        cameras[0].transform.localScale = new Vector3(1, 1, 1);
        // cameras[0].depth = -1;
        Vector3 pos = Pacman.position;
        cameras[1].transform.localPosition = new Vector3(pos.x, 20f, pos.z - 10f);
        cameras[1].transform.localScale = new Vector3(1, 1, 1);
        // cameras[1].depth = 0;


        cameras[0].enabled = true;

        for (int i = 1; i < 2; i++)
        {
            cameras[i].enabled = false;
        }

        distance1 = cameras[0].transform.localPosition - Pacman.position;
        distance2 = cameras[1].transform.localPosition - Pacman.position;
        // distance3 = cameras[2].transform.localPosition - Pacman.position;
        go = true;
    }

    private void FixedUpdate()
    {
        // Debug.Log("hiehie");
        if (!go)
        {
            return;
        }


        cameras[0].transform.localPosition = Pacman.position + distance1;
        cameras[1].transform.localPosition = Pacman.position + distance2;
        // cameras[2].transform.localPosition = Pacman.position + distance3;
        if (Input.GetKeyDown(KeyCode.Q))
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

        if (currentCamera != 0)
        {
            for (int i = 0; i < 2; i++)
            {
                cameras[i].enabled = false;
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentCamera = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentCamera = 1;
            }

            cameras[currentCamera].enabled = true;
        }
    }
}