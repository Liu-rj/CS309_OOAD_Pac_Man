using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // TODO: Merge into main version
    public static string UserName;
    public static int Score;
    public static int Coin;
    public static int Diamond;
    public static int EmpValue;
    public static int Level;
    
    public static void PushUserData()
    {
        ServerConnector.SendData("6");
        var signal = ServerConnector.ReceiveData();
        if (signal == "y")
        {
            var sendData = Score + " ";
            sendData += Coin + " ";
            sendData += Diamond + " ";
            sendData += EmpValue + " ";
            sendData += Level;
            Debug.Log(sendData);
            ServerConnector.SendData(sendData);
        }
    }

    public static void PullUserData()
    {
        ServerConnector.SendData("7");
        var signal = ServerConnector.ReceiveData();
        if (signal == "y")
        {
            
            var rawData = ServerConnector.ReceiveData();
            var seg = rawData.Split(' ');
            Score = int.Parse(seg[0]);
            Coin = int.Parse(seg[1]);
            Diamond = int.Parse(seg[2]);
            EmpValue = int.Parse(seg[3]);
            Level = int.Parse(seg[4]);
        }
        Debug.Log("Level: " + Level);
    }
}
