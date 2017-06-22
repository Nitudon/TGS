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

public class ModeSceneController : UdonBehaviour
{
    protected static float INPUT_STICK_VALUE = 0.7f;

    private IDisposable OnSubmitButtonDownedObservable;
    private IDisposable OnCancelButtonDownedObservable;
    private IDisposable GamePadStickObservable;

    public void ControllConnect(IDisposable submit, IDisposable cancel, IDisposable stick)
    {
        OnSubmitButtonDownedObservable = submit;
        OnCancelButtonDownedObservable = cancel;
        GamePadStickObservable = stick;
    }

    public virtual void Dispose()
    {
        OnSubmitButtonDownedObservable.Dispose();
        OnCancelButtonDownedObservable.Dispose();
        GamePadStickObservable.Dispose();
    }

    protected virtual void Submit() { }
    protected virtual void Cancel() { }
}
