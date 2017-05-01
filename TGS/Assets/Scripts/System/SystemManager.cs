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
    private SystemPresenter Presenter;

    [SerializeField]
    private GameObject SystemCanvas;

    [SerializeField]
    private GameObject StartSceneObjects;

    [SerializeField]
    private GameObject EndSceneObjects;

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
    }

    public void GameStart()
    {
        StartSceneObjects.SetActive(false);
        SystemCanvas.SetActive(true);
        _isPause = false;
        _isGame = true;
        CreatePlayingObjects();
        Presenter.Init();   
    }

    public void GameEnd()
    {
        StartSceneObjects.SetActive(true);
        SystemCanvas.SetActive(false);
        _isGame = false;
        _model.Dispose();
        DestroyPlayingObjects();
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
