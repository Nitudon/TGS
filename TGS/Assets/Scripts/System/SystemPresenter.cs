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
        _model =_system.Model;
        _system.SetTimer();
        SetEvents();
        ObserveSystem();
    }

    private void ObserveSystem()
    {
        _model.Timer
            .First()
            .Publish()
            .Subscribe(_ => _view.OnTimerStarted())
            .AddTo(gameObject);

        _model.Timer
            .Where(x => x >= 0)
            .Publish()
            .Subscribe(_ => _view.OnTimerChanged(_model.Timer.Value))
            .AddTo(gameObject);

        _model.Timer
            .Where(x => x == 0)
            .First()
            .Publish()
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

    }
}
