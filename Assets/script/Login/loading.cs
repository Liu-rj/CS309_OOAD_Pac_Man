using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loading : MonoBehaviour
{
    public Text ld_text;
    // Start is called before the first frame update
    private float _totaltime;
    private int state;
    public bool start_loading=true;
    void Start()
    {
        
    }

    public void setloading(bool state)
    {
        start_loading = state;
    }

    // Update is called once per frame
    void Update()
    {
        _totaltime += Time.deltaTime;
            if (_totaltime>0.2)
            {
                state = (state + 1) % 4;
                _totaltime = 0;
            }

        
            if (state==0)
            {
                ld_text.text = "loading";
            }else if (state==1)
            {
                ld_text.text = "loading.";
            }else if (state==2)
            {
                ld_text.text = "loading..";
            }else if (state==3)
            {
                ld_text.text = "loading...";
            }
    }
}
