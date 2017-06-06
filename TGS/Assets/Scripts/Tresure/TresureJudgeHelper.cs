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
        var colorCount = models.Select(x => x.TresureColor).Distinct().Count();
        var modelCount = models.Count;

        score =
            (int)Mathf.Pow(GameValue.SCORE_RATE_CALCULATE, colorCount) * GameValue.SCORE_RATE_COLOR +
            (int)Mathf.Pow(GameValue.SCORE_RATE_CALCULATE, modelCount) * GameValue.SCORE_RATE_NUMBER;

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

        var backList = backPlayer.Tresures.ToList();
        var frontList = frontPlayer.Tresures.ToList();

        if (frontList == null || backList == null)
        {
            InstantLog.StringLogError("tresures is null");
            return false;
        }

        if(frontList.Count < 1 && backList.Count < 2)
        {
            return false;
        }
        else if (backList.Count < 2)
        {
            if (frontList.Where(x => x.TresureColor == backPlayer.TresureColor).Any())
            {
                var index = frontList.FindLastIndex(x => x.TresureColor == backPlayer.TresureColor);
                frontPlayer.RemoveTresureRange(0,index+1);
                score += GameValue.SCORE_RATE_PLAYER;
                return true;
            }

            return false;
        }
        else if (frontList.Count < 1)
        {
            judge = JudgeColorList(backPlayer, backList, ref score);
            if (judge)
            {
                score += GameValue.SCORE_RATE_PLAYER;
            }
            return judge;
        }
        else
        {
            frontList.Insert(0,frontPlayer);
            backList.AddRange(frontList);
            backList.Insert(0,backPlayer);
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
        if (list.Where(x => x is CharacterModel).Any() == false || list.Last() is CharacterModel)
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
                score = CalculateScore(list);
                return true;
            }
            else if (player.Tresures.Where(x => x.EqualColor(frontPlayer)).Any())
            {
                var playerRemoveIndex = player.Tresures.ToList().FindLastIndex(x => x.EqualColor(list.Last()) && !(x is CharacterModel));
                player.RemoveTresureRange(playerRemoveIndex,player.Tresures.Count - playerRemoveIndex);
                score = CalculateScore(list.GetRange(playerRemoveIndex, list.Count - playerRemoveIndex));
                return true;
            }

        }
        return false;
    }

}
