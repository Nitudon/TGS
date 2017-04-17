using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CharacterTresureView : MonoBehaviour
{
    public Action OnTresuresAddedListener;

    public Action OnTresuresRemovedListener;

    public void OnTresuresAdded()
    {
        if (OnTresuresAddedListener == null)
        {
            return;
        }
        else
        {
            OnTresuresAddedListener();
        }
    }

    public void OnTresuresRemoved()
    {
        if (OnTresuresRemovedListener == null)
        {
            return;
        }
        else
        {
            OnTresuresRemovedListener();
        }
    }
}