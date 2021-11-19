using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class newheihei : MonoBehaviour
{
    // public Camera camera_one;
    public Rigidbody rd;

    public Transform up_t;
    public Transform down_t;
    public Transform right_t;
    public Transform left_t;
    public Text dialog;

    private int speed = 6;
    private bool _show_test = false;

    // Start is called before the first frame update
    void Start()
    {
        rd=GetComponent<Rigidbody>();
        rd.freezeRotation = true;
        // am = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_show_test)
        {
            dialog.text = "紫色的藤蔓漫过了角斗场,\n 金色的阶梯通往人与机器的战场，\n 迷雾笼罩了蓝色的轮回之门,\n AI在地狱中搏杀。";
        }
        else
        {
            dialog.text ="";
        }
        float h=Input.GetAxis("Horizontal");
        float v=Input.GetAxis("Vertical");
        int temp = GetComponent<changeView>().currentCamera;
        if (temp<2)
        {
            if(v>0){
                // rd.AddForce(new Vector3(0,0,1)*2);
                rd.velocity = (new Vector3(0, 0, 1) * speed);
                transform.LookAt(left_t.position);
            }else if(v<0){
                // rd.AddForce(new Vector3(0,0,-1)*2);
                rd.velocity = (new Vector3(0, 0, -1) * speed);
                transform.LookAt(right_t.position);
            }else if(h>0){
                // rd.AddForce(new Vector3(1,0,0)*2);
                rd.velocity = (new Vector3(1, 0, 0) * speed);
                transform.LookAt(up_t.position);
            }else if(h<0){
                // rd.AddForce(new Vector3(-1,0,0)*2);
                rd.velocity = (new Vector3(-1, 0, 0) * speed);
                transform.LookAt(down_t.position);
            }
        }
        else
        {
            if(v<0){
                // rd.AddForce(new Vector3(0,0,1)*2);
                rd.velocity = (new Vector3(0, 0, 1) * speed);
                transform.LookAt(left_t.position);
            }else if(v>0){
                // rd.AddForce(new Vector3(0,0,-1)*2);
                rd.velocity = (new Vector3(0, 0, -1) * speed);
                transform.LookAt(right_t.position);
            }else if(h<0){
                // rd.AddForce(new Vector3(1,0,0)*2);
                rd.velocity = (new Vector3(1, 0, 0) * speed);
                transform.LookAt(up_t.position);
            }else if(h>0){
                // rd.AddForce(new Vector3(-1,0,0)*2);
                rd.velocity = (new Vector3(-1, 0, 0) * speed);
                transform.LookAt(down_t.position);
            }
        }
        // if (Input.GetKey(KeyCode.Space)){
        //     rd.AddForce(new Vector3(0,1,0)*5);
        // }
    }

    private void OnTriggerEnter(Collider col)
    {
   
        if (col.gameObject.CompareTag("p2e"))
            SceneManager.LoadScene(5);
        if (col.gameObject.CompareTag("e2e"))
            SceneManager.LoadScene(7);
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("guide"))
            _show_test = true;
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("guide"))
            _show_test = false;
    }
}
