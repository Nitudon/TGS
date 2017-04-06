using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UdonCommons;
using SystemParameter;
using UdonObservable.InputRx.Key;

public class CharacterModelController{

    private CharacterModel _characterModel;

    private float speedScale;

    public CharacterModelController(CharacterModel model)
    {
        _characterModel = model;
        speedScale = 1.0f;

        foreach (GameEnum.direction dir in Enum.GetValues(typeof(GameEnum.direction))) {
            KeyObservable.GetKeyObservable(dirToKey(dir))
                .Subscribe(_ => modelMove(dir));

        }
    }    

    private KeyCode dirToKey(GameEnum.direction dir)
    {
        switch (dir)
        {
            case GameEnum.direction.up:
                return KeyCode.UpArrow;
                break;
            case GameEnum.direction.down:
                return KeyCode.DownArrow;
                break;
            case GameEnum.direction.left:
                return KeyCode.LeftArrow;
                break;
            case GameEnum.direction.right:
                return KeyCode.RightArrow;
                break;
            default:
                InstantLog.StringLogError("Wrong Direction");
                return KeyCode.Space;
        }
    }

    public void SetSpeedScale(float scale)
    {
        speedScale = scale;
    }

    private void modelMove(GameEnum.direction dir)
    {
        switch (dir)
        {
            case GameEnum.direction.up:
                _characterModel.posZ += GameValue.MOVE_BASE_SPEED * speedScale;
                break;
            case GameEnum.direction.down:
                _characterModel.posZ -= GameValue.MOVE_BASE_SPEED * speedScale;
                break;
            case GameEnum.direction.left:
                _characterModel.posX -= GameValue.MOVE_BASE_SPEED * speedScale;
                break;
            case GameEnum.direction.right:
                _characterModel.posX += GameValue.MOVE_BASE_SPEED * speedScale;
                break;
            default:
                InstantLog.StringLogError("Wrong Direction");
                return;
        }

        return;
    }
}
