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
            .Subscribe(_ => 
            {
                if (SystemManager.Instance.GameType == SystemParameter.GameEnum.gameType.battle)
                {
                    _view.OnScoreChanged(_model.Score.Value);
                }
                else
                {
                    _view.OnTeamScoreChanged();
                }
            })
            .AddTo(gameObject);

        _model.SubscriotionPosition
            .Subscribe(x => _view.OnPlayerPositionChanged(x))
            .AddTo(gameObject);

    }

    private void SetEvents()
    {

    }

}
