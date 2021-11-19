using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarFog : MonoBehaviour
{
    [SerializeField]
    //拖到子摄像机上的Render Texture  
    private RenderTexture mask;  
  
    [SerializeField]  
    //创建的材质球   需要用到WarFog sharder  在下面给出  
    private Material mat;  
    //在图像渲染之后执行  
    public void OnRenderImage(RenderTexture source,RenderTexture des)  
    {  
        //将遮罩的mask传入材质球  
        mat.SetTexture("_MaskTex",mask);  
        //经过材质球的sharder变换后  拷贝源纹理到目的渲染纹理。  
        Graphics.Blit(source,des,mat);  
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
