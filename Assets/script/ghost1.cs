using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ghost1 : MonoBehaviour
{
    private Rigidbody rd;
    private Vector3 Pos_pre;
    private Vector3 Pos_now;
    // public Transform up_t;
    // public Transform down_t;
    // public Transform right_t;
    // public Transform left_t;
    private float start;
    private int count;
    public bool reset;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        rd.freezeRotation = true;
        rd.velocity = (new Vector3(0, 0, 1) * 3);
        //transform.LookAt(right_t.position);
        transform.rotation =Quaternion.Euler(-90, 0, 90);
        Pos_pre=GetComponent<Transform>().position-new Vector3(1,1,0);
        Pos_now = GetComponent<Transform>().position;
        count=0;
    }

    // Update is called once per frame
    void Update()
    {
        count++;
        if (count==20){
            count=0;
             Pos_pre.x=Pos_now.x;
         Pos_pre.z=Pos_now.z;
         Pos_now = GetComponent<Transform>().position;
         //transform.position = new Vector3(Pos_now.x,0,Pos_now.z);
         //  Debug.Log(Pos_pre.x);
        //  Debug.Log(Pos_pre.z);
        // Debug.Log(Pos_now.x);
        // Debug.Log(Pos_now.z);
         System.Random random=new System.Random();
        if (Pos_pre.x==Pos_now.x&&Pos_pre.z==Pos_now.z ){
            //  Debug.Log("em here");
              var i = random.Next(0,4);
             
              if (i==0){
                  rd.velocity = (new Vector3(-1, 0, 0) * 3);
                  //transform.LookAt(up_t.position);
                  transform.rotation =Quaternion.Euler(-90, 0, 0);
              }else if (i==1){
                  rd.velocity=(new Vector3(1,0,0)*3);
                  //transform.LookAt(down_t.position);
                  transform.rotation =Quaternion.Euler(-90, 0, -180);
              }else if (i==2){
                  rd.velocity=(new Vector3(0,0,1)*3);
                  //transform.LookAt(right_t.position);
                  transform.rotation =Quaternion.Euler(-90, 0, 90);
              }else {
                  rd.velocity = (new Vector3(0, 0, -1) * 3);
                  //transform.LookAt(left_t.position);
                  transform.rotation =Quaternion.Euler(-90, 0, -90);
              }
        }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
      
        System.Random random=new System.Random();
        
        if (collision.gameObject.CompareTag("out_wall")||collision.gameObject.CompareTag("chair")||collision.gameObject.CompareTag("desk")||collision.gameObject.CompareTag("man")||collision.gameObject.CompareTag("ghost")){
           
              var i = random.Next(0,4);
           
              if (i==0){
                  rd.velocity = (new Vector3(-1, 0, 0) * 3);
                  //transform.LookAt(up_t.position);
                  transform.rotation =Quaternion.Euler(-90, 0, 0);
              }else if (i==1){
                  rd.velocity=(new Vector3(1,0,0)*3);
                  //transform.LookAt(down_t.position);
                  transform.rotation =Quaternion.Euler(-90, 0, -180);
              }else if (i==2){
                  rd.velocity=(new Vector3(0,0,1)*3);
                  //transform.LookAt(right_t.position);
                  transform.rotation =Quaternion.Euler(-90, 0, 90);
              }else {
                  rd.velocity = (new Vector3(0, 0, -1) * 3);
                  //transform.LookAt(left_t.position);
                  transform.rotation =Quaternion.Euler(-90, 0, -90);
              }
        }
       
    }
}
