using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UdonCommons;
using UdonObservable.ColiderRx;

[RequireComponent(typeof(Collider))]
public class CollisionPresenter : MonoBehaviour {

    [SerializeField]
    private GameObject Character;

    private Collider _collider;

    private CollisionView _view;

    private void ObserveCollision()
    {
        ColiderObservable.OnTriggerEnterObservable(gameObject)
            .Where(x => x.gameObject != Character && ExtensionGameObject.HasComponent<ColorModel>(x.gameObject))
            .Subscribe(_ => _view.OnTriggerEntered())
            .AddTo(gameObject);

        ColiderObservable.OnTriggerExitObservable(gameObject)
            .Where(x => x.gameObject != Character && ExtensionGameObject.HasComponent<ColorModel>(x.gameObject))
            .Subscribe(_ => _view.OnTriggerExited())
            .AddTo(gameObject);

    }

    private void SetEvents()
    {
        _view.OnTriggerEnteredListener = OnCollisionEntered;
        _view.OnTriggerExitListener = OnCollisionExited;
    }

    private void OnCollisionEntered()
    {

    }

    private void OnCollisionExited()
    {

    }

}
