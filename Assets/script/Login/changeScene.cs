using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour
{
    public void jumpto(int target)
    {
        if (target == 0)
        {
            ServerConnector.CloseConnection();
        }
        SceneManager.LoadScene(target);
    }
}
