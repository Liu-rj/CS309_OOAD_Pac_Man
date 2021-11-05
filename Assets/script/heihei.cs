using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class heihei : MonoBehaviour
{
    // public Camera camera_one;
    public Rigidbody rd;
    public Text score_text;
    public Text hp_text;
    public int score;
    public Text victory;
    private Renderer rend;
    private Material peace;
    public Material angry;

    public Transform up_t;
    public Transform down_t;
    public Transform right_t;
    public Transform left_t;
     private int hp;
    private bool strong;
    private bool suck;
    private bool exit;
    private float exit_time;
    private int direction;
    private float originTime;
    private float originTime_suck;
    private int total_score=0;
    public int type;
    //0  自定义， 1 完全随机
    // Start is called before the first frame update
    void Start()
    {   exit=false;
        hp=3;
        rd=GetComponent<Rigidbody>();
        rd.freezeRotation = true;
        direction=0;
        Debug.Log(type);
        Debug.Log(MazeRenderer.tot_score);
        Debug.Log("****************");
       if (type==0){
            string[,]  str=login_人机._maze;
      
        for (int i=0;i<str.GetLength(0);i++){
            for(int j=0;j<str.GetLength(1);j++){              
                if (str[i,j]=="-1"){
                total_score+=1;
                }
               
            }
        }
       
       }else if (type==1){
        //    total_score=MazeRenderer.tot_score;
        total_score=99;
       }
       Debug.Log(total_score);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Transform>().position=new Vector3(GetComponent<Transform>().position.x,0.5f,GetComponent<Transform>().position.z);
         if (exit){
           exit_time+=Time.deltaTime;
           if (exit_time>2){
                SceneManager.LoadScene(2);
           }
         }
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
            direction=1;
        }else if(v<0){
            rd.velocity=(new Vector3(0,0,-1)*5);
            transform.LookAt(right_t.position);
            direction=2;
        }else if(h>0){
            rd.velocity=(new Vector3(1,0,0)*5);
            transform.LookAt(up_t.position);
            direction=3;
        }else if(h<0){
            rd.velocity=(new Vector3(-1,0,0)*5);
            transform.LookAt(down_t.position);
            direction=4;
        }
    }

    private void OnTriggerEnter(Collider col){
        
        if (col.gameObject.CompareTag("food"))
        {
            Destroy(col.gameObject);
            score++;
            score_text.text="Score:  "+score;
            if(score==total_score){
                victory.text="Win!";
                exit=true;
            }
        }
        if (col.gameObject.CompareTag("bigball"))
        {
            Destroy(col.gameObject);
        
            strong = true;
            originTime=0;
        }
        if (col.gameObject.CompareTag("suckball"))
        {
            Destroy(col.gameObject);
            suck = true;
        }
    }
     private void OnCollisionEnter(Collision collision){
        Debug.Log("Colli");
      
        if (collision.gameObject.CompareTag("ghost"))
        {
            Debug.Log("in");
            if (strong)
            {
                Debug.Log("eat");
                collision.gameObject.transform.position = new Vector3(-24.5f, 0, -24.5f);
                collision.gameObject.GetComponent<ghost1>().reset=true;
            }
            else
            {
                Debug.Log("oh");
                hp-=1;
                hp_text.text="Hp: "+hp;
                if (hp==0){
                      victory.text="Lose!";
                      exit=true;
                }

               if (type==0){
                    rd.velocity = new Vector3(0, 0, 0);
                            transform.position = new Vector3(-24.5f, 0, -24.5f);
               }
               else if (type==1){
                   rd.velocity = new Vector3(0, 0, 0);
                            transform.position = new Vector3(-5, 0.5f, -5);
               }
            }
        }
        if (collision.gameObject.CompareTag("wall_")||(collision.gameObject.CompareTag("out_wall"))){
            Debug.Log("wall");
            if (direction==1){
                rd.velocity=(new Vector3(0,0,1)*5);
            }
            else if (direction==2){
                rd.velocity=(new Vector3(0,0,-1)*5);
            }
            else if (direction==3){
                rd.velocity=(new Vector3(1,0,0)*5);
            }
            else if (direction==4){
                rd.velocity=(new Vector3(-1,0,0)*5);
            }
        }
     }
}
