using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitToMenu : MonoBehaviour
{
    public Canvas cv_exit;
    // Start is called before the first frame update
    void Start()
    {
        cv_exit.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            cv_exit.enabled = true;
        }
    }
}
