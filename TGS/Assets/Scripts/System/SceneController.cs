using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SystemParameter;
using DG.Tweening;

public class SceneController : MonoBehaviour{

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
    private RectTransform TitleArrow;

    [SerializeField]
    private RectTransform TitleTint;

    private TitleController _titleController;

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
        if (_titleController == null)
        {
            _titleController = new TitleController(TitleArrow,TitleTint);
        }

        _titleController.ControllConnect();
    }

    private Tweener Fadein()
    {
        return
        SceneFadeTint.DOFade(1, 1.4f);
    }

    private Tweener FadeOut()
    {
        return
        SceneFadeTint.DOFade(0, 1.4f);
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
            systemTask();
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
            if(_titleController != null)
            {
                _titleController.Dispose();
            }
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
        GameEndUI.SetActive(true);
        SetAnimationTrigger(SceneTrigger.GameEnd);
    }

    public void ResultSceneTranslate(Action systemTask)
    {
        SceneFade(GameEnum.BGM.end
            ,() => {
            systemTask();
            SystemCanvas.SetActive(false);
            ResultUI.SetActive(true);
            GameEndUI.SetActive(false);
        });

    }
}
