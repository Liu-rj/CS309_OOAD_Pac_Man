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
    public GameObject suckball;
    public GameObject accelerate_ball;

    Vector3 a = new Vector3(-25, 0.5f, -25); //实例化预制体的position，可自定义
    // Quaternion b = new Quaternion(0, 0, 0, 0);//实例化预制体的rotation，可自定义

    // Start is called before the first frame update
    void Start()
    {
        string[,] str = login_人机._maze;

        for (int i = 0; i < str.GetLength(0); i++)
        {
            for (int j = 0; j < str.GetLength(1); j++)
            {
                if (str[i, j] == "1")//obstacle
                {
                    GameObject Chair = GameObject.Instantiate(chair,
                        a + new Vector3(i, 0, j) + new Vector3(0.5f, 0, 0.5f), chair.transform.rotation) as GameObject;
                }

                if (str[i, j] == "-1")//food
                {
                    GameObject Food = GameObject.Instantiate(food,
                        a + new Vector3(i, 0, j) + new Vector3(0.5f, 0, 0.5f), food.transform.rotation) as GameObject;
                }

                if (str[i, j] == "2")//bigball
                {
                    GameObject Strong_tool = GameObject.Instantiate(suckball,
                        a + new Vector3(i, 0, j) + new Vector3(0.5f, 0, 0.5f),
                        suckball.transform.rotation) as GameObject;
                }
                
                if (str[i, j] == "3")//suckball
                {
                    GameObject suck = GameObject.Instantiate(strong_tool,
                        a + new Vector3(i, 0, j) + new Vector3(0.5f, 0, 0.5f),
                        strong_tool.transform.rotation) as GameObject;
                }

                if (str[i, j] == "4")//ghost
                {
                    GameObject Ghost = GameObject.Instantiate(ghost,
                        a + new Vector3(i, 0, j) + new Vector3(0.5f, 0, 0.5f), ghost.transform.rotation) as GameObject;
                }
                
                if (str[i, j] == "5")//accelerate_ball
                {
                    GameObject accball = GameObject.Instantiate(accelerate_ball,
                        a + new Vector3(i, 0, j) + new Vector3(0.5f, 0, 0.5f),
                        accelerate_ball.transform.rotation) as GameObject;
                }
            }
        }
    }
}