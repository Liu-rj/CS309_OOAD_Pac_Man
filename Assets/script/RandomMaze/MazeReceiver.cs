using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class MazeReceiver : MonoBehaviour
{
    public static void ReceiveMaze()
    {
        Process process = new Process();
        process.StartInfo.FileName = @"node.exe";
        string path = Application.dataPath;
        path += "/script/RandomMaze/MazeGenerator.js";
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
            EVEManager.Maze = new string[31, 28];
            int index = 0;
            for (int i = 0; i < 31; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    if (chars[index] == '|')
                    {
                        EVEManager.Maze[i, j] = "1";
                    }
                    else
                    {
                        EVEManager.Maze[i, j] = "0";
                    }

                    index += 1;
                }
            }
        }
    }
}