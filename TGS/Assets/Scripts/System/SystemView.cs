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
    private FilledSprite TimeBar;

    public Action OnTimerStartedListener;

    public Action OnTimerEndedListener;

    public void OnTimerChanged(int time)
    {
        TimeBar.SetFill(((float)time/(float)GameValue.BATTLE_TIME));
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
