using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using SystemParameter;
using UdonCommons;
using UnityEngine;

public static class TresureJudgeHelper{

    private static int CalculateScore(List<ColorModel> models)
    { 
        if(models == null)
        {
            InstantLog.StringLogError("Calculate models is null");
            return 0;
        }

        var score = 0;
        var modelCount = models.Count;
        float bonusRate = (modelCount > 2) ? 1 + 0.25f * (modelCount - 2) : 1;

        score = (int)(modelCount * GameValue.SCORE_RATE_NUMBER * bonusRate);

        return score;
    }

    public static bool JudgeTresures(CharacterModel player, out int score)
    {
        score = 0;

        var tresures = player.Tresures.ToList();

        if (tresures == null)
        {
            InstantLog.StringLogError("tresures is null");
            return false;
        }

        if (player.TresureColor == tresures.Last().TresureColor && tresures.Count > 1 && tresures.Select(x => x.TresureColor).Distinct().Count() > 1)
        {
            score = CalculateScore(tresures);
            player.RemoveTresureAll();
            return true;
        }
        else
        {
            return JudgeColorList(player,tresures,ref score);
        }
    }

    public static bool JudgeCharacter(CharacterModel backPlayer, CharacterModel frontPlayer, out int score)
    {
        bool judge;

        score = 0;

        var frontList = frontPlayer.Tresures.ToList();
        var backList = backPlayer.Tresures.ToList();
        var frontCount = frontList.Where(x => x is TresureModel).Count();
        var backCount = backList.Where(x => x is TresureModel).Count();

        if (frontList == null || backList == null)
        {
            InstantLog.StringLogError("tresures is null");
            return false;
        }

        if(frontCount < 1 && backCount < 2)
        {
            return false;
        }
        else
        {
            frontList.Insert(0,frontPlayer);
            backList.AddRange(frontList);
            judge = JudgeColorList(backPlayer,backList,ref score);
            if (judge)
            {
                score += GameValue.SCORE_RATE_PLAYER;
            }
            return judge;
        }
        
    }

    private static bool JudgeColorList(CharacterModel player,List<ColorModel> list, ref int score)
    {
        if (list.Where(x => x is CharacterModel).Any() == false)
        {
            for (int col = 1; col < GameValue.COLOR_LIST_COUNT + 1; ++col)
            {
                GameEnum.tresureColor color = (GameEnum.tresureColor)col;
                var colorList = list.Where(x => x.TresureColor == color);
                var isSame = colorList.Count();
                if (isSame < 2)
                {
                    continue;
                }
                var indexList = colorList.Select(x => x.ListNumber);
                for (int j = 0; j < indexList.Count() - 1; ++j)
                {
                    if (indexList.ElementAt(j) + 1 != indexList.ElementAt(j + 1))
                    {
                        int start = indexList.First();
                        int count = indexList.Last() - start + 1;
                        if (list.Last() is CharacterModel)
                        {
                            count--;
                        }
                        player.RemoveTresureRange(start, count);
                        score = CalculateScore(list.GetRange(start, count));
                        return true;
                    }
                }
            }
        }
        else
        {
            var frontPlayer = list.FindLast(x => x is CharacterModel) as CharacterModel;

            if (player.EqualColor(list.Last()))
            {
                frontPlayer.RemoveTresureAll();
                player.RemoveTresureAll();
                frontPlayer.SetGamePose(GameEnum.animTrigger.crash);
                score = CalculateScore(list.GetRange(1,list.Count-1));
                return true;
            }
            else if (player.Tresures.Where(x => x.EqualColor(frontPlayer)).Any())
            {
                GameEnum.tresureColor color = frontPlayer.TresureColor;
                var colorList = player.Tresures.Where(x => x.TresureColor == color);
                var isSame = colorList.Count();
                if(isSame < 2)
                {
                    return false;
                }
                var indexList = colorList.Select(x => x.ListNumber);
                for (int j = 0; j < indexList.Count() - 1; ++j)
                {
                    if (indexList.ElementAt(j) + 1 != indexList.ElementAt(j + 1))
                    {
                        int start = indexList.First();
                        int count = indexList.Last() - start;
                        player.RemoveTresureRange(start, count);
                        var scoreList = list.GetRange(start, count);
                        scoreList.Add(frontPlayer);
                        score = CalculateScore(scoreList);
                        return true;
                    }
                }
            }

        }
        return false;
    }

}
