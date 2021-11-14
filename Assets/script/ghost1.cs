using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ghost1 : MonoBehaviour
{
    private Rigidbody rd;
    private Vector3 Pos_pre;
    private Vector3 Pos_now;
    
    private float start;
    private int count;
    private Vector3 _velocity;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        rd.velocity = new Vector3(0, 0, 1) * 3;
        _velocity = rd.velocity;
        transform.rotation = Quaternion.Euler(-90, 0, 90);
        Pos_now = GetComponent<Transform>().position;
        Pos_pre = Pos_now;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        rd.velocity = _velocity;
        count++;
        if (count == 5)
        {
            count = 0;
            Pos_pre.x = Pos_now.x;
            Pos_pre.z = Pos_now.z;
            Pos_now = GetComponent<Transform>().position;
            System.Random random = new System.Random();
            if (Math.Abs(Pos_now.x - Pos_pre.x) < 0.05f && Math.Abs(Pos_now.z - Pos_pre.z) < 0.05f)
            {
                var i = random.Next(0, 4);
        
                if (i == 0)
                {
                    rd.velocity = (new Vector3(-1, 0, 0) * 3);
                    transform.rotation = Quaternion.Euler(-90, 0, 0);
                    _velocity = rd.velocity;
                }
                else if (i == 1)
                {
                    rd.velocity = (new Vector3(1, 0, 0) * 3);
                    transform.rotation = Quaternion.Euler(-90, 0, -180);
                    _velocity = rd.velocity;
                }
                else if (i == 2)
                {
                    rd.velocity = (new Vector3(0, 0, 1) * 3);
                    transform.rotation = Quaternion.Euler(-90, 0, 90);
                    _velocity = rd.velocity;
                }
                else
                {
                    rd.velocity = (new Vector3(0, 0, -1) * 3);
                    transform.rotation = Quaternion.Euler(-90, 0, -90);
                    _velocity = rd.velocity;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        System.Random random = new System.Random();

        if (collision.gameObject.CompareTag("out_wall") || collision.gameObject.CompareTag("chair") ||
            collision.gameObject.CompareTag("desk") || collision.gameObject.CompareTag("man") ||
            collision.gameObject.CompareTag("ghost"))
        {
            // Debug.Log("ghost collide with " + collision.gameObject.tag);
            var i = random.Next(0, 4);

            if (i == 0)
            {
                rd.velocity = (new Vector3(-1, 0, 0) * 3);
                transform.rotation = Quaternion.Euler(-90, 0, 0);
                _velocity = rd.velocity;
            }
            else if (i == 1)
            {
                rd.velocity = (new Vector3(1, 0, 0) * 3);
                transform.rotation = Quaternion.Euler(-90, 0, -180);
                _velocity = rd.velocity;
            }
            else if (i == 2)
            {
                rd.velocity = (new Vector3(0, 0, 1) * 3);
                transform.rotation = Quaternion.Euler(-90, 0, 90);
                _velocity = rd.velocity;
            }
            else
            {
                rd.velocity = (new Vector3(0, 0, -1) * 3);
                transform.rotation = Quaternion.Euler(-90, 0, -90);
                _velocity = rd.velocity;
            }
        }
    }
}