using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heihei : MonoBehaviour
{
    // public Camera camera_one;
    public Rigidbody rd;
    public Text score_text;
    public int score;
    public Text victory;
    private Renderer rend;
    private Material peace;
    public Material angry;

    public Transform up_t;
    public Transform down_t;
    public Transform right_t;
    public Transform left_t;

    private bool strong;
    private bool suck;

    private float originTime;
    private float originTime_suck;
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
        if (strong)
        {
            originTime += Time.deltaTime;
            if (originTime>10)
            {
                strong = false;
                originTime = 0;
            }
        }
        if (suck)
        {
            originTime_suck += Time.deltaTime;
            if (originTime_suck>5)
            {
                suck = false;
                originTime_suck = 0;
            }
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, 20);
            foreach (var item in colliders)
            {
                if (item.tag.Equals("food"))
                {
                    //让金币的开始移动
                    item.GetComponent<bean>().moving = true;
                }
            }
        }
        float h=Input.GetAxis("Horizontal");
        float v=Input.GetAxis("Vertical");
        if(v>0){
            rd.velocity=(new Vector3(0,0,1)*5);
            transform.LookAt(left_t.position);
        }else if(v<0){
            rd.velocity=(new Vector3(0,0,-1)*5);
            transform.LookAt(right_t.position);
        }else if(h>0){
            rd.velocity=(new Vector3(1,0,0)*5);
            transform.LookAt(up_t.position);
        }else if(h<0){
            rd.velocity=(new Vector3(-1,0,0)*5);
            transform.LookAt(down_t.position);
        }
    }
    // private void OnCollisionEnter(Collections col){

    // }
    private void OnTriggerEnter(Collider col){
        Debug.Log("trigger");
        if (col.gameObject.CompareTag("food"))
        {
            Destroy(col.gameObject);
            score++;
            score_text.text="Score:  "+score;
            if(score==30){
                victory.text="Win!";
            }
        }
        if (col.gameObject.CompareTag("bigball"))
        {
            Destroy(col.gameObject);
            strong = true;
            // col.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }

        if (col.gameObject.CompareTag("suckball"))
        {
            Destroy(col.gameObject);
            suck = true;
        }

        // if (col.gameObject.CompareTag("ghost"))
        // {
        //     Debug.Log("in");
        //     if (strong)
        //     {
        //         col.gameObject.transform.position = new Vector3(94, 4, -32);
        //         col.gameObject.GetComponent<ghost1>().reset=true;
        //     }
        //     else
        //     {Debug.Log("a");
        //         rd.velocity = new Vector3(0, 0, 0);
        //                     transform.position = new Vector3(0.5f, 0, 0.5f);
        //     }
        // }
    }
     private void OnCollisionEnter(Collision collision){
          Debug.Log("collison");
        if (collision.gameObject.CompareTag("food"))
        {
            Destroy(collision.gameObject);
            score++;
            score_text.text="Score:  "+score;
            if(score==30){
                victory.text="Win!";
            }
        }
        if (collision.gameObject.CompareTag("bigball"))
        {
            Destroy(collision.gameObject);
            strong = true;
            // col.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }

        if (collision.gameObject.CompareTag("suckball"))
        {
            Destroy(collision.gameObject);
            suck = true;
        }

        if (collision.gameObject.CompareTag("ghost"))
        {
            Debug.Log("in");
            if (strong)
            {
                collision.gameObject.transform.position = new Vector3(94, 4, -32);
                collision.gameObject.GetComponent<ghost1>().reset=true;
            }
            else
            {Debug.Log("a");
                rd.velocity = new Vector3(0, 0, 0);
                            transform.position = new Vector3(0.5f, 0, 0.5f);
            }
        }
     }
}
