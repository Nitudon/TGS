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

    private float speedScale;

    public CharacterModelController(CharacterModel model)
    {
        _characterModel = model;
        speedScale = 1.0f;

        ControllConnect();

        #region[For Debug]
#if UNITY_EDITOR
        foreach (GameEnum.direction dir in Enum.GetValues(typeof(GameEnum.direction)))
        {
            KeyObservable.GetKeyObservable(dirToKey(dir))
                .Subscribe(_ => ModelMove(dir));

            KeyObservable.GetKeyUpObservable(dirToKey(dir))
                .Subscribe(_ => StopMove());
        }
    }
#endif
    #endregion

    public void SetSpeedScale(float scale)
    {
        speedScale = scale;
    }

    public void ControllConnect()
    {
        GamePadObservable.GetAxisStickObservable()
            .Subscribe(x => CharacterMove(x));
    }

    private void CharacterMove(GamepadStickInput.StickInfo info)
    {
        var tresureHeavy = Mathf.Pow(GameValue.SPEED_RATE,_characterModel.Tresures.Count);
        _characterModel.rigitbody.AddForce(info.movePosition * GameValue.SPEED_BASE * tresureHeavy);
        info.RotatePosition(_characterModel.transform);
    }

    private void StopMove()
    {
        _characterModel.rigitbody.velocity = new Vector3();
    }

    #region[For Debug]
#if UNITY_EDITOR

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

    private void ModelMove(GameEnum.direction dir)
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
#endif
    #endregion
}
