using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonCommons;

public class SpinDisplay : UdonBehaviour {

    private static readonly float INTERVAL = 0.01f;

    [SerializeField]
    private float speed = 0.2f;

    protected override void OnEnable()
    {
        StartCoroutine(SpinCoroutine());
        base.OnEnable();
    }

    private IEnumerator SpinCoroutine()
    {
        while (gameObject.activeSelf)
        {
            RotY += speed;
            yield return new WaitForSeconds(INTERVAL);
        }
    }
}
