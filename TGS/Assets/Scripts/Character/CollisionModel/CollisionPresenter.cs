using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UdonCommons;
using SystemParameter;
using UdonObservable.ColiderRx;

[RequireComponent(typeof(Collider))]
public class CollisionPresenter : UdonBehaviour {

    [SerializeField]
    private CharacterModel Character;

    private Collider _collider;

    [SerializeField]
    private CollisionView _view;

    protected override void Start()
    {
        SetEvents();
        ObserveCollision();
    }

    private void ObserveCollision()
    {
        ColiderObservable.OnTriggerEnterObservable(gameObject)
            .Where(x => x.gameObject != Character && ExtensionGameObject.HasComponent<ColorModel>(x.gameObject))
            .Subscribe(x => _view.OnTriggerEntered(x.gameObject))
            .AddTo(gameObject);

        ColiderObservable.OnTriggerExitObservable(gameObject)
            .Where(x => x.gameObject != Character && ExtensionGameObject.HasComponent<ColorModel>(x.gameObject))
            .Subscribe(x => _view.OnTriggerExited(x.gameObject))
            .AddTo(gameObject);
    }

    private void SetEvents()
    {
        _view.OnTriggerEnteredListener = OnCollisionEntered;
        _view.OnTriggerExitListener = OnCollisionExited;
    }

    private void OnCollisionEntered(GameObject go)
    {
        ColorModel model = go.GetComponent<ColorModel>();

        if (model.IsEnable)
        {
            Character.AddColor(model);

            posZ = GameValue.OWN_TRESURE_POSITION_OFFSET * Character.Tresures.Count;
        }
    }

    private void OnCollisionExited(GameObject go)
    {
        ColorModel model = go.GetComponent<ColorModel>();

        if (model is CharacterModel && Character.Tresures.Count > 0) 
        {
            Character.RemoveTresure(Character.Tresures.Count-1,false);
        }


        posZ = GameValue.OWN_TRESURE_POSITION_OFFSET * Character.Tresures.Count;
    }

}
