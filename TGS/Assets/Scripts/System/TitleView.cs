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
    private static readonly float STAGE_DISPLAY_HIDE_X = 200f;
    private static readonly float STAGE_DISPLAY_VIEW_X = 50f;

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

    [SerializeField]
    private GameObject StageDisplay;

    [SerializeField]
    private GameObject[] StageDisplayObjects;

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

        for (int i=0;i < StageDisplayObjects.Length; ++i)
        {
            StageDisplayObjects[i].SetActive(i+1 == index);
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
                    StageDisplay.transform.DOKill();
                    ViewMenu.SetActive(true);
                    BattleMenu.SetActive(false);
                    StageDisplay.transform.DOLocalMoveX(STAGE_DISPLAY_HIDE_X, PANEL_SLIDE_TIME)
                        .OnComplete(() => StageDisplay.SetActive(false));
                }
                else
                {
                    StartSubscription.SetActive(false);
                    MenuPanel.DOKill();
                    MenuPanel.DOLocalMoveX(VIEW_TINT_X, PANEL_SLIDE_TIME);
                }
                break;

            case TitleController.panelMode.battle:
                StageDisplay.transform.DOKill();
                ViewMenu.SetActive(false);
                BattleMenu.SetActive(true);
                StageDisplay.SetActive(true);
                StageDisplay.transform.DOLocalMoveX(STAGE_DISPLAY_VIEW_X,PANEL_SLIDE_TIME);
                break;

            case TitleController.panelMode.play:
                StartSubscription.SetActive(true);
                MenuPanel.DOKill();
                MenuPanel.DOLocalMoveX(HIDE_TINT_X, PANEL_SLIDE_TIME)
                    .OnComplete(() => {
                        MenuArrow.localPosition = POSITIONS[0];
                        ViewMenu.SetActive(true);
                        BattleMenu.SetActive(false);
                    } );
                StageDisplay.transform.DOKill();
                StageDisplay.transform.DOLocalMoveX(STAGE_DISPLAY_HIDE_X, PANEL_SLIDE_TIME)
                       .OnComplete(() => StageDisplay.SetActive(false));
                break;
        }
    }
}
