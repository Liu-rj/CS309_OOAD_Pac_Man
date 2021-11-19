using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField id;
    public InputField pwd;

    public void OnLoginButtonClick()
    {
        ServerConnector.SetupConnection();
        ServerConnector.SendData("0");
        var signal = ServerConnector.ReceiveData();
        if (signal == "y")
        {
            ServerConnector.SendData(id.text + " " + pwd.text);
            var str = ServerConnector.ReceiveData();
            if (str == "y")
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                Debug.Log("Username or Password failed!");
                Debug.Log(str);
                ServerConnector.CloseConnection();
            }
        }
        else
        {
            Debug.Log(signal);
            ServerConnector.CloseConnection();
        }
    }
}
// 0 Login
// 1 AIScript
// 2 PlayTo