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

    public void Init()
    {
        _system.SetTimer();
        SetEvents();
        ObserveSystem();
    }

    private void ObserveSystem()
    {
        _system.Timer
            .First()
            .Publish()
            .Subscribe(_ => _view.OnTimerStarted())
            .AddTo(gameObject);

        _system.Timer
            .Where(x => x >= 0)
            .Publish()
            .Subscribe(_ => _view.OnTimerChanged())
            .AddTo(gameObject);

        _system.Timer
            .Where(x => x == 0)
            .Publish()
            .Subscribe(_ => _view.OnTimerEnded())
            .AddTo(gameObject);
    }

    private void SetEvents()
    {
        _view.OnTimerStartedListener = OnTimerStarted;
        _view.OnTimerChangedListener = OnTimerChanged;
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
