using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using SystemParameter;
using UdonCommons;
using UdonObservable.InputRx.GamePad;
using DG.Tweening;

public class ResultView : MonoBehaviour {

    private static readonly float PANEL_SCALE_TIME = 0.3f;
    private static readonly Vector3 UP_POSITION = new Vector3(-338.3f, 138.6f, 0f);
    private static readonly Vector3 DOWN_POSITION = new Vector3(-338.3f, -133.1f, 0f);

    [SerializeField]
    private GameObject MenuTint;

    [SerializeField]
    private Transform MenuArrow;

    [SerializeField]
    private Transform MenuPanel;

    [SerializeField]
    private GameObject StartSubscription;

    public Action OnArrowPosChangedListener;
    public Action OnViewChangedListener;

    public void OnArrowPosChanged(bool tryGame)
    {
        if (OnArrowPosChangedListener != null)
        {
            OnArrowPosChangedListener();
        }

        if (tryGame)
        {
            MenuArrow.localPosition = UP_POSITION;
        }
        else
        {
            MenuArrow.localPosition = DOWN_POSITION;
        }
    }

    public void OnViewChanged(bool view)
    {
        if (OnViewChangedListener != null)
        {
            OnViewChangedListener();
        }

        if (view)
        {
            StartSubscription.SetActive(false);
            MenuTint.SetActive(true);
            MenuPanel.DOScaleY(1, PANEL_SCALE_TIME);
        }
        else
        {
            StartSubscription.SetActive(true);
            MenuPanel.DOScaleY(0, PANEL_SCALE_TIME)
                        .OnComplete(() => { MenuTint.SetActive(false); MenuArrow.localPosition = UP_POSITION; });
        }
    }
}
