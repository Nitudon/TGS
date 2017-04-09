using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemParameter;
using UdonCommons;

public class TresureModel : ColorModel{

    public CharacterModel Owner
    {
        get
        {
            if(_owner == null)
            {
                InstantLog.StringLogError("Owner is null");
                return null;
            }
            return _owner;
        }
    }

    private CharacterModel _owner;

    public bool HasOwner
    {
        get
        {
            return Owner != null;
        }
    }

    public TresureModel(GameEnum.tresureColor color)
    {
        _tresureColor = color;
    }

    public void SetOwner(CharacterModel model)
    {
        _owner = model;
    }

}
