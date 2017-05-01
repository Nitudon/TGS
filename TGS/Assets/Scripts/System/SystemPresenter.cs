using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UdonCommons;
using SystemParameter;

public class SystemPresenter : MonoBehaviour {

    [SerializeField]
    private SystemManager _system;

    [SerializeField]
    private SystemView _view;

    private SystemManager.SystemModel _model;

    public void Init()
    {
        _model = new SystemManager.SystemModel(GameValue.BATTLE_TIME);
        SetEvents();
        ObserveSystem();
    }

    private void ObserveSystem()
    {
        _model.Timer
            .First()
            .Subscribe(_ => _view.OnTimerStarted())
            .AddTo(gameObject);

        _model.Timer
            .Where(x => x >= 0)
            .Subscribe(_ => _view.OnTimerChanged(_model.Timer.Value))
            .AddTo(gameObject);

        _model.Timer
            .Where(x => x == 0)
            .First()
            .Subscribe(_ => _view.OnTimerEnded())
            .AddTo(gameObject);
    }

    private void SetEvents()
    {
        _view.OnTimerStartedListener = OnTimerStarted;
        _view.OnTimerEndedListener = OnTimerEnded;
    }

    private void OnTimerStarted()
    {

    }

    private void OnTimerChanged()
    {

    }

    private void OnTimerEnded()
    {
        SystemManager.Instance.GameEnd();
    }
}
