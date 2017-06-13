using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using SystemParameter;
using UdonCommons;
using UdonObservable.InputRx.GamePad;
using DG.Tweening;

public class ResultController : ModeSceneController
{
    [SerializeField]
    private List<GameObject> BattleResultUIPrefabs;

    [SerializeField]
    private GameObject TeamResultUI;

    [SerializeField]
    private Image TeamRankImage;

    [SerializeField]
    private Text TeamScore;

    [SerializeField]
    private List<Sprite> TeamRankSprites;

    private GameObject ResultUIObject;

    private ReactiveProperty<bool> _viewMenu;
    private ReactiveProperty<bool> _retryGame;

    private IDisposable FirstStartButtonObservable;

    private bool _isConnected = false;
    private bool _isAnimation = false;
    private int _teamScore;
    private Tweener _scoreTweener;

    public IReadOnlyReactiveProperty<bool> ViewMenu
    {
        get
        {
            return _viewMenu;
        }
    }
    public IReadOnlyReactiveProperty<bool> RetryGame
    {
        get
        {
            return _retryGame;
        }
    } 

    public void Init()
    {
        _isConnected = true;

        _viewMenu = new ReactiveProperty<bool>(false);
        _retryGame = new ReactiveProperty<bool>(true);

        ControllConnect(
            GamePadObservable.GetButtonDownObservable(GamePadObservable.ButtonCode.A)
                .Where(_ => SystemManager.Instance.IsGame == false && SystemManager.Instance.CreateGame == false && _isAnimation == false)
                .Subscribe(x => Submit())
            ,
            GamePadObservable.GetButtonDownObservable(GamePadObservable.ButtonCode.B)
              .Where(_ => SystemManager.Instance.IsGame == false && SystemManager.Instance.CreateGame == false && _isAnimation == false)
              .Subscribe(x => Cancel())
            ,
             GamePadObservable.GetAxisVerticalObservable()
              .Where(x => SystemManager.Instance.IsGame == false && _viewMenu.Value &&  Mathf.Abs(x) > 0.7f && SystemManager.Instance.CreateGame == false)
              .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
              .Subscribe(x => ResultModeSelect(x))
       );

        FirstStartButtonObservable = GamePadObservable.GetButtonDownObservable(GamePadObservable.ButtonCode.START)
                .Where(_ => SystemManager.Instance.IsGame == false && SystemManager.Instance.CreateGame == false)
                .Subscribe(x => ViewPanel());
    }

    public override void Dispose()
    {
        if (_isConnected)
        {
            FirstStartButtonObservable.Dispose();
            _viewMenu.Dispose();
            _retryGame.Dispose();
            if(ResultUIObject != null)
            {
                Destroy(ResultUIObject);
            }
            base.Dispose();
            _isConnected = false;
        }
    }

    private void ViewPanel()
    {
        if (_isAnimation)
        {
            AnimationSkip();
        }
        else if (_viewMenu.Value == false)
        {
            _viewMenu.Value = true;
            AudioManager.Instance.PlaySystemSE(GameEnum.SE.slide, 2.2f);
        }
    }

    protected override void Submit()
    {
        if (_viewMenu.Value)
        {
            if (_retryGame.Value)
            {
                SystemManager.Instance.GameStart();
            }
            else
            {
                SystemManager.Instance.BackTitle();
            }

            _viewMenu.Value = false;
            AudioManager.Instance.PlaySystemSE(GameEnum.SE.decide);
        }
    }

    protected override void Cancel()
    {
        if (_viewMenu.Value)
        {
            _viewMenu.Value = false;
            AudioManager.Instance.PlaySystemSE(GameEnum.SE.cancel,2.2f);
        }
    }

    private void ResultModeSelect(float vert)
    {
        if (Mathf.Abs(vert) > 0.7f)
        {
            _retryGame.Value = !_retryGame.Value;
            AudioManager.Instance.PlaySystemSE(GameEnum.SE.cursor);
        }
    }

    public void SetRank(List<Sprite> sprites, List<int> ranking, int score)
    {
        StartCoroutine(RankCoroutine(sprites,ranking,score));
    }

    private IEnumerator RankCoroutine(List<Sprite> sprites, List<int> ranking, int score)
    {
        _teamScore = score;
        TeamResultUI.SetActive(SystemManager.Instance.GameType == GameEnum.gameType.team);
        if (TeamResultUI.activeSelf)
        {
            TeamRankImage.color = new Color(255,255,255,0);
            TeamRankImage.transform.localScale *= 5;
            TeamRankImage.sprite = GetTeamRankSprite(score);
            _isAnimation = true;
            var _score = 0;
            _scoreTweener = DOTween.To(
                () => _score,
                num => { _score = num; TeamScore.text = _score.ToString(); },
                score,
                (score>25000) ? 5 : score / 5000 
            ).OnComplete(() => AnimationRankSequence().Play());
        }
        else
        {
            var prefab = BattleResultUIPrefabs.ElementAt(GameValue.MAX_PLAYER_NUM - SystemManager.Instance.PlayerNum);

            ResultUIObject = Instantiate(prefab, transform, false);
            ResultUIObject.transform.SetAsFirstSibling();

            var RankImages = ResultUIObject.GetComponentsInChildren<Image>();

            GameEnum.resultAnimPose pose;
            for (int i = 0; i < SystemManager.Instance.PlayerNum; ++i)
            {
                pose = ranking.ElementAt(i) == 1 ? GameEnum.resultAnimPose.win : GameEnum.resultAnimPose.lose;
                CharacterManager.Instance.GetCharacterModel(i).SetResultPose(pose);
                RankImages.ElementAt(i).sprite = sprites.ElementAt(ranking.ElementAt(i) - 1);
            }
        }
        yield break;
    }

    private Sequence AnimationRankSequence()
    {
        return DOTween.Sequence()
            .Append(TeamRankImage.DOFade(1.0f, 2.0f).SetEase(Ease.OutCubic))
            .Join(TeamRankImage.transform.DOScale(1.0f, 2.0f)).SetEase(Ease.OutCubic)
            .OnComplete(() => _isAnimation = false);
    }

    private void AnimationSkip()
    {
        _scoreTweener.Kill();
        TeamScore.text = _teamScore.ToString();
        TeamRankImage.transform.localScale = new Vector3(1,1,1);
        TeamRankImage.color = new Color(255, 255, 255, 1);
        _isAnimation = false;
    }

    private Sprite GetTeamRankSprite(int score)
    {
        if(score >= GameValue.S3_SCORE)
        {
            return TeamRankSprites.ElementAt(0);
        }
        else if (score >= GameValue.S_SCORE)
        {
            return TeamRankSprites.ElementAt(1);
        }
        else if (score >= GameValue.A_SCORE)
        {
            return TeamRankSprites.ElementAt(2);
        }
        else if(score >= GameValue.B_SCORE)
        {
            return TeamRankSprites.ElementAt(3);
        }
        else
        {
            return TeamRankSprites.ElementAt(4);
        }
    }
}
