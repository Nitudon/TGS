using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using SystemParameter;
using UdonCommons;
using UdonObservable.InputRx.GamePad;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CharacterModel : ColorModel {

    protected override void Awake()
    {
        _controller = new CharacterModelController(this,CharacterAnimator);
        CharacterManager.Instance.AddCharacterModel(this);
        if (isGameModel)
        {
            _tresures = new ReactiveCollection<ColorModel>();
            AudioManager.Instance.SetPlayerSource(Player, SEPlayer);
            _tresureColor = (GameEnum.tresureColor)Player;
            _score = new ReactiveProperty<int>(0);
        }
    }

    [SerializeField]
    private bool isGameModel = true;

    [SerializeField]
    private Collider FrontCollider;

    [SerializeField]
    private Animator CharacterAnimator;

    [SerializeField]
    private AnimationScoreText ScoreSuscription;

    [SerializeField]
    private GamePadObservable.Player Player;

    [SerializeField]
    private AudioSource SEPlayer;

    public bool IsGameModel
    {
        get
        {
            return isGameModel;
        }
    }

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

    private void JudgeTresure()
    {
        int score;

        if (TresureJudgeHelper.JudgeTresures(this, out score))
        {
            AddScore(score);
        }
        else
        {
            AudioManager.Instance.PlayPlayerSE(Player, GameEnum.SE.get);
            _controller.SetAnimTrigger(GameEnum.animTrigger.tresure);
        }
    }

    private void JudgeCharacter(CharacterModel model)
    {
        int score;

        if (TresureJudgeHelper.JudgeCharacter(this, model, out score))
        {
            AddScore(score);
        }
    }

    public void AddColor(ColorModel model)
    {
        if (SystemManager.Instance.IsGame)
        {
            if (model is TresureModel)
            {
                var tresure = model as TresureModel;
                if (tresure.HasOwner == false)
                {
                    AddTresure(tresure);
                }
            }
            else if (model is CharacterModel)
            {
                var character = model as CharacterModel;
                AddCharacter(character);
            }
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
            var tresure = _tresures.ElementAt(index) as TresureModel;
            AudioManager.Instance.PlayPlayerSE(Player,GameEnum.SE.crash);
            tresure.BreakTresure();
            tresure.Destroy();
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
        ScoreSuscription.gameObject.SetActive(true);
        ScoreSuscription.SetPosition(RectTransformUtility.WorldToScreenPoint(Camera.main, position) + new Vector2(0f,45f));
        ScoreSuscription.Play(score);
    }

    public void SetResultPose(GameEnum.resultAnimPose pose)
    {
        if(_controller == null)
        {
            InstantLog.StringLogError("animator is missing");
            return;
        }

        _controller.SetResultPose(pose);
    }

    public void StopMove()
    {
        if (_controller == null)
        {
            InstantLog.StringLogError("animator is missing");
            return;
        }

        _controller.StopMove();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CharacterModel))]
[CanEditMultipleObjects]
public class CharacterEditor : Editor
{
    private bool _isGameModel = true;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var isGameModel = serializedObject.FindProperty("isGameModel");
        var characterAnimator = serializedObject.FindProperty("CharacterAnimator");

        _isGameModel = isGameModel.boolValue;


        if (_isGameModel)
        {
            base.OnInspectorGUI();
        }
        else
        {
            isGameModel.boolValue = EditorGUILayout.Toggle("isGameModel", isGameModel.boolValue);
            characterAnimator.objectReferenceValue = EditorGUILayout.ObjectField("CharacterAnimator", characterAnimator.objectReferenceValue, typeof(Animator), true) as Animator;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif