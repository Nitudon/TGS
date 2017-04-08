using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using SystemParameter;
using UdonCommons;
using UnityEngine;

public class TresureJudge{

    private int CalculateScore(List<ColorModel> models)
    {
        if(models == null)
        {
            InstantLog.StringLogError("Calculate models is null");
            return 0;
        }

        var score = 0;
        var array = models.ToArray();
        var colorCount = array.Select(x => x.TresureColor).Distinct().Count();
        var modelCount = array.Length;
        var hasPlayer = array.Where(x => x is CharacterModel).Any();

        score =
            (int)Mathf.Pow(GameValue.SCORE_RATE_CALCULATE, colorCount) * GameValue.SCORE_RATE_COLOR +
            (int)Mathf.Pow(GameValue.SCORE_RATE_CALCULATE, modelCount) * GameValue.SCORE_RATE_NUMBER +
            Convert.ToInt32(hasPlayer) * GameValue.SCORE_RATE_PLAYER;

        return score;
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
