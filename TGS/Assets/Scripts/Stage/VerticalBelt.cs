using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonCommons;

public class VerticalBelt : MonoBehaviour {

    private static readonly Vector3 SPIN_FORCE = new Vector3(0f,0f,14.2f);

    [SerializeField]
    private Renderer BeltRenderer;

    [SerializeField]
    private float scrollSpeed = 1.5f;

    [SerializeField]
    private bool reflect = false;

    void FixedUpdate()
    {
        float offset = reflect ? Time.time * scrollSpeed : Time.time * -scrollSpeed;
        BeltRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }

    private void OnCollisionStay(Collision col)
    {
        if (ExtensionGameObject.HasComponent<CharacterModel>(col.gameObject))
        {
            AddBeltForceToRigitbody(col.gameObject);
        }
    }

    private void AddBeltForceToRigitbody(GameObject chara)
    {
        var force = reflect ? -SPIN_FORCE : SPIN_FORCE;
        var rigit = chara.GetComponent<Rigidbody>();

        rigit.AddForce(force);
    }
}
