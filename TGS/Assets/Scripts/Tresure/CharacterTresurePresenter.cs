﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UdonCommons;
using SystemParameter;
using UniRx;

public class CharacterTresurePresenter : UdonBehaviour
{
    [SerializeField]
    private CharacterModel _model;

    [SerializeField]
    private CharacterTresureView _view;

    private void Start()
    {
        SetEvents();
        ObserveCharacterTresure();
    }

    private void ObserveCharacterTresure()
    {
        _model.Tresures
            .ObserveAdd()
            .Subscribe(_ => _view.OnTresuresAdded())
            .AddTo(gameObject);

        _model.Tresures
            .ObserveRemove()
            .Subscribe(_ => _view.OnTresuresRemoved())
            .AddTo(gameObject);

        _model.Tresures
            .ObserveReplace()
            .Subscribe(_ => _view.OnTresuresReplaced())
            .AddTo(gameObject);
    }

    private void SetEvents()
    {
        _view.OnTresuresAddedListener = OnTresuresAdded;
        _view.OnTresuresRemovedListener = OnTresuresRemoved;
    }

    private void OnTresuresAdded()
    {
        var addTresure = _model.Tresures.Last();
        addTresure.transform.SetParent(transform);
        addTresure.SetLocalPosition(0,0,GameValue.OWN_TRESURE_POSITION_OFFSET*_model.Tresures.Count-1);
    }

    private void OnTresuresRemoved()
    {

    }

}
