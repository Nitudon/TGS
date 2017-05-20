using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class CharacterView : MonoBehaviour {

    [SerializeField]
    private Text Score;

    public Action OnScoreChangedListener;

    public Action OnFrontColorChangedListener;

    public void OnScoreChanged(int score)
    {
        Score.text = score.ToString();
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

}
