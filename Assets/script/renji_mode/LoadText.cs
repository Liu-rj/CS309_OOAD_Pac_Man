using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextAsset textAsset=Resources.Load("a") as TextAsset;
 
        // 以换行符作为分割点，将该文本分割成若干行字符串，并以数组的形式来保存每行字符串的内容
        string[] str = textAsset.text.Split('\n');
        // 将该文本中的字符串输出
        Debug.Log("str[0]= "+str[0]);
        Debug.Log("str[1]= "+str[1]);
        Debug.Log("str[2]= "+str[2]);
        Debug.Log("str[3]= "+str[3]);
        Debug.Log("str[4]= "+str[4]);
        Debug.Log("str[5]= "+str[5]);
        Debug.Log("str[6]= "+str[6]);
        Debug.Log("str[7]= "+str[7]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
