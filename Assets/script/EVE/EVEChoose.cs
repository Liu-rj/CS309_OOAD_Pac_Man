using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using System.Diagnostics;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class EVEChoose : MonoBehaviour
{
    public InputField IDField;

    private static int _state = 0;

    private static int _opponentID = 0;

    // Start is called before the first frame update
    void Start()
    {
        _state = 0;
        _opponentID = 0;
    }

    public static int GETOpponent()
    {
        return _opponentID;
    }

    public void ClickChooseScriptButton()
    {
        int id = 11911400;
        Process process = new Process();
        process.StartInfo.FileName = @"python.exe";
        string path = Application.dataPath;
        path += "/script/python_interface/AI_selection.py";
        path = path + " --player " + id;
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

    void DataReceiver(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            string data = e.Data;
            if (data == "0")
            {
                _state = 0;
            }
            else
            {
                _state = 1;
            }
        }
        else
        {
            _state = 0;
        }
    }

    private bool isNumeric(string str)
    {
        try
        {
            int.Parse(str);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public void ClickOpponentButton()
    {
        string str = IDField.text;
        if (isNumeric(str))
        {
            _opponentID = int.Parse(str);
            string path = Environment.CurrentDirectory;
            path += "/UserScripts/AI_" + _opponentID + ".py";
            // Debug.Log(path);
            // TODO: check whether the user's file exists
            if (File.Exists(path))
            {
                SceneManager.LoadScene(8);
            }
            else
            {
                Debug.Log("File not found");
            }
        }
    }
}