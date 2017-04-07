﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using SystemParameter;
using UdonCommons;

public class CharacterModel : UdonBehaviour {

    public CharacterModel(GameEnum.tresureColor color = GameEnum.tresureColor.red)
    {
        _color = color;
        _score = new ReactiveProperty<int>(0);
        _controller = new CharacterModelController(this);
        _tresures = new ReactiveCollection<TresureModel>();
    }

    private CharacterModelController _controller;

    private GameEnum.tresureColor _color;
    public GameEnum.tresureColor Color
    {
        get
        {
            return _color;
        }
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
