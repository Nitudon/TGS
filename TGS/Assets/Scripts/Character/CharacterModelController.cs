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

            KeyObservable.GetKeyUpObservable(dirToKey(dir))
                .Subscribe(_ => stopMove());
        }
    }    

    private KeyCode dirToKey(GameEnum.direction dir)
    {
        switch (dir)
        {
            case GameEnum.direction.up:
                return KeyCode.UpArrow;
      
            case GameEnum.direction.down:
                return KeyCode.DownArrow;
                
            case GameEnum.direction.left:
                return KeyCode.LeftArrow;
                
            case GameEnum.direction.right:
                return KeyCode.RightArrow;
                
            default:
                InstantLog.StringLogError("Wrong Direction");
                return KeyCode.Space;
        }
    }

    public void SetSpeedScale(float scale)
    {
        speedScale = scale;
    }

    private void stopMove()
    {
        _characterModel.rigitbody.velocity = new Vector3();
    }

    private void modelMove(GameEnum.direction dir)
    {
        switch (dir)
        {
            case GameEnum.direction.up:
                _characterModel.rigitbody.AddForce(new Vector3(0, 0, GameValue.MOVE_BASE_SPEED * speedScale));
                _characterModel.SetLocalEulerAngles(GameValue.UP_DIRECTION_ANGLE);
                break;
            case GameEnum.direction.down:
                _characterModel.rigitbody.AddForce(new Vector3(0, 0, -GameValue.MOVE_BASE_SPEED * speedScale));
                _characterModel.SetLocalEulerAngles(GameValue.DOWN_DIRECTION_ANGLE);
                break;
            case GameEnum.direction.left:
                _characterModel.rigitbody.AddForce(new Vector3(-GameValue.MOVE_BASE_SPEED * speedScale, 0, 0));
                _characterModel.SetLocalEulerAngles(GameValue.LEFT_DIRECTION_ANGLE);
                break;
            case GameEnum.direction.right:
                _characterModel.rigitbody.AddForce(new Vector3(GameValue.MOVE_BASE_SPEED * speedScale, 0, 0));
                _characterModel.SetLocalEulerAngles(GameValue.RIGHT_DIRECTION_ANGLE);
                break;
            default:
                InstantLog.StringLogError("Wrong Direction");
                return;
        }

        return;
    }
}
