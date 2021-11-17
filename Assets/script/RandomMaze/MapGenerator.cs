using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class MapGenerator : MonoBehaviour
{
    public static void Generate()
    {
        LoadMaze();
    }

    private static void LoadMaze()
    {
        Process process = new Process();
        process.StartInfo.FileName = @"python.exe";
        string path = Application.dataPath;
        path += "/script/python_interface/Maze_Generation.py";
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
    }
    
    private static void DataReceiver(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            string rawData = e.Data;
            char[] chars = rawData.ToCharArray();
            EVEManager.Maze = new string[22, 22];
            int index = 0;
            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 22; j++)
                {
                    EVEManager.Maze[i, j] = chars[index++].ToString();
                }
            }

            EVEManager.Maze[10, 11] = "4";
            EVEManager.Maze[10, 10] = "4";
            EVEManager.Maze[11, 11] = "4";
            EVEManager.Maze[11, 10] = "4";
            EVEManager.Maze[1, 1] = "10";
            EVEManager.Maze[20, 20] = "11";
        }
    }
}