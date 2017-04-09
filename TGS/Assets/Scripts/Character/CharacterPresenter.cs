using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CharacterPresenter : MonoBehaviour {

    [SerializeField]
    private CharacterModel _model;

    [SerializeField]
    private CharacterView _view;

    private void Start()
    {
        SetEvents();
        ObserveCharacter();
    }

    private void ObserveCharacter()
    {
        _model.Score
            .Subscribe(_ => _view.OnScoreChanged())
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
        _view.OnFrontColorChangedListener = OnFrontColorChanged;
        _view.OnBackColorChangedListener = OnBackColorChanged;
    }

    private void OnScoreChanged()
    {

    }

    private void OnFrontColorChanged()
    {

    }

    private void OnBackColorChanged()
    {

    }

}
