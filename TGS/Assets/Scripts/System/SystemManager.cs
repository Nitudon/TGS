using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UdonCommons;
using SystemParameter;
using UdonObservable.Commons;

public class SystemManager : UdonBehaviourSingleton<SystemManager> {

    [SerializeField]
    private GameObject PlayingGamePrefab;

    [SerializeField]
    private GameObject StageManagerPrefab;

    [SerializeField]
    private GameObject ParticleManagerPrefab;

    [SerializeField]
    private GameObject ResultScenePrefab;

    [SerializeField]
    private SystemPresenter Presenter;

    [SerializeField]
    private SceneController SystemCanvas;

    private GameObject ParticleManagerObject;

    private GameObject PlayingGameObject;

    private GameObject ResultGameObject;

    private GameObject StageManagerObject;

    private IDisposable SubmitController;

    private IDisposable CancelController;

    private bool _createdGame;
    private bool _finishedGame;
    private bool _isGame;

    public bool IsGame
    {
        get
        {
            return _isGame;
        }
    }

    private bool _isPause;

    public bool IsPause
    {
        get
        {
            return _isPause;
        }
    }

    public bool CreateGame
    {
        get
        {
            return _createdGame;
        }
    }

    protected override void Start()
    {
        AudioManager.Instance.PlayBGM(GameEnum.BGM.title);
    }

    public void GameStart()
    {
        if (_createdGame == false)
        {
            StartCoroutine(OnGameStartCoroutine());
        }
    }

    public void GameEnd()
    {
        StartCoroutine(OnGameEndCoroutine());
    }

    public void BackTitle()
    {
        if (_finishedGame == false)
        {
            StartCoroutine(OnBackTitleCoroutine());
        }
    }

    private IEnumerator OnGameStartCoroutine()
    {
        _createdGame = true;
        _isGame = false;
        _finishedGame = false;
        CharacterManager.Instance.InitCharacterList();
        SystemCanvas.GameSceneTranslate(() => { CreatePlayingObjects();  DestroyResultObjects();});

        yield return new WaitUntil(() => SystemCanvas.isPlayingGame);

        SystemCanvas.BattleStart();
        AudioManager.Instance.PlaySystemSE(GameEnum.SE.start);
        _isPause = false;
        _isGame = true;
        Presenter.Init();

        yield break;
    }

    private IEnumerator OnGameEndCoroutine()
    {
        _isGame = false;
        _createdGame = false;
        AudioManager.Instance.PlaySystemSE(GameEnum.SE.end);
        SystemCanvas.BattleEnd();

        yield return new WaitUntil(() => SystemCanvas.isEndingGame);
        yield return new WaitWhile(() => SystemCanvas.isEndingGame);

        SystemCanvas.ResultSceneTranslate(() => { DestroyPlayingObjects(); CreateResultPlayingObject(); });
        AudioManager.Instance.ResetPlayerSource();

        yield break;
    }

    private IEnumerator OnBackTitleCoroutine()
    {
        _finishedGame = true;
        SystemCanvas.TitleSceneTranslate(() => DestroyResultObjects());
        yield break;
    }

    private void CreatePlayingObjects()
    {
        PlayingGameObject = Instantiate(PlayingGamePrefab, transform);
        StageManagerObject = Instantiate(StageManagerPrefab, transform);
        ParticleManagerObject = Instantiate(ParticleManagerPrefab, transform);
    }

    private void DestroyPlayingObjects()
    {
        Destroy(PlayingGameObject);
        Destroy(StageManagerObject);
        Destroy(ParticleManagerObject);
    }

    private void CreateResultPlayingObject()
    {
        ResultGameObject = Instantiate(ResultScenePrefab, transform);
    }

    private void DestroyResultObjects()
    {
        Destroy(ResultGameObject);
    }

    public void SubmitConnect(IDisposable controller)
    {
        SubmitController = controller;
    }

    public void CancelConnect(IDisposable controller)
    {
        CancelController = controller;
    }

    public void SubmitDispose()
    {
        SubmitController.Dispose();
    }

    public void CancelDispose()
    {
        CancelController.Dispose();
    }

    public class SystemModel
    {
        public SystemModel(int time)
        {
            _timer = ReactiveTimer.ReactiveTimerForSeconds(time);
        }

        private ReactiveProperty<int> _timer;
        public IReadOnlyReactiveProperty<int> Timer
        {
            get
            {
                if (_timer == null)
                {
                    InstantLog.StringLogError("timer is null");
                    _timer = new ReactiveProperty<int>(GameValue.BATTLE_TIME);
                }

                return _timer;
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }

    public SystemModel Model
    {
        get
        {
            if(_model == null)
            {
                _model = new SystemModel(0);
            }

            return _model;
        }
    }

    private SystemModel _model;

    public void SetPause(bool pause)
    {
        _isPause = pause;
    }

}
