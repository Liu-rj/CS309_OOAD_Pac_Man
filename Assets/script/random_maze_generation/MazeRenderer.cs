using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    public GameObject food;
    public GameObject ghost;
   
    [SerializeField]
    [Range(1, 50)]
    private int width = 10;
     public static int tot_score=0;

    [SerializeField]
    [Range(1, 50)]
    private int height = 10;

    [SerializeField]
    private float size = 1f;

    [SerializeField]
    private Transform wallPrefab = null;

    [SerializeField]
    private Transform floorPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        var maze = MazeGenerator.Generate(width, height);
        Draw(maze);
    }

    private void Draw(WallState[,] maze)
    {

        var floor = Instantiate(floorPrefab, transform);
        floor.localScale = new Vector3(width, 1, height);
        var rng = new System.Random((int)DateTime.Now.Ticks);
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                var cell = maze[i, j];
                var position = new Vector3(-width / 2 + i, 0, -height / 2 + j);
                if (i==width-1 && j==height-1){
                    // GameObject Player = GameObject.Instantiate(player,position,player.transform.rotation) as GameObject; 
                          GameObject Ghost = GameObject.Instantiate(ghost,position,ghost.transform.rotation) as GameObject;
                }else{
                    GameObject Food = GameObject.Instantiate(food,position,food.transform.rotation) as GameObject;  
                    tot_score+=1;
                }
                var test = rng.Next(0, 10);
                if (i > 0 && i < width - 1 && j > 0 && j < height - 1)
                {
                    if (test > 6)
                    {
                        continue;
                    }
                }
                if (cell.HasFlag(WallState.UP))
                {
                    var topWall = Instantiate(wallPrefab, transform) as Transform;
                    
                    topWall.position = position + new Vector3(0, 0, size / 2);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                }

                if (cell.HasFlag(WallState.LEFT))
                {
                    var leftWall = Instantiate(wallPrefab, transform) as Transform;
                    
                    leftWall.position = position + new Vector3(-size / 2, 0, 0);
                    leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                }

                if (i == width - 1)
                {
                    if (cell.HasFlag(WallState.RIGHT))
                    {
                        var rightWall = Instantiate(wallPrefab, transform) as Transform;
                        
                        rightWall.position = position + new Vector3(+size / 2, 0, 0);
                        rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);
                    }
                }

                if (j == 0)
                {
                    if (cell.HasFlag(WallState.DOWN))
                    {
                       
                        var bottomWall = Instantiate(wallPrefab, transform) as Transform;
                        bottomWall.position = position + new Vector3(0, 0, -size / 2);
                        bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                    }
                }
            }

        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}