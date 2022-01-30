using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class button_for_random_maze : MonoBehaviour
{

    public void OnLoginButtonClick()
    {
        ServerConnector.SendData("3");
        var signal = ServerConnector.ReceiveData();
        if (signal == "y")
        {
            SceneManager.LoadScene(6);
        }
    }
}
