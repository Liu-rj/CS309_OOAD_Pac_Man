using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;

public class networkPVP : MonoBehaviourPunCallbacks
{
    public static int State;
    public static int DotNum;
    public Text room_name;
    public static string[,] Maze;
    private bool rejoinCalled;

        private bool reconnectCalled;
        private bool inRoom;
         private DisconnectCause previousDisconnectCause;

    private string _mazestr = "1111111111111111111111111111100000003011000011030000000110111101101101101101101111011011110113110110113110111101100030311011011011011303000110110111100031130001111011011011011110110110110111101101101130000011011011000003110110111101101100001101101111011011110110111111110110111101120000011011111111011000002110111111130000000031111111011011111110111001110111111101100000000014000051000000000110111101101320023101101111011011113110160000710113111101100011011011111111011011000111101101100208002001101101111110110111110110111110110111111011011111011011111011011111100330000001100000033001111110111110111111110111110111111011111311111111311111011130030001103030030301100030031110110110111111110110110111111011311011111111011311011110031100300001100003001130011011111110110110110111111101101111111011011011011111110110003000001100001100000300011111111111111111111111111111";
    

// public Text pname;
    void Start()
    {
        Debug.Log("start is");
        State = 0;
        DotNum = 0;
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log(" connect no");
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("You have aleardy log in master server");
        }
        else
        {
            Debug.Log("in");
            PhotonNetwork.NetworkingClient.SerializationProtocol =
                ExitGames.Client.Photon.SerializationProtocol.GpBinaryV16;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void Connect()
    {
        Debug.Log(" connect no");
        // if (PhotonNetwork.IsConnected)
        // {
        //     Debug.Log("You have aleardy log in master server");
        // }
        // else
        // {
        //     Debug.Log("in");
        //     PhotonNetwork.NetworkingClient.SerializationProtocol =
        //         ExitGames.Client.Photon.SerializationProtocol.GpBinaryV16;
        //     PhotonNetwork.ConnectUsingSettings();
        // }
    }

    //连接成功后的回调函数
    public override void OnConnected()
    {
        Debug.Log("Connect successful!");

        base.OnConnected();
    }


    public void JoinRoom()
    {
        //如果还没有连上服务器
        if (!PhotonNetwork.IsConnected)
        {
            Connect();
        }
        else
        {
            RoomOptions roomOptions = new RoomOptions();
            // PhotonNetwork.JoinRandomRoom();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.JoinOrCreateRoom(room_name.text, roomOptions, TypedLobby.Default);
        }
          Maze = ParseMaze(_mazestr);
        Debug.Log("zzzzzzzzzzzzzzz");
    }

    //加入成功的回调函数
    public override void OnJoinedRoom()
    {   
       
        Debug.Log("Join successful!");
        if (PhotonNetwork.IsMasterClient)
        {

             State++;
            Debug.Log(State);
        }
      
      
        PhotonNetwork.LoadLevel(10);
          
       
        
       
        base.OnJoinedRoom();
    }

    private string[,] ParseMaze(string rawData)


    {
        var chars = rawData.ToCharArray();
        var maze = new string[31, 28];
        var index = 0;
        for (int i = 0; i < 31; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                maze[i, j] = chars[index++].ToString();
            }
        }

        return maze;
    }

    
}