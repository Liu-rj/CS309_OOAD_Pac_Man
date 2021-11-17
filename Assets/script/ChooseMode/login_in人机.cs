using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class login_in人机 : MonoBehaviour
{
    AsyncOperation asyncOperation;
    private float culload = 0; //已加载的进度
    public Text loadtext; //百分制显示进度加载情况
    public Slider slider;

    void Start()
    {
        StartCoroutine(loadScene());
    }

    // Update is called once per frame
    void Update()
    {
        //  Text.GetComponent<Text>().text=(float)asyncOperation.progress*100+10+"%";
        // progress.GetComponent<Slider>().value=(float)asyncOperation.progress+.1f;

        //判断是否有场景正在加载
        if (asyncOperation == null)
        {
            return;
        }

        int progrssvalue = 0;
        //当场景加载进度在90%以下时，将数值以整数百分制呈现，当资源加载到90%时就将百分制进度设置为100，
        if (asyncOperation.progress < 0.9f)
        {
            progrssvalue = (int) asyncOperation.progress * 100;
        }
        else
        {
            progrssvalue = 100;
        }

        //每帧对进度条的图片和Text百分制数据进行更改，为了实现数字的累加而不是跨越，用另一个变量来实现。
        if (culload < progrssvalue)
        {
            culload++;
            // load.fillAmount = culload / 100f;
            loadtext.text = culload.ToString() + "%";
            slider.value = culload / 100;
        }

        //一旦进度到达100时，开启自动场景跳转，LoadSceneAsync会加载完剩下的10%的场景资源
        if (culload == 100)
        {
            asyncOperation.allowSceneActivation = true;
        }
    }

    IEnumerator loadScene()
    {
        asyncOperation = SceneManager.LoadSceneAsync(4);
        asyncOperation.allowSceneActivation = false;
        yield return asyncOperation;
    }
}