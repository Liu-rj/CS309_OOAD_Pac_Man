using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ServerConnector : MonoBehaviour
{
    private static readonly string _serverAddress = "10.20.100.145";
    private static readonly int _serverPort = 5000;
    private static TcpClient _client;
    private static NetworkStream _stream; // C#中采用NetworkStream的方式, 可以类比于python网络编程中的socket
    
    public static bool IsConnected;

    private void OnApplicationQuit()
    {
        CloseConnection();
    }

    public static void SetupConnection()
    {
        try
        {
            _client = new TcpClient(_serverAddress, _serverPort);
            _stream = _client.GetStream();
            IsConnected = true;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            CloseConnection();
        }
    }

    public static string ReceiveData()
    {
        Debug.Log("Entered ReceiveData function...");
        if (IsConnected && _stream.CanRead)
        {
            try
            {
                var buffer = new byte[1024];
                var numberOfBytesRead = _stream.Read(buffer, 0, buffer.Length);
                var receiveMsg = Encoding.UTF8.GetString(buffer, 0, numberOfBytesRead);
                // _stream.Flush();
                Debug.Log(receiveMsg);
                return receiveMsg;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                // CloseConnection();
            }
        }

        return "";
    }

    public static void SendData(string msgToSend)
    {
        byte[] bytesToSend = Encoding.UTF8.GetBytes(msgToSend);
        if (_stream.CanWrite)
        {
            _stream.Write(bytesToSend, 0, bytesToSend.Length);
        }
    }

    public static void CloseConnection()
    {
        if (IsConnected)
        {
            _stream.Close();
            _client.Close();
            IsConnected = false;
        }
    }
}