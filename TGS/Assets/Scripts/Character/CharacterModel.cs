using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using SystemParameter;
using UdonCommons;

public class CharacterModel : ColorModel {

    private void Awake()
    {
        _score = new ReactiveProperty<int>(0);
        _controller = new CharacterModelController(this);
        _tresures = new ReactiveCollection<TresureModel>();
        _frontColor = new ReactiveProperty<ColorModel>();
        _backColor = new ReactiveProperty<ColorModel>();
    }

    [SerializeField]
    private Collider FrontCollider;

    [SerializeField]
    private Collider BackCollider;

    private CharacterModelController _controller;

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

    private ReactiveProperty<ColorModel>_frontColor;
    public IReadOnlyReactiveProperty<ColorModel> FrontColor
    {
        get
        {
            if (_frontColor == null)
            {
                InstantLog.StringLogError("frontColor property is null");
                _frontColor = new ReactiveProperty<ColorModel>();
            }

            return _frontColor;
        }
    }

    private ReactiveProperty<ColorModel> _backColor;
    public IReadOnlyReactiveProperty<ColorModel> BackColor
    {
        get
        {
            if (_backColor == null)
            {
                InstantLog.StringLogError("backColor property is null");
                _backColor = new ReactiveProperty<ColorModel>();
            }

            return _backColor;
        }
    }

    public void AddTresure(TresureModel tresure)
    {
        _tresures.Add(tresure);
    }

    public void RemoveTresure(int index)
    {
        _tresures.RemoveAt(index);
    }

    public void RemoveRangeTresure(int start,int end)
    {
        
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
