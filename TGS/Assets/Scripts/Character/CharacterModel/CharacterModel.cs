using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using SystemParameter;
using UdonCommons;

public class CharacterModel : ColorModel {

    private void Awake()
    {
        _tresureColor = GameEnum.tresureColor.red;
        _score = new ReactiveProperty<int>(0);
        _controller = new CharacterModelController(this);
        _judge = new TresureJudge();
        _tresures = new ReactiveCollection<ColorModel>();
        _frontColor = new ReactiveProperty<ColorModel>();
        _backColor = new ReactiveProperty<ColorModel>();
    }

    [SerializeField]
    private Collider FrontCollider;

    [SerializeField]
    private Collider BackCollider;

    private CharacterModelController _controller;

    private TresureJudge _judge;

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

    private ReactiveCollection<ColorModel> _tresures;
    public IReadOnlyReactiveCollection<ColorModel> Tresures
    {
        get
        {
            if(_tresures == null)
            {
                InstantLog.StringLogError("score property is null");
                _tresures = new ReactiveCollection<ColorModel>();
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

    public void JudgeTresure()
    {
        int score;
        int index;
        int count;
        var tresurelist = _tresures.ToList();

        if (_judge.JudgeTresures(_tresureColor,tresurelist, out score,out index, out count))
        {
            for (int i=index + count;i >= index ;--i)
            {
                RemoveTresure(i);
            }
        }
        
    }

    public void AddTresure(ColorModel tresure)
    {
        if(tresure is TresureModel)
        {
            var _tresure = tresure as TresureModel;
            _tresure.SetOwner(this);
        }

        _tresures.Add(tresure);

        JudgeTresure();
    }

    public void RemoveTresure(int index)
    {
        if(_tresures.ElementAt(index) is CharacterModel == false)
        {
            _tresures.ElementAt(index).Destroy();
        }
        _tresures.RemoveAt(index);
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
