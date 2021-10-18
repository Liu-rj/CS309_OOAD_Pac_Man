using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heihei : MonoBehaviour
{
    public Camera camera_one;
    public Rigidbody rd;
    public Text score_text;
    public int score;
    public Text victory;
    public Animation am;

    public Transform up_t;
    public Transform down_t;
    public Transform right_t;
    public Transform left_t;
    // Start is called before the first frame update
    void Start()
    {
        rd=GetComponent<Rigidbody>();
        rd.freezeRotation = true;
        am = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
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
            if(score==32){
                victory.text="Win!";
            }
        }
    }
}
