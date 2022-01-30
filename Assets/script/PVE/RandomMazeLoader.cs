using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMazeLoader : MonoBehaviour
{
    public GameObject pacman;
    public GameObject blinky;
    public GameObject clyde;
    public GameObject inky;
    public GameObject pinky;
    public GameObject walls;
    public GameObject strongTool;
    public GameObject food;
    public GameObject suckBall;
    public GameObject accBall;
    public static int food_number = 0;
    private string[,] _maze;
    private GameObject[] _players;
    private int pacman_pos_in_maze = 0;
    private Vector3 pacman_pos = new Vector3(0, 0, 0);
    public Canvas cv_exit;
    
    // Start is called before the first frame update
    void Start()
    {
        cv_exit.gameObject.SetActive(false);
        _players = new[] {blinky, clyde, inky, pinky, pacman};
        _maze = MazeReceiver.ReceiveMaze();
        food_number = 0;//!!!init food number since it is a static variable by plk
        DrawMaze();
        init_pacman(); //!!!init pacman after the maze by plk
    }
    
    private void PauseGame()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i < 4)
            {
                _players[i].GetComponent<GhostMove>().enabled = false;
            }
            else
            {
                _players[i].GetComponent<PacmanMove>().enabled = false;
            }
        }
    }
    
    public void ClickOnBack()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i < 4)
            {
                _players[i].GetComponent<GhostMove>().enabled = true;
            }
            else
            {
                _players[i].GetComponent<PacmanMove>().enabled = true;
            }
        }
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            PauseGame();
            cv_exit.gameObject.SetActive(true);
        }
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
    // a suckBall
    // b accelerate_ball
    private void DrawMaze()
    {
        for (int i = 0; i < _maze.GetLength(0); i++)
        {
            for (int j = 0; j < _maze.GetLength(1); j++)
            {
                if (_maze[i, j] == "1")
                {
                    var obj = Instantiate(walls, new Vector3(j, 0.5f, -i), walls.transform.rotation);
                    obj.SetActive(true);
                }
                else if (_maze[i, j] == "2")
                {
                    var obj = Instantiate(strongTool, new Vector3(j, 0.5f, -i), strongTool.transform.rotation);
                    obj.SetActive(true);
                }
                else if (_maze[i, j] == "3")
                {
                    var obj = Instantiate(food, new Vector3(j, 0.5f, -i), food.transform.rotation);
                    obj.SetActive(true);
                    food_number++;
                }
                else if (_maze[i, j] == "a")
                {
                    var obj = Instantiate(suckBall, new Vector3(j, 0.5f, -i), food.transform.rotation);
                    obj.SetActive(true);
                }
                else if (_maze[i, j] == "b")
                {
                    var obj = Instantiate(accBall, new Vector3(j, 0.5f, -i), food.transform.rotation);
                    obj.SetActive(true);
                }
                else if (int.Parse(_maze[i, j]) > 0 && int.Parse(_maze[i, j]) < 9)
                {
                    var pos = new Vector3(j, 0.5f, -i);
                    if (int.Parse(_maze[i, j]) < 8)
                    {
                        _players[int.Parse(_maze[i, j]) - 4].GetComponent<GhostMove>().Init(pos, 28, 31);
                    }
                    else
                    {
                        //!!! only record the value by plk
                        pacman_pos_in_maze = int.Parse(_maze[i, j]) - 4;
                        pacman_pos = new Vector3(j, 0.5f, -i);
                        //_players[int.Parse(_maze[i, j]) - 4].GetComponent<PacmanMove>().Init(pos, 28, 31);
                    }
                }
            }
        }
    }
    //!!! init pacman by plk
    private void init_pacman()
    {
        _players[pacman_pos_in_maze].GetComponent<PacmanMove>().Init(pacman_pos, 28, 31);
    }
}
