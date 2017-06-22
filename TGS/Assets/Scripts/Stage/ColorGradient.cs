using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGradient : MonoBehaviour {

    [SerializeField]
    private Gradient color;

    [SerializeField]
    private Renderer renderer;

    [SerializeField,Range(0,1)]
    private float speed = 0.1f;

	void Start () {
        StartCoroutine(ColorGradationCoroutine());
	}
	
	private IEnumerator ColorGradationCoroutine()
    {
        var time = 0f;

        while (SystemManager.Instance.StageNum == 3)
        {
            var timeColor = (int)time % 2 == 0 ? color.Evaluate(time - (int)time) : color.Evaluate(1.0f - (time - (int)time));
            renderer.sharedMaterial.SetColor("_EmissionColor",timeColor);
            time += speed;

            yield return new WaitForSeconds(0.1f);
        }

    }
}
