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

}
