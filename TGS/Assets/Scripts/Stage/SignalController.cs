using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UdonCommons;
using UdonObservable.ColiderRx;

public class SignalController : MonoBehaviour {

    private static readonly float SIGNAL_TIME = 15f;
    private static readonly float OFF_EMISSION_LIGHT = 0.2f;
    private static readonly float ON_EMISSION_LIGHT = 5f;
    private static readonly float BLUE_SPEED_SCALE = 2f;
    private static readonly float RED_SPEED_SCALE = 0.5f;

    [SerializeField]
    private Transform RightBars;

    [SerializeField]
    private Renderer[] VerticalLens;

    [SerializeField]
    private Renderer[] HorizontalLens;

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
               .Subscribe(_ => ChangeSignal());

        gameObject.OnTriggerEnterAsObservable()
             .Where(x => x.gameObject.HasComponent<CharacterModel>())
             .Select(x => x.gameObject.GetComponent<CharacterModel>())
             .Subscribe(x => { x.EnterZone(BLUE_SPEED_SCALE); InternalModels.Add(x); });

        gameObject.OnTriggerExitAsObservable()
            .Where(x => x.gameObject.HasComponent<CharacterModel>())
            .Select(x => x.gameObject.GetComponent<CharacterModel>())
            .Subscribe(x => { x.ExitZone(BLUE_SPEED_SCALE); InternalModels.RemoveAt(InternalModels.FindIndex(y => x.EqualColor(y))); });

        VerticalColliders.OnTriggerEnterAsObservable()
             .Where(x => x.gameObject.HasComponent<CharacterModel>())
             .Select(x => x.gameObject.GetComponent<CharacterModel>())
             .Subscribe(x => x.EnterZone(_isVertBlue ? RED_SPEED_SCALE : BLUE_SPEED_SCALE));

        HorizontalColliders.OnTriggerEnterAsObservable()
             .Where(x => x.gameObject.HasComponent<CharacterModel>())
             .Select(x => x.gameObject.GetComponent<CharacterModel>())
             .Subscribe(x => x.EnterZone(_isVertBlue ? BLUE_SPEED_SCALE : RED_SPEED_SCALE));

        VerticalColliders.OnTriggerExitAsObservable()
            .Where(x => ExtensionGameObject.HasComponent<CharacterModel>(x.gameObject))
            .Select(x => x.gameObject.GetComponent<CharacterModel>())
            .Subscribe(x => x.ExitZone(_isVertBlue ? RED_SPEED_SCALE : BLUE_SPEED_SCALE));

        HorizontalColliders.OnTriggerExitAsObservable()
            .Where(x => ExtensionGameObject.HasComponent<CharacterModel>(x.gameObject))
            .Select(x => x.gameObject.GetComponent<CharacterModel>())
            .Subscribe(x => x.ExitZone(_isVertBlue ? BLUE_SPEED_SCALE : RED_SPEED_SCALE));
    }

    private void ChangeSignal()
    {
        _isVertBlue = !_isVertBlue;
        RightBars.localEulerAngles += new Vector3(0, 90, 0);
        for(int i=0; i < CharacterManager.Instance.CharacterModels.Count; ++i)
        {
            var chara = CharacterManager.Instance.CharacterModels.ElementAt(i);
            if (chara.SpeedScaleZone.Count > 0 && InternalModels.Where(x => x.EqualColor(chara)).Any() == false)
            {
                var changeSpeed = chara.SpeedScaleZone.Last() == BLUE_SPEED_SCALE ? RED_SPEED_SCALE : BLUE_SPEED_SCALE;
                chara.ExitZone(chara.SpeedScaleZone.Last());
                chara.EnterZone(changeSpeed);
            }
        }
        SetSignalColor();
    }
	
    private void SetSignalColor()
    {
        VerticalLens[0].material.SetColor("_EmissionColor", new Color(_isVertBlue ? OFF_EMISSION_LIGHT : ON_EMISSION_LIGHT, 0, 0));
        VerticalLens[1].material.SetColor("_EmissionColor", new Color(0, 0, _isVertBlue ? ON_EMISSION_LIGHT : OFF_EMISSION_LIGHT));
        HorizontalLens[0].material.SetColor("_EmissionColor", new Color(_isVertBlue ? ON_EMISSION_LIGHT : OFF_EMISSION_LIGHT, 0, 0));
        HorizontalLens[1].material.SetColor("_EmissionColor", new Color(0, 0, _isVertBlue ? OFF_EMISSION_LIGHT : ON_EMISSION_LIGHT));
    }

}
