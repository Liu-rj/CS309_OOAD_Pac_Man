using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bean : MonoBehaviour
{
    // Start is called before the first frame update
    public bool moving;
    private float originTime;
    public float speed = 100;
    GameObject target;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("man");
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position,target.transform.position,Time.deltaTime*speed);
            
        }
        
        transform.Rotate(new Vector3(1,1,-1));
    }
}
