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

    private void Start()
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

        Character.AddColor(model);

        posZ = GameValue.OWN_TRESURE_POSITION_OFFSET * Character.Tresures.Count;
    }

    private void OnCollisionExited(GameObject go)
    {
        if(ExtensionGameObject.HasComponent<CharacterModel>(go) && Character.Tresures.Count > 0)
        {
            Character.RemoveTresure(Character.Tresures.Count-1);
            posZ = GameValue.OWN_TRESURE_POSITION_OFFSET * Character.Tresures.Count;
        }
    }

}
