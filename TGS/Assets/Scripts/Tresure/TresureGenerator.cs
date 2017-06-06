using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using UnityEngine;
using UdonCommons;
using SystemParameter;
using UniRx;
using DG.Tweening;

public class TresureGenerator :  UdonBehaviour{

    [SerializeField]
    private List<Transform> ChestPositions;

    [SerializeField]
    private List<Transform> GeneratePositions;

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

    protected override void Awake()
    {
        _hasTresure = false;
        Init();
    }

    public void Init()
    {
        SetGenerator();
        Generate(true);
    }

    private void SetGenerator()
    {
        TresureGenerateObservable();
    }

    private IDisposable TresureGenerateObservable()
    {
        return Observable.Interval(TimeSpan.FromSeconds(GameValue.GENERATE_TIME_SPAN))
            .Where(_ => _hasTresure == false && SystemManager.Instance.IsPause == false && SystemManager.Instance.IsGame)
            .Subscribe(_ => Generate())
            .AddTo(gameObject);
    }

    private void Generate(int index, Vector3 position)
    {
        var tresure = Tresures[index];

        tresure = Instantiate(tresure, transform,true);
        tresure.transform.position = position;
        tresure.SetGenerator(this);
        _hasTresure = true;
    }

    private void Generate(Vector3 position)
    {
        int rand = RandMT.GenerateNext(SystemManager.Instance.PlayerNum);
        while (rand == SystemManager.Instance.PlayerNum)
        {
            rand = RandMT.GenerateNext(SystemManager.Instance.PlayerNum);
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

    private void Generate(bool init = false)
    {
        int rand = GetIndexForRandom(SystemManager.Instance.PlayerNum);
        int c_pos = GetIndexForRandom(ChestPositions.Count);

        var tresure = Tresures[rand];
        var center = (position + ChestPositions.ElementAt(c_pos).position) / 2;

        tresure = Instantiate(tresure, transform);
        if (init)
        {
            tresure.transform.position = transform.position;
            tresure.Enable();
        }
        else
        {
            tresure.transform.position = ChestPositions.ElementAt(c_pos).position;
            tresure.transform.DOPath(new Vector3[] { tresure.position, center + new Vector3(0f, 0.7f, 0f), position }, 2.5f, PathType.CatmullRom)
                .OnComplete(() => tresure.Enable());
        }

         tresure.SetGenerator(this);
        _hasTresure = true;
    }

    private int GetIndexForRandom(int count)
    {
        int index = RandMT.GenerateNext(count);
        while (index == count)
        {
            index = RandMT.GenerateNext(count);
        }

        return index;
    }

    public void GetTresure()
    {
        _hasTresure = false;
    }

}
