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
    private SystemPresenter Presenter;

    [SerializeField]
    private SceneController SystemCanvas;

    [SerializeField]
    private GameObject StartSceneObjects;

    [SerializeField]
    private GameObject EndSceneObjects;

    private GameObject ParticleManagerObject;

    private GameObject PlayingGameObject;

    private GameObject StageManagerObject;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameStart();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            BackTitle();
        }
    }

    public void GameStart()
    {
        StartCoroutine(OnGameStartCoroutine());
    }

    public void GameEnd()
    {
        EndSceneObjects.SetActive(true);
        SystemCanvas.gameObject.SetActive(false);
        _isGame = false;
        DestroyPlayingObjects();
    }

    private IEnumerator OnGameStartCoroutine()
    {
        _isGame = false;
        CreatePlayingObjects();
        StartSceneObjects.SetActive(false);
        AudioManager.Instance.PlayBGM(GameEnum.BGM.battle);
        SystemCanvas.gameObject.SetActive(true);

        yield return new WaitUntil(() => SystemCanvas.isPlayingGame);

        SystemCanvas.GameStart();
        AudioManager.Instance.PlaySystemSE(GameEnum.SE.start);
        _isPause = false;
        _isGame = true;
        Presenter.Init();

        yield break;
    }

    private void BackTitle()
    {
        StartSceneObjects.SetActive(true);
        EndSceneObjects.SetActive(false);
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
