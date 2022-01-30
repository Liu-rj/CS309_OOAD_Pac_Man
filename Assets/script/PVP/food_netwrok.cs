using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class food_netwrok : MonoBehaviour
{
    // Start is called before the first frame update
    [PunRPC]
    public void food_destroy(){
        Debug.Log(this.gameObject.GetComponent<PhotonView>());
        Debug.Log("food");
Destroy(gameObject);
    }
}