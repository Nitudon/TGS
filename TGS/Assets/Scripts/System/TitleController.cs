﻿using System;
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

public class TitleController : ModeSceneController{

    private enum titleCommand { game,explain }

    private RectTransform _arrowTransform;
    private RectTransform _tintTransform;
    private GameObject _startsubscription;

    private static readonly float HIDE_TINT_X = -1500f;
    private static readonly float VIEW_TINT_X = -630f;
    private static readonly Vector3 UP_POSITION = new Vector3(8.6f, 17.31f,0f);
    private static readonly Vector3 DOWN_POSITION = new Vector3(-0.95f, 8.06f, 0f);

    private titleCommand _mode;
    private bool _viewMenu;

    private IDisposable ControllConnecter;

    public TitleController(RectTransform arrow,RectTransform tint, GameObject subscription)
    {
        _arrowTransform = arrow;
        _tintTransform = tint;
        _mode = titleCommand.game;
        _viewMenu = false;
        _startsubscription = subscription;
    }

    public override void ControllConnect()
    {
        _startsubscription.SetActive(true);

        ControllConnecter =
            GamePadObservable.GetAxisVerticalObservable()
                .Where(_ => SystemManager.Instance.IsGame == false && _viewMenu)
                .Subscribe(x => TitleModeSelect(x));

        base.ControllConnect();
    }

    public override void Dispose()
    {
        ControllConnecter.Dispose();
        base.Dispose();
    }

    protected override void Submit()
    {
        if (SystemManager.Instance.CreateGame == false)
        {
            if (_viewMenu == false)
            {
                _startsubscription.SetActive(false);
                _tintTransform.DOKill();
                _tintTransform.DOLocalMoveX(VIEW_TINT_X, 0.4f);
                _viewMenu = true;
                AudioManager.Instance.PlaySystemSE(GameEnum.SE.slide,2.2f);
            }
            else
            {
                if (_mode == titleCommand.game)
                {
                    _tintTransform.DOLocalMoveX(HIDE_TINT_X, 0.4f);
                    _viewMenu = false;
                    SystemManager.Instance.GameStart();
                    AudioManager.Instance.PlaySystemSE(GameEnum.SE.decide);
                }
                else
                {
                    AudioManager.Instance.PlaySystemSE(GameEnum.SE.decide);
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
            }
        }
    }

    protected override void Cancel()
    {
        if(_viewMenu == true)
        {
            _tintTransform.DOKill();
            _tintTransform.DOLocalMoveX(HIDE_TINT_X, 0.4f)
                .OnComplete(() => _startsubscription.SetActive(true));
            _viewMenu = false;
            AudioManager.Instance.PlaySystemSE(GameEnum.SE.cancel,2.2f);
        }
    }

    private void TitleModeSelect(float vert)
    {
        if(vert > 0.7f && _mode == titleCommand.game)
        {
            _arrowTransform.localPosition = DOWN_POSITION;
            _mode = titleCommand.explain;
            AudioManager.Instance.PlaySystemSE(GameEnum.SE.cursor);
        }

        else if (vert < -0.7f && _mode == titleCommand.explain)
        {
            _arrowTransform.localPosition = UP_POSITION;
            _mode = titleCommand.game;
            AudioManager.Instance.PlaySystemSE(GameEnum.SE.cursor);
        }
    }

}