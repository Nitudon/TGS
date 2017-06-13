using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SystemParameter;
using DG.Tweening;

public class SceneController : MonoBehaviour{

    private static readonly float FADE_TIME = 1.4f;
    private static readonly float TITLE_SCENE_CAMERA_DISTANCE = 40f;
    private static readonly float RESULT_SCENE_CAMERA_DISTANCE = 0.02f;
    private static readonly int TITLE_SCENE_CAMERA_SORT = -2;
    private static readonly int RESULT_SCENE_CAMERA_SORT = 1;

    [SerializeField]
    private Canvas Canvas;

    [SerializeField]
    private Animator SceneUIAnimator;

    [SerializeField]
    private GameObject SystemCanvas;

    [SerializeField]
    private GameObject GameStartUI;

    [SerializeField]
    private GameObject GameEndUI;

    [SerializeField]
    private GameObject TitleUI;

    [SerializeField]
    private GameObject ResultUI;

    [SerializeField]
    private CanvasGroup SceneFadeTint;

    [SerializeField]
    private Text TimeText;

    [SerializeField]
    private TitlePresenter TitleController;

    [SerializeField]
    private ResultPresenter ResultController;

    private enum BattleState{StartAnimation,PlayingGame,EndAnimation}
    private enum SceneTrigger { GameStart,GameEnd}

    public bool isStartingGame
    {
        get
        {
            return SceneUIAnimator.GetCurrentAnimatorStateInfo(0).IsName(BattleState.StartAnimation.ToString());
        }
    }

    public bool isPlayingGame
    {
        get
        {
            return SceneUIAnimator.GetCurrentAnimatorStateInfo(0).IsName(BattleState.PlayingGame.ToString());
        }
    }

    public bool isEndingGame
    {
        get
        {
            return SceneUIAnimator.GetCurrentAnimatorStateInfo(0).IsName(BattleState.EndAnimation.ToString());
        }
    }

    private void Awake()
    {
        TitleConnect();
    }

    private void TitleConnect()
    {
        Canvas.planeDistance = TITLE_SCENE_CAMERA_DISTANCE;
        Canvas.sortingOrder = TITLE_SCENE_CAMERA_SORT;
        TitleController.Init();
    }

    private void ResultConnect(List<int> ranking,int score)
    {
        Canvas.planeDistance = RESULT_SCENE_CAMERA_DISTANCE;
        Canvas.sortingOrder = RESULT_SCENE_CAMERA_SORT;
        ResultController.Init(ranking,score);
    }

    private void TimerReset()
    {
        TimeText.text = "2:00";
        TimeText.color = Color.white;
    }

    private Tweener Fadein()
    {
        return
        SceneFadeTint.DOFade(1, FADE_TIME);
    }

    private Tweener FadeOut()
    {
        return
        SceneFadeTint.DOFade(0, FADE_TIME);
    }

    private void SceneFade(GameEnum.BGM bgm, Action inTask = null,Action outTask = null)
    {
        StartCoroutine(FadeCoroutine(bgm,inTask,outTask));
    }

    private IEnumerator FadeCoroutine(GameEnum.BGM bgm,Action inTask = null,Action outTask = null)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(Fadein()
            .OnComplete(() => { if (inTask != null) inTask(); }));
        seq.Append(FadeOut()
            .OnComplete(() => { if (outTask != null) outTask(); AudioManager.Instance.PlayBGM(bgm); }));
        yield return null;
    }

    public void TitleSceneTranslate(Action systemTask)
    {
        SceneFade(GameEnum.BGM.title
            ,() =>
        {
            if (ResultController != null)
            {
                ResultController.Dispose();
            }
            systemTask();
            TitleConnect();
            ResultUI.SetActive(false);
            TitleUI.SetActive(true);
        }
        );
    }

    public void GameSceneTranslate(Action systemTask)
    {
        SceneFade(GameEnum.BGM.battle
            ,() => 
        {
            if(TitleController != null)
            {
                TitleController.Dispose();
            }
            if (ResultController != null)
            {
                ResultController.Dispose();
            }
            TimerReset();
            ResultUI.SetActive(false);
            SystemCanvas.SetActive(true);
            systemTask();
            TitleUI.SetActive(false);
            GameStartUI.SetActive(true);
        },
            () => SetAnimationTrigger(SceneTrigger.GameStart)
        );
    }

    private void SetAnimationTrigger(SceneTrigger trigger)
    {
        SceneUIAnimator.SetTrigger(trigger.ToString());
    }

    public void BattleStart()
    {
        GameStartUI.SetActive(false);
    }

    public void BattleEnd()
    {
        CharacterManager.Instance.AllCharacterStop();
        GameEndUI.SetActive(true);
        SetAnimationTrigger(SceneTrigger.GameEnd);
    }

    public void ResultSceneTranslate(Action systemTask)
    {
        var ranking = CharacterManager.Instance.GetCharacterRankList();
        var teamScore = CharacterManager.Instance.GetCharacterSumScore();

        SceneFade(GameEnum.BGM.end
            ,() => {
                CharacterManager.Instance.InitCharacterList();
                systemTask();
                ResultUI.SetActive(true);
                ResultConnect(ranking,teamScore);
                SystemCanvas.SetActive(false);
                GameEndUI.SetActive(false);
            }
       );

    }
}
