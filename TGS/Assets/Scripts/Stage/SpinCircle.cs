using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonCommons;
using DG.Tweening;
using SystemParameter;

public class SpinCircle : UdonBehaviour{

    private static float SPIN_SPEED_COEFFICIENT = 55.0f;

    [SerializeField]
    private float SpinSpan = 0.2f;

    [SerializeField]
    private GameObject RotateMesh;

    private bool isReflect = false;

	protected override void Start()
    {
        StartCoroutine(SpinCoroutine());
    }

    private IEnumerator SpinCoroutine()
    {
        yield return new WaitUntil(() => SystemManager.Instance.IsGame);

        while (SystemManager.Instance.IsGame)
        {
            RotateMesh.transform.localEulerAngles += new Vector3(0, 0, SpinSpan);
            if(isReflect == false && SystemManager.Instance.Time < GameValue.BATTLE_TIME / 2)
            {
                SpinSpan *= -1;
                isReflect = true;
            }
            yield return null;
        }

        yield break;
    }
	
    private void OnCollisionStay(Collision col)
    {
        if (ExtensionGameObject.HasComponent<CharacterModel>(col.gameObject))
        {
            AddSpinForceToRigitbody(col.gameObject);
        }
    }

    private void AddSpinForceToRigitbody(GameObject chara)
    {
        var charaTransform = chara.transform;
        var radVec = new Vector2(charaTransform.position.x - position.x, charaTransform.position.z - position.z);
        var rigit = chara.GetComponent<Rigidbody>();

        var circleVec = Quaternion.Euler(0f, 90f, 0f) * new Vector3(radVec.x,0f,radVec.y);

        rigit.AddForce(-circleVec * SPIN_SPEED_COEFFICIENT * SpinSpan);
    }
}
