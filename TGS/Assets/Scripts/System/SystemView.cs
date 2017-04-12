using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonCommons;
using UniRx;

public class SystemView : UdonBehaviour{

    public Action OnTimerChangedListener;

    public Action OnTimerStartedListener;

    public Action OnTimerEndedListener;

    public void OnTimerChanged()
    {
        if (OnTimerChangedListener == null)
        {
            return;
        }

        OnTimerChangedListener();
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
