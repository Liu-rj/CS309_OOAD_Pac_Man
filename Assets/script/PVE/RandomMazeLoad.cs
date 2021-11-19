using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMazeLoad : MonoBehaviour
{
    public GameObject pacman;
    public GameObject blinky;
    public GameObject clyde;
    public GameObject inky;
    public GameObject pinky;
    public GameObject walls;
    public GameObject strongTool;
    public GameObject food;
    
    private string[,] _maze;
    private GameObject[] _players;

    // Start is called before the first frame update
    void Start()
    {
        _players = new[] {blinky, clyde, inky, pinky, pacman};
        _maze = MazeReceiver.ReceiveMaze();
        DrawMaze();
    }

    // 0 empty
    // 1 wall
    // 2 strongTool
    // 3 dot
    // 4 ghost1 0
    // 5 ghost2 1
    // 6 ghost3 2
    // 7 ghost4 3
    // 8 pacman1 4
    // 9 pacman2 5
    private void DrawMaze()
    {
        for (int i = 0; i < _maze.GetLength(0); i++)
        {
            for (int j = 0; j < _maze.GetLength(1); j++)
            {
                if (_maze[i, j] == "1")
                {
                    var obj = Instantiate(walls, new Vector3(j, 0, -i), walls.transform.rotation);
                    obj.SetActive(true);
                }
                else if (_maze[i, j] == "2")
                {
                    var obj = Instantiate(strongTool, new Vector3(j, 0, -i), strongTool.transform.rotation);
                    obj.SetActive(true);
                }
                else if (_maze[i, j] == "3")
                {
                    var obj = Instantiate(food, new Vector3(j, 0, -i), food.transform.rotation);
                    obj.SetActive(true);
                }
                else if (_maze[i, j] != "0")
                {
                    var pos = new Vector3(j, 0, -i);
                    if (int.Parse(_maze[i, j]) < 8)
                    {
                        _players[int.Parse(_maze[i, j]) - 4].GetComponent<GhostMove>().Init(pos);
                    }
                    else
                    {
                        _players[int.Parse(_maze[i, j]) - 4].GetComponent<PacmanMove>().Init(pos);
                    }
                }
            }
        }
    }
}
