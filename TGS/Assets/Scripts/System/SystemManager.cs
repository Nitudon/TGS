using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UdonCommons;
using SystemParameter;
using UdonObservable.Commons;

public class SystemManager : UdonBehaviourSingleton<SystemManager> {

    private void Awake()
    {
        _timer = ReactiveTimer.ReactiveTimerForSeconds(GameValue.BATTLE_TIME);
    }

    private ReactiveProperty<int> _timer;
    public IReadOnlyReactiveProperty<int> Timer
    {
        get
        {
            if(_timer == null)
            {
                InstantLog.StringLogError("timer is null");
                _timer = new ReactiveProperty<int>(GameValue.BATTLE_TIME);
            }

            return _timer;
        }
    }

}
