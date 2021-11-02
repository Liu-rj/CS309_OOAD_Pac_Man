using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class load_object : MonoBehaviour
{
    public GameObject chair;
    // public GameObject desk;
    public GameObject food;
 Vector3 a = new Vector3(-25, 0, -25); //实例化预制体的position，可自定义
    // Quaternion b = new Quaternion(0, 0, 0, 0);//实例化预制体的rotation，可自定义

    // Start is called before the first frame update
    void Start()
    {
        // TextAsset textAsset=Resources.Load("b") as TextAsset;
        
        // // 以换行符作为分割点，将该文本分割成若干行字符串，并以数组的形式来保存每行字符串的内容
        // string[] str = textAsset.text.Split('\n');
        // // 将该文本中的字符串输出
         string[,]  str=login_人机._maze;
      
        for (int i=0;i<str.GetLength(0);i++){
            for(int j=0;j<str.GetLength(1);j++){
                if (str[i,j]=="1"){
                  GameObject Chair = GameObject.Instantiate(chair,a+new Vector3(i,0,j)+new Vector3(0.5f,0,0.5f),chair.transform.rotation) as GameObject;  
                }
                if (str[i,j]=="-1"){
                    GameObject Food=GameObject.Instantiate(food,a+new Vector3(i,0,j)+new Vector3(0.5f,0,0.5f),food.transform.rotation) as GameObject;
                }
                // else if (str[i,j]=="2"   ){
                //     //22
                //     //22
                //     //22
                //     //22
                //     if ((j+1)<str.GetLength(1)&& str[i,j+1]=="2"&&((j+2<str.GetLength(1))||(str[i,j+2]!="2"))){
                //         if ((i+1)<str.GetLength(0)&& str[i+1,j]=="2"&&(i+2)<str.GetLength(0)&& str[i+2,j]=="2"&&(i+3)<str.GetLength(0)&& str[i+3,j]=="2"){
                //             if (str[i+1,j+1]=="2"&& str[i+2,j+1]=="2"&& str[i+3,j+1]=="2"){
                //                  GameObject  Desk=GameObject.Instantiate(food,a+new Vector3(i+3/2,0,j+1/2),food.transform.rotation) as GameObject;
                //             }
                //         }      
                //     }
                //     //2222
                //     //2222
                //      if ((i+1)<str.GetLength(0)&& str[i+1,j]=="2"&&((i+2<str.GetLength(0))||(str[i+2,j]!="2"))){
                //         if ((j+1)<str.GetLength(1)&& str[i,j+1]=="2"&&(j+2)<str.GetLength(1)&& str[i,j+2]=="2"&&(j+3)<str.GetLength(1)&& str[i,j+3]=="2"){
                //             if (str[i+1,j+1]=="2"&& str[i+1,j+2]=="2"&& str[i+1,j+3]=="2"){
                //                  GameObject  Desk=GameObject.Instantiate(food,a+new Vector3(i+1/2,0,j+3/2),food.transform.rotation) as GameObject;
                //             }
                //         }      
                //     }
                // }
            }
        }
    
//         for (int i=0;i<str.Length;i++){
//             string[] pos=str[i].Split(',');
//             for(int j=0;j<pos.Length;j++){
//                 if (pos[j]=="1"){
// GameObject Sphere = GameObject.Instantiate(chair,a+new Vector3(i,0,j),chair.transform.rotation) as GameObject;
//                 }
//             }

//         }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
