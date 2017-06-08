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

    private float _speedScale = 1.0f;

    private CharacterModel _characterModel;

    private Animator _animator;

    private IDisposable ControllConnecter;

    public CharacterModelController(CharacterModel model,Animator animator)
    {
        _characterModel = model;
        _animator = animator;

        if (model.IsGameModel)
        {
            ControllConnecter = ControllConnect();
        }

    }

    public IDisposable ControllConnect()
    {
        return
        GamePadObservable.GetAxisStickObservable(_characterModel.Player)
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

    public void SetResultPose(GameEnum.resultAnimPose pose)
    {
        _animator.Play(pose.ToString());
    }

    private void CharacterMove(GamepadStickInput.StickInfo info)
    {
        var tresureHeavy = Mathf.Pow(GameValue.SPEED_RATE, _characterModel.Tresures.Count);
        _characterModel.rigitbody.velocity = info.movePosition * GameValue.SPEED_BASE * tresureHeavy * _speedScale;
        info.RotatePosition(_characterModel.transform);
    }

    public void SetSpeedScale(float scale)
    {
        _speedScale = scale;
    }

    public void StopMove()
    {
        _characterModel.rigitbody.isKinematic = true;
        SetSpeedScale(0f);
    }
}
