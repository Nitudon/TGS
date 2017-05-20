using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UdonCommons;
using SystemParameter;
using UdonObservable.InputRx.Key;
using UdonObservable.InputRx.GamePad;

public class CharacterModelController{

    private CharacterModel _characterModel;

    private Animator _animator;

    private IDisposable ControllConnecter;

    private float speedScale;

    public CharacterModelController(CharacterModel model,Animator animator)
    {
        _characterModel = model;
        _animator = animator;
        speedScale = GameValue.SPEED_BASE_SCALE;

        ControllConnecter = ControllConnect();

    }

    public IDisposable ControllConnect()
    {
        return
        GamePadObservable.GetAxisStickObservable(_characterModel.GetPlayerID)
            .Where(_ => SystemManager.Instance.IsGame)
            .Subscribe(x => CharacterMove(x))
            .AddTo(_characterModel);
    }

    public void ControllDisconnect()
    {
        if(ControllConnecter == null)
        {
            return;
        }

        ControllConnecter.Dispose();
    }

    public void SetAnimTrigger(GameEnum.animTrigger trigger)
    {
        _animator.SetTrigger(trigger.ToString());
    }

    public void SetSpeedScale(float scale)
    {
        speedScale = scale;
    }

    private void CharacterMove(GamepadStickInput.StickInfo info)
    {
        var tresureHeavy = Mathf.Pow(GameValue.SPEED_RATE, _characterModel.Tresures.Count);
        _characterModel.rigitbody.velocity = info.movePosition * GameValue.SPEED_BASE * tresureHeavy;
        info.RotatePosition(_characterModel.transform);
    }

    public void StopMove()
    {
        _characterModel.rigitbody.velocity = new Vector3();
    }
}
