using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonCommons;

public class CharacterManager : UdonBehaviourSingleton<CharacterManager> {

    private void Start()
    {
        TresureJudge j = new TresureJudge() ;
        var red = SystemParameter.GameEnum.tresureColor.red;
        var blue = SystemParameter.GameEnum.tresureColor.blue;
        var yellow = SystemParameter.GameEnum.tresureColor.yellow;
        var green = SystemParameter.GameEnum.tresureColor.green;
        var list = new List<SystemParameter.GameEnum.tresureColor> { red, yellow,green, blue, green,green};
        j.DebugJudge(SystemParameter.GameEnum.tresureColor.red,ref list);
    }

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
