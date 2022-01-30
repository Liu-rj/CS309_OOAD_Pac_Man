using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class bigball_network : MonoBehaviour
{
    // Start is called before the first frame update
    [PunRPC]
 public void  bigball_destroy(){
     Destroy(gameObject);
 }
}
