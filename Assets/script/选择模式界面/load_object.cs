using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class load_object : MonoBehaviour
{
    public GameObject chair;
 Vector3 a = new Vector3(-25, 0, -25); //实例化预制体的position，可自定义
    // Quaternion b = new Quaternion(0, 0, 0, 0);//实例化预制体的rotation，可自定义

    // Start is called before the first frame update
    void Start()
    {
        // TextAsset textAsset=Resources.Load("b") as TextAsset;
        
        // // 以换行符作为分割点，将该文本分割成若干行字符串，并以数组的形式来保存每行字符串的内容
        // string[] str = textAsset.text.Split('\n');
        // // 将该文本中的字符串输出
        // Debug.Log("str[0]= "+str[0]);
        // Debug.Log("str[1]= "+str[1]);
        // Debug.Log("str[2]= "+str[2]);
        // Debug.Log("str[3]= "+str[3]);
        // Debug.Log("str[4]= "+str[4]);
        // Debug.Log("str[5]= "+str[5]);
        // Debug.Log("str[6]= "+str[6]);
        // Debug.Log("str[7]= "+str[7]);
         string[,]  str=login_人机._maze;
         Debug.Log(str.Length);
      
        for (int i=0;i<str.GetLength(0);i++){
            for(int j=0;j<str.GetLength(1);j++){
                if (str[i,j]=="1"){
                  GameObject Sphere = GameObject.Instantiate(chair,a+new Vector3(i,0,j),chair.transform.rotation) as GameObject;  
                }
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
