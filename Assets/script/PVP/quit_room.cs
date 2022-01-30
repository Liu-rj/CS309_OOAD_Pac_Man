using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class quit_room : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
         if (PhotonNetwork.IsConnected) { PhotonNetwork.LeaveRoom(); }
    }

  
}
