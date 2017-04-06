using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemParameter;
using UdonCommons;

public class TresureModel : UdonBehaviour{ 

    private GameEnum.tresureColor _tresureColor;

    private System.Action _onDestroy;

    public TresureModel(GameEnum.tresureColor color)
    {
        _tresureColor = color;
    }

    public void SetOnDestroy(System.Action action)
    {
        _onDestroy = action;
    }

    private void OnDestroy()
    {
        _onDestroy();
    }
}
