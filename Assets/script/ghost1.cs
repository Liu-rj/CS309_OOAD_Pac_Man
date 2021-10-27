using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghost1 : MonoBehaviour
{
    private Rigidbody rd;

    // public Transform up_t;
    // public Transform down_t;
    // public Transform right_t;
    // public Transform left_t;
    private float start;

    public bool reset;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        rd.freezeRotation = true;
        rd.velocity = (new Vector3(0, 0, 1) * 25);
    }

    // Update is called once per frame
    void Update()
    {
        // if (reset || start > 11.8)
        // {
        //     start = 0;
        //     reset = false;
        // }
        //
        // start += Time.deltaTime;
        //
        // if (start > 8.6)
        // {
        //     rd.velocity = (new Vector3(1, 0, 0) * 25);
        //     // transform.LookAt(up_t.position);
        // }
        // else if (start > 5.9)
        // {
        //     rd.velocity = (new Vector3(0, 0, -1) * 25);
        //     // transform.LookAt(right_t.position);
        // }
        // else
        // {
        //     rd.velocity = (new Vector3(0, 0, 1) * 25);
        //     // transform.LookAt(left_t.position);
        // }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("gLeft"))
        {
            rd.velocity = (new Vector3(-1, 0, 0) * 25);
        }
        if (col.gameObject.CompareTag("gRight"))
        {
            rd.velocity = (new Vector3(1, 0, 0) * 25);
        }
        if (col.gameObject.CompareTag("gUp"))
        {
            rd.velocity = (new Vector3(0, 0, 1) * 25);
        }
        if (col.gameObject.CompareTag("gDown"))
        { 
            rd.velocity = (new Vector3(0, 0, -1) * 25);
        }
    }
}