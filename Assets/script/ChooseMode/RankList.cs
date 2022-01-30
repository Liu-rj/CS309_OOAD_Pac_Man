using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankList : MonoBehaviour
{
    public static Text[] ranks = new Text[5];

    public static void SetRank()
    {
        var rawData = ServerConnector.ReceiveData();
        var seg = rawData.Split(' ');
        for (int i = 0; i < 5; i++)
        {
            ranks[i].text = seg[0] + " " + seg[1];
        }
    }
}
