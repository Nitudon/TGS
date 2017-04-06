using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UdonCommons;

public class CharacterModel : UdonBehaviour {

    public CharacterModel()
    {
        _score = new ReactiveProperty<int>(0);
        _tresures = new ReactiveCollection<TresureModel>();
    }

    private ReactiveProperty<int> _score;
    public IReadOnlyReactiveProperty<int> Score
    {
        get
        {
            if(_score == null)
            {
                InstantLog.StringLogError("score property is null");
                _score = new ReactiveProperty<int>(0);
            }
            return _score;
        }
    }

    private ReactiveCollection<TresureModel> _tresures;
    public IReadOnlyReactiveCollection<TresureModel> Tresures
    {
        get
        {
            if(_tresures == null)
            {
                InstantLog.StringLogError("score property is null");
                _tresures = new ReactiveCollection<TresureModel>();
            }

            return _tresures;
        }
    }

    public void SetScore(int score)
    {
        _score.Value = score;
    }

    public void AddScore(int score)
    {
        _score.Value += score;
    }
}
