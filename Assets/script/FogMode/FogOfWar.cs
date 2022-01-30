using System;
using UnityEngine;
using UnityEngine.UI;

public class FogOfWar : MonoBehaviour
{
    public RawImage fogRawImage;
    public MeshCollider planeMeshCollider;
    //public Transform cubeTransform;
    //public Rigidbody rd;
    public GameObject pacman;
    public GameObject pacman2;
    public float cubeMoveSpeed = 0.1f;

    public Vector2Int fogDensity = new Vector2Int(100, 100);
    public Vector2Int beEliminatedShapeSize = new Vector2Int(8, 6);
    private Vector2Int previousFogCenter;
    private Vector2Int previousFogCenter2;
    private Texture2D fogTexture;

    private Vector2Int[] shapeLocalPosition;

    private Vector2 planeOriginPoint;
    private Vector2 worldSize;

    // Start is called before the first frame update
    void Start()
    {
        //rd = GetComponent<Rigidbody>();
        fogTexture = new Texture2D(fogDensity.x, fogDensity.y);
        fogRawImage.texture = fogTexture;
        worldSize = new Vector2(planeMeshCollider.bounds.size.x, planeMeshCollider.bounds.size.z);
        // Debug.Log(worldSize);
        //  plane       ȥ   ߴ  һ  ,   ɵõ        ½ǵ     
        planeOriginPoint = new Vector2(planeMeshCollider.transform.position.x - worldSize.x * 0.5f, planeMeshCollider.transform.position.z - worldSize.y * 0.5f);
        // Debug.Log(planeOriginPoint);
        InitializeTheShape();
        InitializeTheFog();
        //EliminateFog();
        //EliminateFog2();
    }

    private void Update()
    {
        //if (Input.anyKey)
        //{
        //    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        //    {
        //        EliminateFog();
        //    }
        //}
        EliminateFog();
        EliminateFog2();
    }

    private void FixedUpdate()
    {
        RecoverFog();
        RecoverFog2();
    }

    void InitializeTheShape()
    {
        int pixelCount = beEliminatedShapeSize.x * beEliminatedShapeSize.y;
        shapeLocalPosition = new Vector2Int[pixelCount];

        int halfX = Mathf.FloorToInt(beEliminatedShapeSize.x * 0.5f);
        int remainingX = beEliminatedShapeSize.x - halfX;
        int halfY = Mathf.FloorToInt(beEliminatedShapeSize.y * 0.5f);
        int remainingY = beEliminatedShapeSize.y - halfY;

        int index = 0;
        for (int y = -halfY; y < remainingY; y++)
        {
            for (int x = -halfX; x < remainingX; x++)
            {
                shapeLocalPosition[index] = new Vector2Int(x, y);
                index++;
            }
        }
    }

    void InitializeTheFog()
    {
        int pixelCount = fogDensity.x * fogDensity.y;
        //       Ĭ    ɫ    Ϊ  ɫ
        Color[] blackColors = new Color[pixelCount];
        for (int i = 0; i < pixelCount; i++)
        {
            blackColors[i] = Color.black;
        }
        fogTexture.SetPixels(blackColors);

        fogTexture.Apply();
    }

    void EliminateFog()
    {
        Vector2 cubePos = new Vector2(pacman.transform.position.x, pacman.transform.position.z);
        // Debug.Log(cubePos);
        //  Լٶ ԭ  ľ      ,  Ϊ          ,          п    Ǹ   ,texture в    ڸ         ,    ת  Ϊ    .
        Vector2 originDistanceRatio = (cubePos - planeOriginPoint) / worldSize;
        // Debug.Log(originDistanceRatio);
        originDistanceRatio.Set(Mathf.Abs(originDistanceRatio.x), Mathf.Abs(originDistanceRatio.y));
        // Debug.Log(originDistanceRatio);
        //            ܶ ,    ֪  cube ൱  texture еĵ㼴 ɼ      
        Vector2Int fogCenter = new Vector2Int(Mathf.RoundToInt(originDistanceRatio.x * fogDensity.x), Mathf.RoundToInt(originDistanceRatio.y * fogDensity.y));
        previousFogCenter = fogCenter;
        //Vector2Int fogCenter = new Vector2Int(Mathf.RoundToInt(pacman.transform.position.x), Mathf.RoundToInt(pacman.transform.position.z));
        // Debug.Log(fogCenter);
        for (int i = 0; i < shapeLocalPosition.Length; i++)
        {
            int x = shapeLocalPosition[i].x + fogCenter.x;
            int y = shapeLocalPosition[i].y + fogCenter.y;
            //  Ϊ           ״ Ǳ cube  λ û Ҫ   ,     Ե  ʱ  ,         ص      ᳬ  texture  Χ,   Գ      ֺ   .
            if (x < 0 || x >= fogDensity.x || y < 0 || y >= fogDensity.y)
                continue;

            fogTexture.SetPixel(x, y, Color.clear);
        }

        fogTexture.Apply();
    }

    void RecoverFog()
    {
        for (int i = 0; i < shapeLocalPosition.Length; i++)
        {
            int x = shapeLocalPosition[i].x + previousFogCenter.x;
            int y = shapeLocalPosition[i].y + previousFogCenter.y;
            //  Ϊ           ״ Ǳ cube  λ û Ҫ   ,     Ե  ʱ  ,         ص      ᳬ  texture  Χ,   Գ      ֺ   .
            if (x < 0 || x >= fogDensity.x || y < 0 || y >= fogDensity.y)
                continue;

            fogTexture.SetPixel(x, y, Color.black);
        }

        fogTexture.Apply();
    }
    
    void EliminateFog2()
    {
        Vector2 cubePos = new Vector2(pacman2.transform.position.x, pacman2.transform.position.z);
        // Debug.Log(cubePos);
        //  Լٶ ԭ  ľ      ,  Ϊ          ,          п    Ǹ   ,texture в    ڸ         ,    ת  Ϊ    .
        Vector2 originDistanceRatio = (cubePos - planeOriginPoint) / worldSize;
        // Debug.Log(originDistanceRatio);
        originDistanceRatio.Set(Mathf.Abs(originDistanceRatio.x), Mathf.Abs(originDistanceRatio.y));
        // Debug.Log(originDistanceRatio);
        //            ܶ ,    ֪  cube ൱  texture еĵ㼴 ɼ      
        Vector2Int fogCenter = new Vector2Int(Mathf.RoundToInt(originDistanceRatio.x * fogDensity.x), Mathf.RoundToInt(originDistanceRatio.y * fogDensity.y));
        previousFogCenter2 = fogCenter;
        //Vector2Int fogCenter = new Vector2Int(Mathf.RoundToInt(pacman.transform.position.x), Mathf.RoundToInt(pacman.transform.position.z));
        // Debug.Log(fogCenter);
        for (int i = 0; i < shapeLocalPosition.Length; i++)
        {
            int x = shapeLocalPosition[i].x + fogCenter.x;
            int y = shapeLocalPosition[i].y + fogCenter.y;
            //  Ϊ           ״ Ǳ cube  λ û Ҫ   ,     Ե  ʱ  ,         ص      ᳬ  texture  Χ,   Գ      ֺ   .
            if (x < 0 || x >= fogDensity.x || y < 0 || y >= fogDensity.y)
                continue;

            fogTexture.SetPixel(x, y, Color.clear);
        }

        fogTexture.Apply();
    }

    void RecoverFog2()
    {
        for (int i = 0; i < shapeLocalPosition.Length; i++)
        {
            int x = shapeLocalPosition[i].x + previousFogCenter2.x;
            int y = shapeLocalPosition[i].y + previousFogCenter2.y;
            //  Ϊ           ״ Ǳ cube  λ û Ҫ   ,     Ե  ʱ  ,         ص      ᳬ  texture  Χ,   Գ      ֺ   .
            if (x < 0 || x >= fogDensity.x || y < 0 || y >= fogDensity.y)
                continue;

            fogTexture.SetPixel(x, y, Color.black);
        }

        fogTexture.Apply();
    }
}
