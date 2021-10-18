using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
public class my_movie : MonoBehaviour
{
    //电影纹理
    public VideoPlayer videoPlayer;
    // float video_count=0;
    // int video_time=19;

        public RawImage rawImage;
    public Text text_playOrPause;
    public Button button_playOrPasuse;
    // Start is called before the first frame update
    void Start()
    {
        button_playOrPasuse.onClick.AddListener(OnPlayOrPauseVideo);

    }

    // Update is called once per frame
    void Update()
    {
           //没有视频则返回，不播放

        if (videoPlayer.texture == null) {

            return;

        }

        //渲染视频到UGUI上
    //    video_count+=Time.deltaTime;
        rawImage.texture = videoPlayer.texture;
        // if (video_count==video_count-1){
        //     videoPlayer.Play();
        // }
    }

    private void OnPlayOrPauseVideo() {

        //判断视频播放情况，播放则暂停，暂停就播放，并更新相关文本

        if (videoPlayer.isPlaying == true) {

            videoPlayer.Pause();

            text_playOrPause.text = "播放";

        }

        else {

            videoPlayer.Play();

            text_playOrPause.text = "暂停";

        }

    }
}
