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
    public Text error;

    public void Exit()
    {
        Debug.Log(123456);
        Application.Quit();
    }

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
                GameManager.UserName = id.text;
                GameManager.PullUserData();
                SceneManager.LoadScene(1);
            }
            else
            {
                Debug.Log("Wrong userID or password!");
                ServerConnector.CloseConnection();
                error.text = "Wrong userID or password!";
            }
        }
        else
        {
            Debug.Log("Connect fail!");
            ServerConnector.CloseConnection();
            error.text = "Connect fail!";
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)||Input.GetMouseButtonDown(1))
        {
            error.text = "";
        }
    }
}
// 0 Login
// 1 AIScript
// 2 PlayTo