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
    private SystemPresenter Presenter;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameStart();
        }
    }

    private void GameStart()
    {
        var prefab = Instantiate(PlayingGamePrefab,transform);
        Presenter.Init();   
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

}
