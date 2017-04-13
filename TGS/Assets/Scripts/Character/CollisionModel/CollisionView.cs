using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CollisionView : MonoBehaviour {

    public Action<GameObject> OnTriggerEnteredListener;

    public Action<GameObject> OnTriggerExitListener;

    public void OnTriggerEntered(GameObject go)
    {
        if(OnTriggerEnteredListener == null)
        {
            return;
        }

        OnTriggerEnteredListener(go);
    }

    public void OnTriggerExited(GameObject go)
    {
        if(OnTriggerEnteredListener == null)
        {
            return;
        }

        OnTriggerExitListener(go);
    }

}
