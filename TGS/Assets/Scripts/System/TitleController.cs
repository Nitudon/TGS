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

public class TitleController : ModeSceneController{

    private static float INPUT_STICK_VALUE = 0.7f;

    public enum panelMode { hidden, view, battle, play}

    private bool _isConnected = false;

    private IDisposable FirstStartButtonObservable;

    private ReactiveProperty<panelMode> _mode;
    private ReactiveProperty<int> _playerNum;
    private ReactiveProperty<int> _stageIndex;
    private ReactiveProperty<int> _arrowPos;

    public IReadOnlyReactiveProperty<panelMode> Mode
    {
        get
        {
            return _mode;
        }
    }
    public IReadOnlyReactiveProperty<int> PlayerNum
    {
        get
        {
            return _playerNum;
        }
    }
    public IReadOnlyReactiveProperty<int> StageIndex
    {
        get
        {
            return _stageIndex;
        }
    }
    public IReadOnlyReactiveProperty<int> ArrowPos
    {
        get
        {
            return _arrowPos;
        }
    }

    public void Init()
    {
        _isConnected = true;

        if (_playerNum == null)
        {
            _playerNum = new ReactiveProperty<int>(4);
        }
        if (_stageIndex == null)
        {
            _stageIndex = new ReactiveProperty<int>(1);
        }
        _mode = new ReactiveProperty<panelMode>(panelMode.hidden);
        _arrowPos = new ReactiveProperty<int>(0);

        ControllConnect(
             GamePadObservable.GetButtonDownObservable(GamePadObservable.ButtonCode.A)
                 .Where(_ => SystemManager.Instance.IsGame == false && SystemManager.Instance.CreateGame == false)
                 .Subscribe(x => Submit())
             ,
             GamePadObservable.GetButtonDownObservable(GamePadObservable.ButtonCode.B)
               .Where(_ => SystemManager.Instance.IsGame == false && SystemManager.Instance.CreateGame == false)
               .Subscribe(x =>  Cancel())
             ,
              GamePadObservable.GetAxisStickObservable()
               .Where(x => SystemManager.Instance.IsGame == false && _mode.Value != TitleController.panelMode.hidden && (Mathf.Abs(x.hori) > 0.7f || Mathf.Abs(x.vert) > 0.7f) && SystemManager.Instance.CreateGame == false)
               .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
               .Subscribe(x => TitleModeSelect(x))
        );

        FirstStartButtonObservable = GamePadObservable.GetButtonDownObservable(GamePadObservable.ButtonCode.START)
                 .Where(_ => SystemManager.Instance.IsGame == false && SystemManager.Instance.CreateGame == false)
                 .Subscribe(x => ViewPanel());
    }

    public override void Dispose()
    {
        if (_isConnected)
        {
            base.Dispose();
            _isConnected = false;
        }
    }

    private void ViewPanel()
    {
        if (_mode.Value == panelMode.hidden)
        {
            _mode.Value = panelMode.view;
            AudioManager.Instance.PlaySystemSE(GameEnum.SE.slide, 2.2f);
        }
    }

    protected override void Submit()
    {
        if (_mode.Value == panelMode.view)
        {
            if (_arrowPos.Value == 0)
            {
                _mode.Value = panelMode.battle;
                SystemManager.Instance.SetGameType(GameEnum.gameType.team);
                AudioManager.Instance.PlaySystemSE(GameEnum.SE.submit);
            }
            else if (_arrowPos.Value == 1)
            {
                _mode.Value = panelMode.battle;
                SystemManager.Instance.SetGameType(GameEnum.gameType.battle);
                AudioManager.Instance.PlaySystemSE(GameEnum.SE.submit);
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
        else if(_mode.Value == panelMode.battle)
        {
            if(_arrowPos.Value == 2)
            {
                _mode.Value = panelMode.play;
                SystemManager.Instance.SetPlayerNum(_playerNum.Value);
                SystemManager.Instance.SetStageNum(_stageIndex.Value);
                SystemManager.Instance.GameStart();
                AudioManager.Instance.PlaySystemSE(GameEnum.SE.decide);
            }
        }
    }


    protected override void Cancel()
    {
        if (_mode.Value == panelMode.view)
        {
            _mode.Value = panelMode.hidden;
            AudioManager.Instance.PlaySystemSE(GameEnum.SE.cancel);
        }
        else if(_mode.Value == panelMode.battle)
        {
            _mode.Value = panelMode.view;
            AudioManager.Instance.PlaySystemSE(GameEnum.SE.cancel);
            
        }
    }

    private void TitleModeSelect(GamepadStickInput.StickInfo info)
    {
        var vert = info.vert;
        var hori = info.hori;

        if(vert > INPUT_STICK_VALUE)
        {
            if(_arrowPos.Value < 2)
            {
                _arrowPos.Value++;
            }
        }
        else if(vert < -INPUT_STICK_VALUE)
        {
            if (_arrowPos.Value > 0)
            {
                _arrowPos.Value--;
            }
        }

        if (_mode.Value == panelMode.battle)
        {
            if (hori > INPUT_STICK_VALUE)
            {
                if (_arrowPos.Value == 0 && _playerNum.Value < GameValue.MAX_PLAYER_NUM)
                {
                    _playerNum.Value++;
                }
                else if (_arrowPos.Value == 1 && _stageIndex.Value < GameValue.STAGE_NUM)
                {
                    _stageIndex.Value++;
                }
            }else if(hori < -INPUT_STICK_VALUE)
            {
                if (_arrowPos.Value == 0 && _playerNum.Value > GameValue.MIN_PLAYER_NUM)
                {
                    _playerNum.Value--;
                }
                else if (_arrowPos.Value == 1 && _stageIndex.Value > 1)
                {
                    _stageIndex.Value--;
                }
            }
        }
    }

}
