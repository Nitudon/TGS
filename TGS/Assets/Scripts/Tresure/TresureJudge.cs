using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SystemParameter;
using UdonCommons;
using UnityEngine;

public class TresureJudge{

    private int CalculateScore()
    {

    }

	public bool JudgeTresures(GameEnum.tresureColor player, List<TresureModel> tresures)
    {
        if(tresures == null)
        {
            InstantLog.StringLogError("tresures is null");
            return false;
        }

        if (tresures.Where(x => x.TresureColor == player).Any())
        {
            var index = tresures.;
            var deleteTresures = tresures.GetRange(0,);
        }
    }

    public bool JudgeTresures(GameEnum.tresureColor player, out ColorModel front, List<TresureModel> tresures)
    {
        
    }

    public bool JudgeTresures(GameEnum.tresureColor player,List<TresureModel> tresures, out ColorModel backColor)
    {

    }

}
