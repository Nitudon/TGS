using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemParameter;
using UdonCommons;

public class TresureModel : ColorModel{

    [SerializeField]
    private GameEnum.tresureColor Color;

    private TresureGenerator _tresureGenerator;

    protected override void Awake()
    {
        SetColor(Color);
        StartCoroutine(TresureMoveCoroutine());
    }

    public CharacterModel Owner
    {
        get
        {
            return _owner;
        }
    }

    private CharacterModel _owner;

    private int _ownIndex;

    public int GetIndex
    {
        get
        {
            if (HasOwner) {
                return _ownIndex;
            }
            else
            {
                return -1;
            }
        }
    }

    public bool HasOwner
    {
        get
        {
            return Owner != null;
        }
    }

    public TresureModel(GameEnum.tresureColor color)
    {
        _tresureColor = color;
    }

    public void SetOwner(CharacterModel model)
    {
        if (model == null)
        {
            InstantLog.StringLogError("Owner model is Null");
            return;
        }

        _owner = model;
        _ownIndex = model.Tresures.Count-1;
    }

    public void SetGenerator(TresureGenerator generator)
    {
        if (generator == null)
        {
            InstantLog.StringLogError("Generator model is Null");
            return;
        }

        _tresureGenerator = generator;
    }

    public void GetTresure(CharacterModel model)
    {
        SetOwner(model);
       _tresureGenerator.GetTresure();
        RotY = 0;
    }

    private IEnumerator TresureMoveCoroutine()
    {
        while (SystemManager.Instance.IsPause | _owner != null == false)
        {
            RotZ += GameValue.SPIN_SPEED;
            yield return null;
        }
    }

    public void BreakTresure()
    {
        if (SystemManager.Instance.IsGame)
        {
            ParticleManager.Instance.GetParticleEvent(0).NumEmit(position);
        }
    }

}
