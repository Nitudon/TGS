﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UdonCommons;
using UdonObservable.ColiderRx;

[RequireComponent(typeof(Collider))]
public class CollisionPresenter : MonoBehaviour {

    [SerializeField]
    private CharacterModel Character;

    private Collider _collider;

    private CollisionView _view;

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

        Character.AddTresure(model);
    }

    private void OnCollisionExited(GameObject go)
    {

    }

}
