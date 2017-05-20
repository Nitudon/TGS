using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonCommons;
using SystemParameter;
using UniRx;
using UnityEngine.UI;

public class SystemView : UdonBehaviour{

    [SerializeField]
    private Text TimeText;

    public Action OnTimerStartedListener;

    public Action OnTimerEndedListener;

    public void OnTimerChanged(int time)
    {
        var minute = (time / 60).ToString();
        var second = time > 10 ? (time%60).ToString() : "0" + (time % 60).ToString();
        TimeText.text = minute + ":" + second;
    }

    public void OnTimerStarted()
    {
        if (OnTimerStartedListener == null)
        {
            return;
        }

        OnTimerStartedListener();
    }

    public void OnTimerEnded()
    {
        if (OnTimerEndedListener == null)
        {
            return;
        }

        OnTimerEndedListener();
    }

}
