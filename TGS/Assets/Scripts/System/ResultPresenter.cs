﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using SystemParameter;
using DG.Tweening;

public class ResultPresenter : MonoBehaviour {

    [SerializeField]
    private ResultController _model;

    [SerializeField]
    private ResultView _view;

    [SerializeField]
    private List<Sprite> RankImageResources;

    public void Init(List<int> ranking,int score)
    {
        _model.Init();
        _view.Init();
        _model.SetRank(RankImageResources,ranking,score);
        SetEvents();
        ObserveTitleScene();
    }

    public void Dispose()
    {
        _model.Dispose();
    }

    private void ObserveTitleScene()
    {
        _model.ViewMenu
            .Skip(1)
            .Subscribe(x => _view.OnViewChanged(x));

        _model.RetryGame
            .Skip(1)
            .Subscribe(x => _view.OnArrowPosChanged(x));

        _model.RetryStageIndex
            .Skip(1)
            .Subscribe(x => _view.OnStageIndexChanged(x));
    }

    private void SetEvents()
    {
        _view.OnArrowPosChangedListener = OnArrowPosChanged;
        _view.OnViewChangedListener = OnViewChanged;
        _view.OnStageIndexChangedListener = OnSrageIndexChanged;
    }

    public void OnArrowPosChanged()
    {
        AudioManager.Instance.PlaySystemSE(GameEnum.SE.cursor);
    }

    public void OnViewChanged()
    {

    }

    public void OnSrageIndexChanged()
    {
        AudioManager.Instance.PlaySystemSE(GameEnum.SE.cursor);
    }
}
