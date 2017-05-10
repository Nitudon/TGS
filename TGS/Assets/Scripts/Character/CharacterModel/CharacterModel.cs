using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using SystemParameter;
using UdonCommons;
using UdonObservable.InputRx.GamePad;

public class CharacterModel : ColorModel {

    private void Awake()
    {
        _tresureColor = (GameEnum.tresureColor)Player;
        _score = new ReactiveProperty<int>(0);
        _controller = new CharacterModelController(this,CharacterAnimator);
        _tresures = new ReactiveCollection<ColorModel>();
        _frontColor = new ReactiveProperty<ColorModel>();
        _backColor = new ReactiveProperty<ColorModel>();
    }

    [SerializeField]
    private Collider FrontCollider;

    [SerializeField]
    private Animator CharacterAnimator;

    [SerializeField]
    private GamePadObservable.Player Player;

    public GamePadObservable.Player GetPlayerID
    {
        get
        {
            return Player;
        }
    }

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

    private void JudgeTresure()
    {
        int score;
        var tresurelist = _tresures.ToList();

        if (TresureJudgeHelper.JudgeTresures(this, out score))
        {
            AddScore(score);
        }
        else
        {
            _controller.SetAnimTrigger(GameEnum.animTrigger.tresure);
        }
    }

    private void JudgeCharacter(CharacterModel model)
    {
        int score;
        var tresurelist = _tresures.ToList();

        if (TresureJudgeHelper.JudgeCharacter(this, model, out score))
        {
            AddScore(score);
        }
    }

    public void AddColor(ColorModel model)
    {
        if(model is TresureModel)
        {
            var tresure = model as TresureModel;
            if (tresure.HasOwner == false)
            {
                AddTresure(tresure);
            }
        }
        else if(model is CharacterModel)
        {
            var character = model as CharacterModel;
            AddCharacter(character);
        }
    }

    private void AddCharacter(CharacterModel model)
    {
        _tresures.Add(model);
        model.SetListNumber(_tresures.Count - 1);
        JudgeCharacter(model);
    }

    private void AddTresure(TresureModel tresure)
    {
        tresure.GetTresure(this);
        _tresures.Add(tresure);
        #region[Debug]
        string ans = "";
        for (int i=0;i<Tresures.Count;++i)
        {
            ans += Tresures.ElementAt(i).TresureColor.ToString() + " : ";
        }

        Debug.Log(ans);
        #endregion
        tresure.SetListNumber(_tresures.Count - 1);
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

    public void RemoveTresureAll()
    {
        for (int i=_tresures.Count-1; i >= 0; --i) {
            RemoveTresure(i);
        }
    }
    
    public void RemoveTresureRange(int index,int count)
    {
        for (int i=index+count-1; i >= index; --i) {
            RemoveTresure(i);
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
