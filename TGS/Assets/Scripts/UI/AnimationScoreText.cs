using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonCommons;
using TMPro;

public class AnimationScoreText : UdonBehaviour{

    [SerializeField]
    private TMP_Text subscription;

    private const float SPEED = 0.5f;
    private const float UP_BORDER = 30f;

    protected override void OnEnable()
    {
        posY = 0;
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
        subscription.SetText("+" + score.ToString());

        while (posY < UP_BORDER)
        {
            posY += SPEED;
            yield return null;
        }

        gameObject.SetActive(false);
    }

}
