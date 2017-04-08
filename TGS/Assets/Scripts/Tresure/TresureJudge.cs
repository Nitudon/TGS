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

	public bool JudgeTresures(GameEnum.tresureColor player, List<ColorModel> tresures, out int score)
    {
        score = 0;

        if(tresures == null)
        {
            InstantLog.StringLogError("tresures is null");
            return false;
        }

        if (tresures.Where(x => x.TresureColor == player).Any())
        {
            var index = tresures.LastIndexOf(tresures.LastOrDefault(x => x.TresureColor == player));
            if (index == tresures.Count - 1)
            {
                return false;
            }

            var deleteTresures = tresures.GetRange(index, tresures.Count - 1);
            score = CalculateScore(deleteTresures);
            if (index > 0)
            {
                tresures = tresures.GetRange(0, index - 1);
            }
            else
            {
                tresures = new List<ColorModel>();
            }

            return true;
        }

        else
        {
            return false;
        }
    }

    //public bool JudgeTresures(GameEnum.tresureColor player, out ColorModel front, List<TresureModel> tresures)
    //{
        
    //}

    //public bool JudgeTresures(GameEnum.tresureColor player,List<TresureModel> tresures, out ColorModel backColor)
    //{

    //}

}
