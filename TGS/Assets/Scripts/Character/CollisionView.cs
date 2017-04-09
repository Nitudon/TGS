using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CollisionView : MonoBehaviour {

    public Action OnTriggerEnteredListener;

    public Action OnTriggerExitListener;

    public void OnTriggerEntered()
    {
        if(OnTriggerEnteredListener == null)
        {
            return;
        }

        OnTriggerEnteredListener();
    }

    public void OnTriggerExited()
    {
        if(OnTriggerEnteredListener == null)
        {
            return;
        }

        OnTriggerExitListener();
    }

}
