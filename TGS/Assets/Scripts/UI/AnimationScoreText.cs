using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonCommons;
using TMPro;

public class AnimationScoreText : UdonBehaviour{

    [SerializeField]
    private TMP_Text subscription;

    private const float SPEED = 4f;
    private const float UP_BORDER = 150f;

    protected override void OnEnable()
    {
        posY = 0f;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public void Play(int score)
    {
        StartCoroutine(ScoreAnimation(score));
    }

    private IEnumerator ScoreAnimation(int score)
    {
        var pos = 0f;

        subscription.SetText("+" + score.ToString());

        while (pos < UP_BORDER)
        {
            pos += SPEED;
            posY += SPEED;
            yield return null;
        }

        gameObject.SetActive(false);
    }

}
