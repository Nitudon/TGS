using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CharacterTresureView : MonoBehaviour
{
    void Awake()
    {
        viewTresureCount = 0;
    }

    private int viewTresureCount;

    public Action OnTresuresAddedListener;

    public Action OnTresuresRemovedListener;

    public Action OnTresuresChangedListener;

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

    public void OnTresuresChanged()
    {
        if (OnTresuresChangedListener == null)
        {
            return;
        }
        else
        {
            OnTresuresChangedListener();
        }
    }
}