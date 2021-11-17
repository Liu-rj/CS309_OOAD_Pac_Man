using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Threading;
using UnityEngine.SceneManagement;

public class EVEManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject blinky;
    public GameObject clyde;
    public GameObject inky;
    public GameObject pinky;
    public GameObject walls;
    public GameObject strong_tool;
    public GameObject food;
    public static string[,] Maze;
    public static int DotNum;
    public static bool Moving;

    private int _id1;
    private int _id2;
    private GameObject[] _players;
    private int _index;
    private GameObject _curPlayer;

    // Start is called before the first frame update
    void Start()
    {
        _id1 = 11911419;
        _id2 = EVEChoose.GETOpponent();
        player1.GetComponent<EVEPlayerMove>().Init(_id1, _id2, 10, player1.transform.position);
        player2.GetComponent<EVEPlayerMove>().Init(_id2, _id1, 11, player2.transform.position);
        _players = new[] {player1, player2, blinky, clyde, inky, pinky};
        _index = 0;
        _curPlayer = _players[_index];
        MazeReceiver.ReceiveMaze();
        AdjustMaze();
        DrawMaze();
        CalDotNum();
        Moving = false;
        // StartCoroutine(UpdateMap());
    }

    IEnumerator UpdateMap()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(9);
    }

    private void Update()
    {
        if (DotNum == 0)
        {
            Debug.Log("Game Over!!!");
            Debug.Log("player1 score: " + player1.GetComponent<EVEPlayerMove>().score);
            Debug.Log("player2 score: " + player2.GetComponent<EVEPlayerMove>().score);
        }

        if (!Moving)
        {
            Moving = true;
            if (_index == 0 || _index == 1)
            {
                WriteMaze(_curPlayer.GetComponent<EVEPlayerMove>().id, _curPlayer.GetComponent<EVEPlayerMove>().opp);
                _curPlayer.GetComponent<EVEPlayerMove>().Move();
            }
            else
            {
                _curPlayer.GetComponent<EVEGhostMove>().Move();
            }
        
            _index = (_index + 1) % 6;
            _curPlayer = _players[_index];
        }
    }

    private void CalDotNum()
    {
        for (int i = 0; i < Maze.GetLength(0); i++)
        {
            for (int j = 0; j < Maze.GetLength(1); j++)
            {
                if (Maze[i, j] == "-1")
                {
                    DotNum += 1;
                }
            }
        }
    }

    private string[,] FillMaze(int x, int y, string[,] maze)
    {
        var dirs = new[] {Tuple.Create(1, 0), Tuple.Create(-1, 0), Tuple.Create(0, 1), Tuple.Create(0, -1)};
        foreach (var dir in dirs)
        {
            var i = x + dir.Item1;
            var j = y + dir.Item2;
            if (i >= 0 && j >= 0 && i < 32 && j < 28 && maze[i, j] == "0")
            {
                maze[i, j] = "1";
                FillMaze(i, j, maze);
            }
        }

        return maze;
    }

    private void AdjustMaze()
    {
        var maze = new string[31, 28];
        for (int i = 0; i < Maze.GetLength(0); i++)
        {
            for (int j = 0; j < Maze.GetLength(1); j++)
            {
                if (Maze[i, j] == "1")
                {
                    maze[i, j] = "1";
                }
                else
                {
                    maze[i, j] = "0";
                }
            }
        }

        var afterFill = FillMaze(1, 1, maze);
        for (int i = 0; i < afterFill.GetLength(0); i++)
        {
            for (int j = 0; j < afterFill.GetLength(1); j++)
            {
                if (afterFill[i, j] == "0")
                {
                    Maze[i, j] = "1";
                }
            }
        }

        var position = player1.transform.position;
        Maze[(int) -position.z, (int) position.x] = "10";
        position = player2.transform.position;
        Maze[(int) -position.z, (int) position.x] = "11";
        for (int i = 2; i < _players.Length; i++)
        {
            position = _players[i].transform.position;
            Maze[(int) -position.z, (int) position.x] = "4";
        }

        Maze[1, 1] = "-1";
        Maze[1, 26] = "2";
        Maze[29, 1] = "2";
        Maze[29, 26] = "-1";
    }

    private void DrawMaze()
    {
        for (int i = 0; i < Maze.GetLength(0); i++)
        {
            for (int j = 0; j < Maze.GetLength(1); j++)
            {
                if (Maze[i, j] == "1")
                {
                    Instantiate(walls, new Vector3(j, 0, -i), walls.transform.rotation);
                }

                if (Maze[i, j] == "-1")
                {
                    Instantiate(food, new Vector3(j, 0, -i), food.transform.rotation);
                }

                if (Maze[i, j] == "2")
                {
                    Instantiate(strong_tool, new Vector3(j, 0, -i), strong_tool.transform.rotation);
                }
            }
        }
    }

    private void WriteMaze(int id1, int id2)
    {
        BinaryWriter bw;
        String path = Environment.CurrentDirectory;
        path += "/Data/" + id1 + "_" + id2;
        try
        {
            bw = new BinaryWriter(new FileStream(path,
                FileMode.Create));
        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message + "\n Cannot create file.");
            return;
        }

        try
        {
            for (int i = 0; i < Maze.GetLength(0); i++)
            {
                for (int j = 0; j < Maze.GetLength(1); j++)
                {
                    bw.Write(Int32.Parse(Maze[i, j]));
                }
            }
        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message + "\n Cannot write to file.");
        }

        bw.Close();
    }
}