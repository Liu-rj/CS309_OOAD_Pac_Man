using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class MazeReceiver : MonoBehaviour
{
    public static string[,] ReceiveMaze()
    {
        var rawData = ServerConnector.ReceiveData();
        var chars = rawData.ToCharArray();
        var maze = new string[31, 28];
        var index = 0;
        for (int i = 0; i < 31; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                maze[i, j] = chars[index++].ToString();
            }
        }

        return maze;
    }
}