using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonCommons;
using UniRx;
using UnityEngine.UI;

public class SystemView : UdonBehaviour{

    [SerializeField]
    private Text TimeText;

    public Action OnTimerStartedListener;

    public Action OnTimerEndedListener;

    public void OnTimerChanged(int time)
    {
        TimeText.text = time.ToString();
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
