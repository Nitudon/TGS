using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonCommons;

public class TresureModel : UdonBehaviour{ 

	public enum TresureColor { red, blue, yellow, green }

    private TresureColor _tresureColor;

    private System.Action _onDestroy;

    public TresureModel(TresureColor color)
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
