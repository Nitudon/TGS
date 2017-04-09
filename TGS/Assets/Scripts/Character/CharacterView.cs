﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CharacterView : MonoBehaviour {

    public Action OnScoreChangedListener;

    public Action OnFrontColorChangedListener;

    public Action OnBackColorChangedListener;

    public void OnScoreChanged()
    {
        if(OnScoreChangedListener == null)
        {
            return;
        }
        else
        {
            OnScoreChangedListener();
        }
    }

    public void OnFrontColorChanged()
    {
        if (OnFrontColorChangedListener == null)
        {
            return;
        }
        else
        {
            OnFrontColorChangedListener();
        }
    }

    public void OnBackColorChanged()
    {
        if (OnBackColorChangedListener == null)
        {
            return;
        }
        else
        {
            OnBackColorChangedListener();
        }
    }
}
