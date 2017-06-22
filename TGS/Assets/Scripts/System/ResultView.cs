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

    [SerializeField]
    private Text StageIndex;

    [SerializeField]
    private GameObject CommandUI;

    [SerializeField]
    private GameObject BattleUI;

    public Action OnArrowPosChangedListener;
    public Action OnViewChangedListener;
    public Action OnStageIndexChangedListener;

    public void Init()
    {
        StageIndex.text = SystemManager.Instance.StageNum.ToString();
        MenuArrow.localPosition = UP_POSITION;
    }

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

    public void OnViewChanged(ResultController.panelMode mode)
    {
        if (OnViewChangedListener != null)
        {
            OnViewChangedListener();
        }

        if (mode == ResultController.panelMode.view)
        {
            StartSubscription.SetActive(false);
            CommandUI.SetActive(true);
            BattleUI.SetActive(false);
            MenuTint.SetActive(true);
            MenuPanel.DOScaleY(1, PANEL_SCALE_TIME);
        }
        else if(mode == ResultController.panelMode.battle)
        {
            CommandUI.SetActive(false);
            BattleUI.SetActive(true);
        }
        else
        {
            StartSubscription.SetActive(true);
            MenuPanel.DOScaleY(0, PANEL_SCALE_TIME)
                        .OnComplete(() => { MenuTint.SetActive(false); });
        }
    }

    public void OnStageIndexChanged(int index)
    {
        if (OnStageIndexChangedListener != null)
        {
            OnStageIndexChangedListener();
        }

        StageIndex.text = index.ToString();
    }
}
