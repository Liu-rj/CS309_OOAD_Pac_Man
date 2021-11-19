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
    public InputField idField;
    private static int _opponentID = 0;

    // Start is called before the first frame update
    void Start()
    {
        _opponentID = 0;
    }

    public static int GETOpponent()
    {
        return _opponentID;
    }

    public void ClickChooseScriptButton()
    {
        var path = FolderBrowserHelper.SelectFile(FolderBrowserHelper.PYFILTER);
        Debug.Log(path);
        if (path != "")
        {
            try
            {
                var content = File.ReadAllText(path);
                ServerConnector.SendData("1");
                var signal = ServerConnector.ReceiveData();
                if (signal == "y")
                {
                    ServerConnector.SendData(content);
                }
                else
                {
                    Debug.Log(signal);
                    ServerConnector.CloseConnection();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        else
        {
            Debug.Log("Invalid File Path!");
        }
    }

    public void ClickOpponentButton()
    {
        var str = idField.text;
        try
        {
            _opponentID = int.Parse(str);
            ServerConnector.SendData("2");
            var signal = ServerConnector.ReceiveData();
            if (signal == "y")
            {
                ServerConnector.SendData(_opponentID.ToString());
                if (ServerConnector.ReceiveData() == "y")
                {
                    SceneManager.LoadScene(8);
                }
                else
                {
                    Debug.Log("Script Does Not Exists!");
                }
            }
            else
            {
                Debug.Log(signal);
                ServerConnector.CloseConnection();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}