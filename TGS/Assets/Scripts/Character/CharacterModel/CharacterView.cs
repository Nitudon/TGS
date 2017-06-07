using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class CharacterView : MonoBehaviour {

    private static readonly Vector2 SUBSCRIPTION_POSITION_OFFSET = new Vector3(20f,30f);

    [SerializeField]
    private Text Score;

    [SerializeField]
    private GameObject PlayerSubscription;

    public Action OnScoreChangedListener;
    public Action OnPlayerPositionChangedListener;

    public void OnScoreChanged(int score)
    {
        if(OnScoreChangedListener != null)
        {
            OnScoreChangedListener();
        }

        Score.text = score.ToString();
    }

    public void OnPlayerPositionChanged(Vector3 position)
    {
        if(OnPlayerPositionChangedListener != null)
        {
            OnPlayerPositionChangedListener();

        }

        PlayerSubscription.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main ,position) + SUBSCRIPTION_POSITION_OFFSET;
    }

}
