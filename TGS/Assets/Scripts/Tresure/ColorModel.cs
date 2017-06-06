using System.Collections;
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

    

    private bool _isEnable = false;

    public bool IsEnable
    {
        get
        {
            return _isEnable;
        }
    }

    public virtual void Enable()
    {
        _isEnable = true;
    }

    protected GameEnum.tresureColor _tresureColor;
    public GameEnum.tresureColor TresureColor
    {
        get
        {
            return _tresureColor;
        }
    }

    public void SetColor(GameEnum.tresureColor color)
    {
        _tresureColor = color;
    }

    private int _listNumber;

    public int ListNumber
    {
        get
        {
            return _listNumber;
        }
    }

    public void SetListNumber(int num)
    {
        _listNumber = num;
    }

    public bool EqualColor(ColorModel another)
    {
        return _tresureColor == another._tresureColor;
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
