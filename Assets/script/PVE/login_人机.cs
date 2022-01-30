using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class login_人机 : MonoBehaviour
{
    public static string[,] _maze;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void OnLoginButtonClick()
    {
        int id = 11911419;
        int width = 50;
        int height = 50;
        Process process = new Process();
        process.StartInfo.FileName = @"python.exe";
        string path = Application.dataPath;
        path += "/script/python_interface/py_import_test.py";
        path = path + " --id " + id.ToString() + " --width " + width + " --height " + height;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.Arguments = path;
        process.Start();
        process.BeginOutputReadLine();
        process.OutputDataReceived += new DataReceivedEventHandler(dataReceiver);
        Console.ReadLine();
        process.WaitForExit();
        if (_maze[0, 0] == "-2")
        {
            SceneManager.LoadScene(2);
        }

        SceneManager.LoadScene(3);
    }

    void dataReceiver(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            string raw_data = e.Data;
            string[] comma_split = raw_data.Split(',');
            int height = comma_split.Length;
            string[] space_split = comma_split[0].Split(' ');
            int width = space_split.Length;
            _maze = new string[height, width];
            for (int j = 0; j < width; j++)
            {
                _maze[0, j] = space_split[j];
            }

            for (int i = 1; i < height; i++)
            {
                space_split = comma_split[i].Split(' ');
                for (int j = 0; j < width; j++)
                {
                    _maze[i, j] = space_split[j];
                }
            }
        }
    }

    public void OnButtonClickEVE()
    {
        SceneManager.LoadScene(7);
    }
}