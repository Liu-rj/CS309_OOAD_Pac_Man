using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Threading;

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

    private Vector3 _a = new Vector3(-25, 0, -25);
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
        LoadMap();
        CalDotNum();
        player1.GetComponent<EVEPlayerMove>().Init(_id1, _id2, 10, new Vector3(-24.5f, 0.5f, -0.5f));
        player2.GetComponent<EVEPlayerMove>().Init(_id2, _id1, 11, new Vector3(24.5f, 0.5f, -0.5f));
        _players = new[] {player1, blinky, clyde, inky, pinky, player2, blinky, clyde, inky, pinky};
        _index = 0;
        _curPlayer = _players[_index];
        Moving = false;
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
            if (_index == 0 || _index == 5)
            {
                WriteMaze(_curPlayer.GetComponent<EVEPlayerMove>().id, _curPlayer.GetComponent<EVEPlayerMove>().opp);
                _curPlayer.GetComponent<EVEPlayerMove>().Move();
            }
            else
            {
                _curPlayer.GetComponent<EVEGhostMove>().Move();
            }

            _index = (_index + 1) % 10;
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

    private void LoadMap()
    {
        //TODO: Use Random to initialize map
        int id = 11911419;
        int width = 50;
        int height = 50;
        Process process = new Process();
        process.StartInfo.FileName = @"python.exe";
        string path = Application.dataPath;
        path += "/script/python_interface/py_import_test.py";
        path = path + " --id " + id + " --width " + width + " --height " + height;
        // Debug.Log(path);
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.Arguments = path;
        process.Start();
        process.BeginOutputReadLine();
        process.OutputDataReceived += DataReceiver;
        Console.ReadLine();
        process.WaitForExit();
        string[,] str = Maze;

        for (int i = 0; i < str.GetLength(0); i++)
        {
            for (int j = 0; j < str.GetLength(1); j++)
            {
                if (str[i, j] == "1")
                {
                    GameObject Wall = GameObject.Instantiate(walls,
                        _a + new Vector3(i, 0, j) + new Vector3(0.5f, 0, 0.5f), walls.transform.rotation) as GameObject;
                }

                if (str[i, j] == "-1")
                {
                    GameObject Food = GameObject.Instantiate(food,
                        _a + new Vector3(i, 0, j) + new Vector3(0.5f, 0, 0.5f), food.transform.rotation) as GameObject;
                }

                if (str[i, j] == "2")
                {
                    GameObject Strong_tool = GameObject.Instantiate(strong_tool,
                        _a + new Vector3(i, 0, j) + new Vector3(0.5f, 0, 0.5f),
                        strong_tool.transform.rotation) as GameObject;
                }
            }
        }
    }

    private void DataReceiver(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            string raw_data = e.Data;
            string[] comma_split = raw_data.Split(',');
            int height = comma_split.Length;
            string[] space_split = comma_split[0].Split(' ');
            int width = space_split.Length;
            Maze = new string[height, width];
            // Debug.Log(e.Data);
            for (int j = 0; j < width; j++)
            {
                Maze[0, j] = space_split[j];
            }

            for (int i = 1; i < height; i++)
            {
                space_split = comma_split[i].Split(' ');
                for (int j = 0; j < width; j++)
                {
                    Maze[i, j] = space_split[j];
                }
            }

            Maze[0, 0] = "4";
            Maze[49, 0] = "4";
            Maze[0, 49] = "4";
            Maze[49, 49] = "4";
            Maze[0, 24] = "10";
            Maze[49, 24] = "11";
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