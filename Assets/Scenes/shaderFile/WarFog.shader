// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/WarFog" {    
    Properties {    
        _MainTex ("_MainTex", 2D) = "white" {}    
        //遮罩纹理     
        _MaskTex ("_MaskTex", 2D) = "white" {}  
    }    
    SubShader {    
        Pass {    
            CGPROGRAM    
            //顶点处理器  
            #pragma vertex m_vert_img    
            //片段处理器  
            #pragma fragment frag    
                 
            #include "UnityCG.cginc"    
                 
            uniform sampler2D _MainTex;    
            uniform sampler2D _MaskTex;   
                 
               struct m_appdata_img {  
                float4 vertex : POSITION;  
                half2 texcoord : TEXCOORD0;  
                half2 texcoord1 : TEXCOORD1;  
                };  
                struct m_v2f_img {  
                float4 pos : SV_POSITION;  
                half2 uv : TEXCOORD0;  
                half2 uv1 : TEXCOORD1;  
                };  
            //像素处理器  
            fixed4 frag(m_v2f_img i) : COLOR    
            {    
                //主纹理  
                fixed4 renderTex = tex2D(_MainTex, i.uv);  
                //遮罩纹理  
                fixed4 renderTex1 = tex2D(_MaskTex, i.uv1);   
                fixed4 finalColor;  
                //如果遮罩的红色通道小于0.3  最终颜色就是遮罩图片   否则为主图片  
                if(renderTex1.r<.3){  
                    finalColor = renderTex1.rgba;  
                }else{  
                    finalColor = renderTex.rgba;  
                }   
                     
                return finalColor;    
            }    
    //调整UV值   实现正常偏转  
    float2 m_MultiplyUV (float4x4 mat, float2 inUV) {  
    float4 temp = float4 (inUV.x, 1-inUV.y, 0, 0);  
    temp = mul (mat, temp);  
    return temp.xy;  
    }  
  
  
m_v2f_img m_vert_img( m_appdata_img v )  
{  
    m_v2f_img o;  
    o.pos = UnityObjectToClipPos (v.vertex);  
    o.uv = MultiplyUV( UNITY_MATRIX_TEXTURE0, v.texcoord );  
    o.uv1 = m_MultiplyUV( UNITY_MATRIX_TEXTURE0, v.texcoord1 );  
    return o;  
}  
            ENDCG    
        }    
    }    
    FallBack "Diffuse"    
}  