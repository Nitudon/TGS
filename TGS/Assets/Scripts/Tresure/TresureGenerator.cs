using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UdonCommons;
using SystemParameter;
using UniRx;

public class TresureGenerator :  UdonBehaviour{

    [SerializeField]
    private TresureModel[] Tresures;

    private bool _hasTresure;

    public bool HasTresure
    {
        get
        {
            return _hasTresure;
        }
    }

    private void Awake()
    {
        _hasTresure = false;
        Init();
    }

    public void Init()
    {
        SetGenerator();
    }

    private void SetGenerator()
    {
        TresureGenerateObservable();
    }

    private IDisposable TresureGenerateObservable()
    {
        return Observable.Interval(TimeSpan.FromSeconds(GameValue.GENERATE_TIME_SPAN))
            .Where(_ => _hasTresure == false && SystemManager.Instance.IsPause == false /*&& SystemManager.Instance.IsGame*/)
            .Subscribe(_ => Generate());
    }

    private void Generate(int index, Vector3 position)
    {
        var tresure = Tresures[index];

        tresure = Instantiate(tresure, transform,true);
        tresure.transform.localPosition = position;
        tresure.SetGenerator(this);
        _hasTresure = true;
    }

    private void Generate(Vector3 position)
    {
        int rand = RandMT.GenerateNext(Enum.GetValues(typeof(GameEnum.tresureColor)).Length);
        while (rand == 4)
        {
            rand = RandMT.GenerateNext(Enum.GetValues(typeof(GameEnum.tresureColor)).Length);
        }

        var tresure = Tresures[rand];

        tresure = Instantiate(tresure, transform);
        tresure.transform.localPosition = position;
        tresure.SetGenerator(this);
        _hasTresure = true;
    }

    private void Generate(int index)
    {
        var tresure = Tresures[index];

        tresure = Instantiate(tresure, transform);
        tresure.SetGenerator(this);
        _hasTresure = true;
    }

    private void Generate()
    {
        int rand = RandMT.GenerateNext(Enum.GetValues(typeof(GameEnum.tresureColor)).Length);
        while (rand == 4)
        {
            rand = RandMT.GenerateNext(Enum.GetValues(typeof(GameEnum.tresureColor)).Length);
        }

        var tresure = Tresures[rand];

        tresure = Instantiate(tresure, transform);
        tresure.SetGenerator(this);
        _hasTresure = true;
    }

    public void GetTresure()
    {
        _hasTresure = false;
    }

}
