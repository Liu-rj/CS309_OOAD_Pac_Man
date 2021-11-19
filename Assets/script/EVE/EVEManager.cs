using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class EVEManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject blinky;
    public GameObject clyde;
    public GameObject inky;
    public GameObject pinky;
    public GameObject walls;
    public GameObject strongTool;
    public GameObject food;
    public static bool Moving;

    private GameObject[] _players;
    private Thread _thread;
    private Queue<string> _msgQ;
    private bool _end;
    private string[,] _maze;

    // Start is called before the first frame update
    void Start()
    {
        _players = new[] {blinky, clyde, inky, pinky, player1, player2};
        _maze = MazeReceiver.ReceiveMaze();
        DrawMaze();
        Moving = false;
        _msgQ = new Queue<string>();
        _end = false;
        _thread = new Thread(ReceiveData)
        {
            IsBackground = true
        };
        _thread.Start();
    }

    private void SetGameOver()
    {
        _end = true;
        for (int i = 0; i < 6; i++)
        {
            if (i < 4)
            {
                _players[i].GetComponent<EVEGhostMove>().enabled = false;
            }
            else
            {
                _players[i].GetComponent<EVEPlayerMove>().enabled = false;
            }
        }
    }

    private void Update()
    {
        if (!Moving && _msgQ.Count > 0 && !_end)
        {
            var str = _msgQ.Dequeue().Split(' ');
            var signal = int.Parse(str[0]);
            if (signal < 4)
            {
                var pos = new Vector3(int.Parse(str[2]), 0.5f, -int.Parse(str[1]));
                _players[signal].GetComponent<EVEGhostMove>().MoveTo(pos);
            }
            else if (signal < 6)
            {
                var pos = new Vector3(int.Parse(str[2]), 0.5f, -int.Parse(str[1]));
                _players[signal].GetComponent<EVEPlayerMove>().MoveTo(pos);
            }
            else
            {
                SetGameOver();
                switch (signal)
                {
                    case 6:
                        Debug.Log(str[1] + " " + str[2]);
                        break;
                    case 7:
                        Debug.Log("Pacman " + str[1] + " Illegal Move");
                        break;
                    case 8:
                        Debug.Log(str[1] + "Game Over!");
                        break;
                }
            }
        }
    }

    private void ReceiveData()
    {
        if (ServerConnector.IsConnected)
        {
            ServerConnector.SendData("y");
            while (ServerConnector.IsConnected)
            {
                var str = ServerConnector.ReceiveData();
                if (str != "")
                {
                    _msgQ.Enqueue(str);
                    ServerConnector.SendData("y");
                    var signal = int.Parse(str[0].ToString());
                    if (signal >= 6)
                    {
                        return;
                    }
                }
            }
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
                        _players[int.Parse(_maze[i, j]) - 4].GetComponent<EVEGhostMove>().Init(pos);
                    }
                    else
                    {
                        _players[int.Parse(_maze[i, j]) - 4].GetComponent<EVEPlayerMove>().Init(pos);
                    }
                }
            }
        }
    }
}