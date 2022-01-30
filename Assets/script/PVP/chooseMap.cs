using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using System;
public class chooseMap : MonoBehaviour
{
   public static string[,] _maze_network;
   public static void choose()
    {
         int id = 11911419;
         int width = 50;
         int height = 50;
         Process process = new Process();
         process.StartInfo.FileName = @"C:\Users\admin\anaconda3\python.exe";
         string path = Application.dataPath;
         path += "/script/python_interface/py_import_test.py";
         path = path + " --id " + id.ToString()+" --width "+width+" --height "+height;
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
         if (_maze_network[0,0]=="-2"){
                   SceneManager.LoadScene(2);
         }
        UnityEngine.Debug.Log("now");
        UnityEngine.Debug.Log(_maze_network);

    }
static void dataReceiver(object sender, DataReceivedEventArgs e)
 {
     if (!string.IsNullOrEmpty(e.Data))
     {
         string raw_data = e.Data;
         string[] comma_split = raw_data.Split(',');
         int height = comma_split.Length;
         string[] space_split = comma_split[0].Split(' ');
         int width = space_split.Length;
         _maze_network = new string[height, width];
         for (int j = 0; j < width; j++)
         {
             _maze_network[0, j] = space_split[j];
         }
         for (int i = 1; i < height; i++)
         {
             space_split = comma_split[i].Split(' ');
             for (int j = 0; j < width; j++)
             {
                 _maze_network[i, j] = space_split[j];
             }
         }
     }
 }
    // Update is called once per frame
    void Update()
    {
        
    }
}
