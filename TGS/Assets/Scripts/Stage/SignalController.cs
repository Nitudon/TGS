using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UdonCommons;
using UdonObservable.ColiderRx;
using DG.Tweening;

public class SignalController : MonoBehaviour {

    private static readonly float SIGNAL_TIME = 15f;
    private static readonly float OFF_EMISSION_LIGHT = 0.2f;
    private static readonly float ON_EMISSION_LIGHT = 5f;
    private static readonly float BLUE_SPEED_SCALE = 2f;
    private static readonly float RED_SPEED_SCALE = 0.5f;
    private static readonly float BRIGHT_TIME = 0.3f;
    private static readonly int BRIGHT_COUNT = 10;

    [SerializeField]
    private Transform RightBars;

    [SerializeField]
    private Renderer[] VerticalLens;

    [SerializeField]
    private Renderer[] HorizontalLens;

    [SerializeField]
    private GameObject NormalColliders;

    [SerializeField]
    private GameObject VerticalColliders;

    [SerializeField]
    private GameObject HorizontalColliders;

    private List<CharacterModel> InternalModels;
    private IDisposable SignalChanger;
    private bool _isVertBlue = false;

	private void Start () {
        InternalModels = new List<CharacterModel>();
        SetSignalColor();
        StartCoroutine(SignalCoroutine());
    }

    private IEnumerator SignalCoroutine()
    {
        yield return new WaitUntil(() => SystemManager.Instance.IsGame);
        SignalChanger =
               Observable.Interval(TimeSpan.FromSeconds(SIGNAL_TIME))
               .Subscribe(_ => StartCoroutine(ChangeSignal()));

        gameObject.OnTriggerEnterAsObservable()
             .Where(x => x.gameObject.HasComponent<CharacterModel>())
             .Select(x => x.gameObject.GetComponent<CharacterModel>())
             .Subscribe(x => { x.SetSpeedScale(BLUE_SPEED_SCALE); InternalModels.Add(x); });

        gameObject.OnTriggerExitAsObservable()
            .Where(x => x.gameObject.HasComponent<CharacterModel>())
            .Select(x => x.gameObject.GetComponent<CharacterModel>())
            .Subscribe(x => InternalModels.RemoveAt(InternalModels.FindIndex(y => x.EqualColor(y))));

        VerticalColliders.OnTriggerEnterAsObservable()
             .Where(x => x.gameObject.HasComponent<CharacterModel>())
             .Select(x => x.gameObject.GetComponent<CharacterModel>())
             .Subscribe(x => x.SetSpeedScale(_isVertBlue ? RED_SPEED_SCALE : BLUE_SPEED_SCALE));

        HorizontalColliders.OnTriggerEnterAsObservable()
             .Where(x => x.gameObject.HasComponent<CharacterModel>())
             .Select(x => x.gameObject.GetComponent<CharacterModel>())
             .Subscribe(x => x.SetSpeedScale(_isVertBlue ? BLUE_SPEED_SCALE : RED_SPEED_SCALE));

        HorizontalColliders.OnTriggerEnterAsObservable()
             .Where(x => x.gameObject.HasComponent<CharacterModel>())
             .Select(x => x.gameObject.GetComponent<CharacterModel>())
             .Subscribe(x => x.SetSpeedScale(1.0f));

    }

    private IEnumerator ChangeSignal()
    {
        var bright = 0;
        var vertSignal = _isVertBlue ? VerticalLens[1].material : VerticalLens[0].material;
        var horiSignal = _isVertBlue ? HorizontalLens[0].material : HorizontalLens[1].material;
        var vertColor = vertSignal.GetColor("_EmissionColor");
        var horiColor = horiSignal.GetColor("_EmissionColor");

        while (bright < BRIGHT_COUNT)
        {
            bright++;
            if(bright%2 == 0)
            {
                vertSignal.SetColor("_EmissionColor", new Color());
                horiSignal.SetColor("_EmissionColor", new Color());
            }
            else
            {
                vertSignal.SetColor("_EmissionColor", vertColor);
                horiSignal.SetColor("_EmissionColor", horiColor);
            }
            yield return new WaitForSeconds(BRIGHT_TIME);
        }

        _isVertBlue = !_isVertBlue;
        RightBars.localEulerAngles += new Vector3(0, 90, 0);
        for(int i=0; i < CharacterManager.Instance.CharacterModels.Count; ++i)
        {
            var chara = CharacterManager.Instance.CharacterModels.ElementAt(i);
            if (InternalModels.Where(x => x.EqualColor(chara)).Any() == false && chara.GetSpeedScale != 1.0f)
            {
                var changeSpeed = chara.GetSpeedScale== BLUE_SPEED_SCALE ? RED_SPEED_SCALE : BLUE_SPEED_SCALE;
                chara.SetSpeedScale(changeSpeed);
            }
        }
        SetSignalColor();

        yield break;
    }
	
    private void SetSignalColor()
    {
        VerticalLens[0].material.SetColor("_EmissionColor", new Color(_isVertBlue ? OFF_EMISSION_LIGHT : ON_EMISSION_LIGHT, 0, 0));
        VerticalLens[1].material.SetColor("_EmissionColor", new Color(0, 0, _isVertBlue ? ON_EMISSION_LIGHT : OFF_EMISSION_LIGHT));
        HorizontalLens[0].material.SetColor("_EmissionColor", new Color(_isVertBlue ? ON_EMISSION_LIGHT : OFF_EMISSION_LIGHT, 0, 0));
        HorizontalLens[1].material.SetColor("_EmissionColor", new Color(0, 0, _isVertBlue ? OFF_EMISSION_LIGHT : ON_EMISSION_LIGHT));
    }

}
