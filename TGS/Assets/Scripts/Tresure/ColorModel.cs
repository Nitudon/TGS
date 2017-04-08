﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonCommons;
using SystemParameter;

public class ColorModel : UdonBehaviour {

    public ColorModel()
    {
        _tresureColor = GameEnum.tresureColor.nothing;
    }

    public ColorModel(GameEnum.tresureColor color)
    {
        _tresureColor = color;
    }

    public void SetColor(GameEnum.tresureColor color)
    {
        _tresureColor = color;
    }

    protected GameEnum.tresureColor _tresureColor;
    public GameEnum.tresureColor TresureColor
    {
        get
        {
            return _tresureColor;
        }
    }

    private System.Action _onDestroy;

    public void SetOnDestroy(System.Action action)
    {
        _onDestroy = action;
    }

    private void OnDestroy()
    {
        if (_onDestroy != null)
        {
            _onDestroy();
        }
    }
}
