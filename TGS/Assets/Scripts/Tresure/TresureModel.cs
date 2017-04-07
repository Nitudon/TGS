using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemParameter;
using UdonCommons;

public class TresureModel : ColorModel{ 

    public TresureModel(GameEnum.tresureColor color)
    {
        _tresureColor = color;
    }

}
