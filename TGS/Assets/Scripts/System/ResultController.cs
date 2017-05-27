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

public class ResultController : ModeSceneController
{

    private enum ResultCommand { game, title }

    private RectTransform _arrowTransform;
    private GameObject _tint;
    private RectTransform _panelTransform;

    private static readonly Vector3 UP_POSITION = new Vector3(-338.3f,138.6f,0f);
    private static readonly Vector3 DOWN_POSITION = new Vector3(-338.3f,-133.1f,0f);

    private ResultCommand _mode;
    private bool _viewMenu;

    private IDisposable ControllConnecter;

    public ResultController(RectTransform arrow, GameObject tint, RectTransform panel)
    {
        _arrowTransform = arrow;
        _tint = tint;
        _mode = ResultCommand.game;
        _panelTransform = panel;
        _viewMenu = false;
    }

    public override void ControllConnect()
    {
        ControllConnecter =
            GamePadObservable.GetAxisVerticalObservable()
                .Where(_ => SystemManager.Instance.IsGame == false && _viewMenu)
                .Subscribe(x => ResultModeSelect(x));

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
                _tint.SetActive(true);
                _panelTransform.DOScaleY(1,0.3f);
                _viewMenu = true;
                AudioManager.Instance.PlaySystemSE(GameEnum.SE.slide);
            }
            else
            {
                if (_mode == ResultCommand.game)
                {   
                    SystemManager.Instance.GameStart();
                }
                else
                {
                    SystemManager.Instance.BackTitle();
                }
                _panelTransform.DOScaleY(0, 0.3f)
                        .OnComplete(() => _tint.SetActive(false));
                _viewMenu = false;
                AudioManager.Instance.PlaySystemSE(GameEnum.SE.decide);
            }
        }
    }

    protected override void Cancel()
    {
        if (_viewMenu == true)
        {
            _panelTransform.DOScaleY(0, 0.3f)
                         .OnComplete(() => _tint.SetActive(false));
            _viewMenu = false;
        }
    }

    private void ResultModeSelect(float vert)
    {
        if (vert > 0.7f && _mode == ResultCommand.game)
        {
            _arrowTransform.localPosition = DOWN_POSITION;
            _mode = ResultCommand.title;
            AudioManager.Instance.PlaySystemSE(SystemParameter.GameEnum.SE.cursor);
        }

        else if (vert < -0.7f && _mode == ResultCommand.title)
        {
            _arrowTransform.localPosition = UP_POSITION;
            _mode = ResultCommand.game;
            AudioManager.Instance.PlaySystemSE(SystemParameter.GameEnum.SE.cursor);
        }
    }

    public void SetRank(List<Image> images, List<Sprite> sprites, List<int> ranking)
    {
        GameEnum.resultAnimPose pose;
        for (int i = 0; i < ranking.Count(); ++i)
        {
            pose = ranking.ElementAt(i) == 1 ? GameEnum.resultAnimPose.win : GameEnum.resultAnimPose.lose;
            CharacterManager.Instance.GetCharacterModel(i).SetResultPose(pose);
            images.ElementAt(i).sprite = sprites.ElementAt(ranking.ElementAt(i) - 1);
        }
    }
}
