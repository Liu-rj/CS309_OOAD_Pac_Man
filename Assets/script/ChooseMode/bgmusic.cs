using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class bgmusic : MonoBehaviour
{   
    public AudioSource audioSource;
    // Start is called before the first frame update
   public void bg_music_on(){
        if (!audioSource.isPlaying){
            audioSource.Play();
        }else{
               audioSource.Stop();
        }
    }

}
