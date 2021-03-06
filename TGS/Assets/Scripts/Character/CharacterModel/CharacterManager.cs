﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonCommons;

public class CharacterManager : UdonBehaviourSingleton<CharacterManager> {

    private List<CharacterModel> _characterModels;
    public List<CharacterModel> CharacterModels
    {
        get
        {
            if(_characterModels == null)
            {
                InstantLog.StringLogError("characterModels is null");
                _characterModels = new List<CharacterModel>();
            }

            return _characterModels;
        }
    }

    public CharacterModel GetCharacterModel(int index)
    {
        if(_characterModels == null)
        {
            InstantLog.StringLogError("characterModels is null");
            return null;
        }
        else
        {
            if(_characterModels.Count <= index || _characterModels.ElementAt(index) == null)
            {
                InstantLog.StringLogError("access to characterModels is wrong");
                return null;
            }
            else
            {
                return _characterModels.ElementAt(index);
            }
        }
    }

    public void AddCharacterModel(CharacterModel model)
    {
        if (_characterModels == null)
        {
            _characterModels = new List<CharacterModel>();
        }
       
            _characterModels.Add(model);
            _characterModels = _characterModels.OrderBy(x => x.Player).ToList();

    }

    public int GetCharacterScore(int index)
    {
        if (_characterModels == null)
        {
            InstantLog.StringLogError("characterModels is null");
            return 0;
        }
        else
        {
            if (_characterModels.Count <= index || _characterModels.ElementAt(index) == null)
            {
                InstantLog.StringLogError("access to characterModels is wrong");
                return 0;
            }
            else
            {
                return _characterModels.ElementAt(index).Score.Value;
            }
        }
    }

    public List<int> GetCharacterRankList()
    {
        return _characterModels.Select(x => _characterModels.Count(y => y.Score.Value > x.Score.Value) +1)
                               .ToList();
    }

    public int GetCharacterSumScore()
    {
        return _characterModels.Select(x => x.Score.Value).Sum();
    }

    public void InitCharacterList()
    {
        _characterModels = new List<CharacterModel>();
    }

    public void AllCharacterSetSpeedScale(float scale)
    {
        if (_characterModels == null)
        {
            InstantLog.StringLogError("characterModels is null");
            return;
        }
        else
        {
            for (int i = 0; i < _characterModels.Count; ++i)
            {
                _characterModels.ElementAt(i).SetSpeedScale(scale);
            }
        }
    }

    public void AllCharacterStop()
    {
        if (_characterModels == null)
        {
            InstantLog.StringLogError("characterModels is null");
            return;
        }
        else
        {
            for (int i = 0; i < _characterModels.Count; ++i)
            {
                _characterModels.ElementAt(i).StopMove();
            }
        }
    }

    public void AllCharacterSetVisible(bool value)
    {
        if (_characterModels == null)
        {
            InstantLog.StringLogError("characterModels is null");
            return;
        }
        else
        {
            for (int i = 0; i < _characterModels.Count; ++i)
            {
                _characterModels.ElementAt(i).SetVisible(value);
            }
        }
    }

}
