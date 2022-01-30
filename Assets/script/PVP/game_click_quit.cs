using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using  Photon.Pun;
public class game_click_quit : MonoBehaviour
{
    // Start is called before the first frame update
   // public void clickquit(){
   //     SceneManager.LoadScene(2);
   // }
   public Canvas cv_exit;

   private void Start()
   {
       cv_exit.gameObject.SetActive(false);
   }

   private void Update()
   {
       if (Input.GetKey(KeyCode.Escape))
       {
           cv_exit.gameObject.SetActive(true);
       }
   }

   public void clickquit(){
       PhotonView photonView;
       Debug.Log((GameObject.Find("PacMan_network(Clone)").transform.position.x));
       while (true){
           photonView=GameObject.Find("PacMan_network(Clone)").GetComponent<PhotonView>();
           if (photonView.IsMine){
               photonView.RPC("gogogo",RpcTarget.All);
               break;
           }else{ 
               break;
           }
       }

       SceneManager.LoadScene(2);
   }
}
