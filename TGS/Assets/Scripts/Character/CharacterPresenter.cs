using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CharacterPresenter : MonoBehaviour {

    private CharacterModel _model;

    private CharacterView _view;

    private void ObserveCharacter()
    {
        _model.Score
            .Subscribe(_ => _view.OnScoreChanged())
            .AddTo(gameObject);

        _model.Tresures
            .ObserveAdd()
            .Subscribe(_ => _view.OnTresuresAdded())
            .AddTo(gameObject);

        _model.Tresures
            .ObserveRemove()
            .Subscribe(_ => _view.OnTresuresRemoved())
            .AddTo(gameObject);

        _model.FrontColor
            .Subscribe(_ => _view.OnFrontColorChanged())
            .AddTo(gameObject);

        _model.BackColor
            .Subscribe(_ => _view.OnBackColorChanged())
            .AddTo(gameObject);

    }

    private void SetEvents()
    {
        _view.OnScoreChangedListener = OnScoreChanged;
        _view.OnTresuresAddedListener = OnTresuresAdded;
        _view.OnTresuresRemovedListener = OnTresuresRemoved;
        _view.OnFrontColorChangedListener = OnFrontColorChanged;
        _view.OnBackColorChangedListener = OnBackColorChanged;
    }

    private void OnScoreChanged()
    {

    }

    private void OnTresuresAdded()
    {

    }

    private void OnTresuresRemoved()
    {

    }

    private void OnFrontColorChanged()
    {

    }

    private void OnBackColorChanged()
    {

    }

}
