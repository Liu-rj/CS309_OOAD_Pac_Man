using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class load_object : MonoBehaviour
{
    
    public GameObject chair;
    // public GameObject desk;
    public GameObject food;
    public GameObject strong_tool;
    public GameObject ghost;
    public int mode;
    //0 为道具模式 ，1为人机模式
 Vector3 a = new Vector3(-25, 0, -25); //实例化预制体的position，可自定义
    // Quaternion b = new Quaternion(0, 0, 0, 0);//实例化预制体的rotation，可自定义

    // Start is called before the first frame update
    void Start()
    {
        
         if (mode==0){
             string[,]  str=login_道具._maze;
              for (int i=0;i<str.GetLength(0);i++){
            for(int j=0;j<str.GetLength(1);j++){
                if (str[i,j]=="1"){
                  GameObject Chair = GameObject.Instantiate(chair,a+new Vector3(i,0,j)+new Vector3(0.5f,0,0.5f),chair.transform.rotation) as GameObject;  
                }
                if (str[i,j]=="-1"){
                    GameObject Food=GameObject.Instantiate(food,a+new Vector3(i,0,j)+new Vector3(0.5f,0,0.5f),food.transform.rotation) as GameObject;
                }
                if (str[i,j]=="2"){
                     GameObject Strong_tool=GameObject.Instantiate(strong_tool,a+new Vector3(i,0,j)+new Vector3(0.5f,0,0.5f),strong_tool.transform.rotation) as GameObject;
                }
                if (str[i,j]=="4"){
                     GameObject Ghost=GameObject.Instantiate(ghost,a+new Vector3(i,0,j)+new Vector3(0.5f,0,0.5f),ghost.transform.rotation) as GameObject;
                }
               
            }
        }
         }else if (mode==1){
             string[,]  str=login_人机._maze;
              for (int i=0;i<str.GetLength(0);i++){
            for(int j=0;j<str.GetLength(1);j++){
                if (str[i,j]=="1"){
                  GameObject Chair = GameObject.Instantiate(chair,a+new Vector3(i,0,j)+new Vector3(0.5f,0,0.5f),chair.transform.rotation) as GameObject;  
                }
                if (str[i,j]=="-1"){
                    GameObject Food=GameObject.Instantiate(food,a+new Vector3(i,0,j)+new Vector3(0.5f,0,0.5f),food.transform.rotation) as GameObject;
                }
                if (str[i,j]=="2"){
                     GameObject Strong_tool=GameObject.Instantiate(strong_tool,a+new Vector3(i,0,j)+new Vector3(0.5f,0,0.5f),strong_tool.transform.rotation) as GameObject;
                }
                if (str[i,j]=="4"){
                     GameObject Ghost=GameObject.Instantiate(ghost,a+new Vector3(i,0,j)+new Vector3(0.5f,0,0.5f),ghost.transform.rotation) as GameObject;
                }
               
            }
        }
         }
         
       
    

    }

  
}
