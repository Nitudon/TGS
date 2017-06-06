using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonCommons;
using DG.Tweening;

public class SpinCircle : UdonBehaviour{

    [SerializeField]
    private float SpinSpan = 5.0f;

	void Update()
    {
        RotZ += 0.5f;
    }
	
}
