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

    private float originTime;
    // Start is called before the first frame update
    void Start()
    {
        rd=GetComponent<Rigidbody>();
        rd.freezeRotation = true;
        strong = false;
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
        float h=Input.GetAxis("Horizontal");
        float v=Input.GetAxis("Vertical");
        if(v>0){
            rd.velocity=(new Vector3(0,0,1)*25);
            transform.LookAt(left_t.position);
        }else if(v<0){
            rd.velocity=(new Vector3(0,0,-1)*25);
            transform.LookAt(right_t.position);
        }else if(h>0){
            rd.velocity=(new Vector3(1,0,0)*25);
            transform.LookAt(up_t.position);
        }else if(h<0){
            rd.velocity=(new Vector3(-1,0,0)*25);
            transform.LookAt(down_t.position);
        }
    }
    // private void OnCollisionEnter(Collections col){

    // }
    private void OnTriggerEnter(Collider col){
        if (col.gameObject.CompareTag("food"))
        {
            Destroy(col.gameObject);
            score++;
            score_text.text="Score:  "+score;
            if(score==31){
                victory.text="Win!";
            }
        }
        if (col.gameObject.CompareTag("bigball"))
        {
            Destroy(col.gameObject);
            strong = true;
            // col.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }

        if (col.gameObject.CompareTag("ghost"))
        {
            if (strong)
            {
                col.gameObject.transform.position = new Vector3(94, 4, -32);
                col.gameObject.GetComponent<ghost1>().reset=true;
            }
            else
            {
                rd.velocity = new Vector3(0, 0, 0);
                            transform.position = new Vector3(55, 4, -55);
            }
        }
    }
}
