using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using UdonCommons;
using UdonObservable.InputRx.GamePad;
using DG.Tweening;

public class TitleController{

    private enum titleCommand { game,explain }

    private RectTransform _arrowTransform;
    private RectTransform _tintTransform;

    private static readonly float HIDE_TINT_X = -1500f;
    private static readonly float VIEW_TINT_X = -630f;
    private static readonly Vector3 UP_POSITION = new Vector3(8.6f, 17.31f,0f);
    private static readonly Vector3 DOWN_POSITION = new Vector3(-0.95f, 8.06f, 0f);

    private titleCommand _mode;
    private bool _viewMenu;

    private IDisposable ControllConnecter;

    public TitleController(RectTransform arrow,RectTransform tint)
    {
        _arrowTransform = arrow;
        _tintTransform = tint;
        _mode = titleCommand.game;
        _viewMenu = false;
    }

    public void ControllConnect()
    {
        ControllConnecter =
            GamePadObservable.GetAxisVerticalObservable()
                .Where(_ => SystemManager.Instance.IsGame == false && _viewMenu)
                .Subscribe(x => TitleModeSelect(x));

        SystemManager.Instance.SubmitConnect(
            GamePadObservable.GetButtonDownObservable(GamePadObservable.ButtonCode.START)
                .Where(_ => SystemManager.Instance.IsGame == false)
                .Subscribe(x => Submit())
            );

        SystemManager.Instance.CancelConnect(
           GamePadObservable.GetButtonDownObservable(GamePadObservable.ButtonCode.B)
               .Where(_ => SystemManager.Instance.IsGame == false)
               .Subscribe(x => Cancel())
           );
    }

    public void Dispose()
    {
        ControllConnecter.Dispose();
        SystemManager.Instance.SubmitDispose();
        SystemManager.Instance.CancelDispose();
    }

    private void Submit()
    {
        if(_viewMenu == false)
        {
            _tintTransform.DOKill();
            _tintTransform.DOLocalMoveX(VIEW_TINT_X,0.4f);
            _viewMenu = true;
        }
        else
        {
            if (_mode == titleCommand.game)
            {
                SystemManager.Instance.GameStart();
            }
        }
    }

    private void Cancel()
    {
        if(_viewMenu == true)
        {
            _tintTransform.DOKill();
            _tintTransform.DOLocalMoveX(HIDE_TINT_X, 0.4f);
            _viewMenu = false;
        }
    }

    private void TitleModeSelect(float vert)
    {
        if(vert > 0.7f && _mode == titleCommand.game)
        {
            _arrowTransform.localPosition = DOWN_POSITION;
            _mode = titleCommand.explain;
        }

        else if (vert < -0.7f && _mode == titleCommand.explain)
        {
            _arrowTransform.localPosition = UP_POSITION;
            _mode = titleCommand.game;
        }
    }

}
