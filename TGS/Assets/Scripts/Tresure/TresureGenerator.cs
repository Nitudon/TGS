using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    private bool AutoEmit = true;

    private bool _hasTresure;

    private IDisposable _generateController;

    private TresureModel _tresure;

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
        if (AutoEmit)
        {
            Init();
        }
    }

    public void HiddenGeneraterSet()
    {
        _generateController = Observable.Interval(TimeSpan.FromSeconds(GameValue.GENERATE_TIME_SPAN))
            .Where(_ => _hasTresure == false && SystemManager.Instance.IsPause == false && SystemManager.Instance.IsGame)
            .Subscribe(_ => Generate())
            .AddTo(gameObject);
    }

    public void Init(bool gameStart = true)
    {
        SetGenerator();
        Generate(gameStart);
    }

    public void Insert()
    {
        SetGenerator();
    }

    private void SetGenerator()
    {
        _generateController = TresureGenerateObservable();
    }

    public void Dispose()
    {
        _generateController.Dispose();
        if (_tresure.HasOwner == false) {
            Destroy(_tresure.gameObject);
        }
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
        _tresure = Tresures[index];

        _tresure = Instantiate(_tresure, transform,true);
        _tresure.transform.position = position;
        _tresure.SetGenerator(this);
        _hasTresure = true;
    }

    private void Generate(Vector3 position)
    {
        int rand = RandMT.GenerateNext(SystemManager.Instance.PlayerNum);
        while (rand == SystemManager.Instance.PlayerNum)
        {
            rand = RandMT.GenerateNext(SystemManager.Instance.PlayerNum);
        }

        _tresure = Tresures[rand];

        _tresure = Instantiate(_tresure, transform);
        _tresure.transform.localPosition = position;
        _tresure.SetGenerator(this);
        _hasTresure = true;
    }

    private void Generate(int index)
    {
        _tresure = Tresures[index];

        _tresure = Instantiate(_tresure, transform);
        _tresure.SetGenerator(this);
        _hasTresure = true;
    }

    public void Generate(bool init = false)
    {
        int rand = GetIndexForRandom(SystemManager.Instance.PlayerNum);
        int c_pos = GetIndexForRandom(ChestPositions.Count);

        _tresure = Tresures[rand];      

        var center = (position + ChestPositions.ElementAt(c_pos).position) / 2;

        _tresure = Instantiate(_tresure, transform);
        if (init)
        {
            _tresure.transform.position = transform.position;
            _tresure.Enable();
        }
        else
        {
            _tresure.transform.position = ChestPositions.ElementAt(c_pos).position;
            _tresure.transform.DOPath(new Vector3[] { _tresure.position, center + new Vector3(0f, 0.7f, 0f), position }, 2.5f, PathType.CatmullRom)
                .OnComplete(() => _tresure.Enable());
        }

         _tresure.SetGenerator(this);
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
