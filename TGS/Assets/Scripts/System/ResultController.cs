using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using UdonCommons;
using UdonObservable.InputRx.GamePad;
using DG.Tweening;

public class ResultController
{
    public void ControllConnect()
    {
        SystemManager.Instance.SubmitConnect(
            GamePadObservable.GetButtonDownObservable(GamePadObservable.ButtonCode.START)
                .Where(_ => SystemManager.Instance.IsGame == false)
                .Subscribe(x => Submit())
            );

        SystemManager.Instance.CancelConnect(
           GamePadObservable.GetButtonDownObservable(GamePadObservable.ButtonCode.B)
               .Where(_ => SystemManager.Instance.IsGame == false)
               .Subscribe(x => Cancel())
           );
    }

    public void Dispose()
    {
        SystemManager.Instance.SubmitDispose();
        SystemManager.Instance.CancelDispose();
    }

    private void Submit()
    {
        SystemManager.Instance.BackTitle();
    }

    private void Cancel()
    {

    }

}
