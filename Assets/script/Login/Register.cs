using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public InputField register_id; //email
    public InputField register_username;
    public InputField register_pwd;

    public InputField verify_code;

    public void OnSendVerifyCodeButtonClick(Text error)
    {
        ServerConnector.SetupConnection();
        ServerConnector.SendData("4");
        var signal = ServerConnector.ReceiveData();
        if (signal == "y")
        {
            ServerConnector.SendData(register_username.text + " " + register_id.text + " " + register_pwd.text);
            var str = ServerConnector.ReceiveData();
            if (str == "y")
            {
                error.text = "Have sent verify code to you";
            }
            else
            {
                error.text = "send email failed";
                ServerConnector.CloseConnection();
            }
        }
        else
        {
            Debug.Log(signal);
            ServerConnector.CloseConnection();
        }
    }

    public void OnRegisterButtonClick(Text error)
    {
        ServerConnector.SendData("y");
        ServerConnector.SendData(verify_code.text);
        var signal = ServerConnector.ReceiveData();
        if (signal == "y")
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            error.text = "Wrong verify code";
        }
    }

    public void OnQuitButtonClick()
    {
        if (ServerConnector.IsConnected)
        {
            ServerConnector.SendData("q");
            ServerConnector.CloseConnection();
        }

        SceneManager.LoadScene(0);
    }
}