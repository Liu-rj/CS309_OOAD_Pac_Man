using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follower : MonoBehaviour
{
    public Transform ball;
    private Vector3 distance;
    // Start is called before the first frame update
    void Start()
    {
        distance = transform.position - ball.position ;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButton(1))
        {
            transform.position = ball.position + distance;
            transform.eulerAngles =new Vector3(20, 0, 0);
        }
    }
}
