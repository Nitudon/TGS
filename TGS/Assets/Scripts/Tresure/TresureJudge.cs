using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using SystemParameter;
using UdonCommons;
using UnityEngine;

public class TresureJudge{

    public void DebugJudge(GameEnum.tresureColor player,ref List<GameEnum.tresureColor> models)
    {
        int score;
        foreach (GameEnum.tresureColor elm in models)
        {
            InstantLog.StringLog(elm.ToString());
        }
    }

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

        InstantLog.ObjectLog(colorCount + ":" + modelCount + ":" + hasPlayer);

        score =
            (int)Mathf.Pow(GameValue.SCORE_RATE_CALCULATE, colorCount) * GameValue.SCORE_RATE_COLOR +
            (int)Mathf.Pow(GameValue.SCORE_RATE_CALCULATE, modelCount) * GameValue.SCORE_RATE_NUMBER +
            Convert.ToInt32(hasPlayer) * GameValue.SCORE_RATE_PLAYER;

        return score;
    }

    public bool JudgeTresures(CharacterModel player, out int score, out int start, out int count)
    {
        score = start = count= 0;

        var tresures = player.Tresures.ToList();

        if (tresures == null)
        {
            InstantLog.StringLogError("tresures is null");
            return false;
        }

        if (player.TresureColor == tresures.Last().TresureColor && tresures.Count > 1 && tresures.Select(x => x.TresureColor).Distinct().Count() > 1)
        {
            score = CalculateScore(tresures);
            count = tresures.Count-1;
            return true;
        }
        else
        {
            var tresuresNoTail = tresures.Count > 1 ? tresures.GetRange(0, tresures.Count - 1) : new List<ColorModel>();

            if (tresuresNoTail.Count > 0 && tresuresNoTail.Where(x => x.TresureColor == tresures.Last().TresureColor).Any())
            {
                var index = tresuresNoTail.FindLastIndex(x => x.EqualColor(tresures.Last()));

                if (index == tresures.Count - 2)
                {
                    return false;
                }

                var deleteModelsCount = tresures.Count - index;
                var deleteTresures = tresures.GetRange(index, deleteModelsCount);

                score = CalculateScore(deleteTresures);
                if (index > 0)
                {
                    start = index;
                    count = deleteModelsCount-1;
                }
                else
                {
                    count = tresures.Count;
                }

                return true;
            }

            else
            {
                return false;
            }
        }
    }

    public bool JudgeCharacter(CharacterModel backPlayer, CharacterModel frontPlayer, out int score, out int start, out int count)
    {
        score = count = 0;

        start = 1;

        var tresures = frontPlayer.Tresures.ToList();

        if (tresures == null)
        {
            InstantLog.StringLogError("tresures is null");
            return false;
        }

        var frontList = frontPlayer.Tresures.ToList();
        frontList.Insert(0, frontPlayer);

        if (frontList.Count > 0 && frontList.Where(x => x.TresureColor == tresures.Last().TresureColor).Any())
        {

            if (backPlayer.TresureColor == tresures.Last().TresureColor && tresures.Count > 1)
            {
                score = CalculateScore(frontList);
                count = tresures.Count;
            }
            else
            {
                var index = frontList.FindLastIndex(x => x.EqualColor(tresures.Last()));

                if (index == tresures.Count - 2)
                {
                    return false;
                }

                var deleteModelsCount = index + 1;
                var deleteTresures = tresures.GetRange(0, deleteModelsCount);

                score = CalculateScore(deleteTresures);

                if (index > 0)
                {
                    count = deleteModelsCount;
                }
                else
                {
                    count = tresures.Count;
                }
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
