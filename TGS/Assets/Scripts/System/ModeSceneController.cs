using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using UdonCommons;
using SystemParameter;
using UdonObservable.InputRx.GamePad;
using DG.Tweening;

public class ModeSceneController
{
    public virtual void ControllConnect()
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

    public virtual void Dispose()
    {
        SystemManager.Instance.SubmitDispose();
        SystemManager.Instance.CancelDispose();
    }

    protected virtual void Submit() { }
    protected virtual void Cancel() { }

}
