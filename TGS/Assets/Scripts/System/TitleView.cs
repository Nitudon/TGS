using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using SystemParameter;
using DG.Tweening;

public class TitleView : MonoBehaviour {

    private static readonly float HIDE_TINT_X = -1500f;
    private static readonly float VIEW_TINT_X = -580f;
    private static readonly float PANEL_SLIDE_TIME = 0.5f;
    private static readonly Vector3[] POSITIONS = {
        new Vector3(16.1f, 25.68f, -0.011f),
        new Vector3(9.7f, 16.1f, -0.011f),
        new Vector3(1f, 4.31f, -0.011f)
    };

    [SerializeField]
    private GameObject StartSubscription;

    [SerializeField]
    private GameObject ViewMenu;

    [SerializeField]
    private GameObject BattleMenu;

    [SerializeField]
    private Transform MenuArrow;

    [SerializeField]
    private Transform MenuPanel;

    [SerializeField]
    private Text PlayerNum;

    [SerializeField]
    private Text StageNum;

    public Action OnPlayerNumChangedListener;
    public Action OnStageIndexChangedListener;
    public Action OnArrowPosChangedListener;
    public Action OnModeChangedListener;

    public void OnPlayerNumChanged(int num)
    {
        if(OnPlayerNumChangedListener != null)
        {
            OnPlayerNumChangedListener();
        }

        PlayerNum.text = num.ToString();

    }

    public void OnStageIndexChanged(int index)
    {
        if (OnStageIndexChangedListener != null)
        {
            OnStageIndexChangedListener();
        }

        StageNum.text = index.ToString();
    }

    public void OnArrowPosChanged(int pos)
    {
        if (OnArrowPosChangedListener != null)
        {
            OnArrowPosChangedListener();
        }

        MenuArrow.localPosition = POSITIONS[pos%3];
    }

    public void OnModeChanged(TitleController.panelMode mode)
    {
        if (OnModeChangedListener != null)
        {
            OnModeChangedListener();
        }

        switch (mode)
        {
            case TitleController.panelMode.hidden:
                StartSubscription.SetActive(true);
                MenuPanel.DOKill();
                MenuPanel.DOLocalMoveX(HIDE_TINT_X,PANEL_SLIDE_TIME);
                break;

            case TitleController.panelMode.view:
                if (BattleMenu.activeSelf == true)
                {
                    ViewMenu.SetActive(true);
                    BattleMenu.SetActive(false);
                }
                else
                {
                    StartSubscription.SetActive(false);
                    MenuPanel.DOKill();
                    MenuPanel.DOLocalMoveX(VIEW_TINT_X, PANEL_SLIDE_TIME);
                }
                break;

            case TitleController.panelMode.battle:
                ViewMenu.SetActive(false);
                BattleMenu.SetActive(true);
                break;

            case TitleController.panelMode.play:
                StartSubscription.SetActive(true);
                MenuPanel.DOKill();
                MenuPanel.DOLocalMoveX(HIDE_TINT_X, PANEL_SLIDE_TIME);
                MenuArrow.localPosition = POSITIONS[0];
                break;
        }
    }
}
