using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonCommons;
using TMPro;
using SystemParameter;
using DG.Tweening;

public class AnimationScoreText : UdonBehaviour{

    [SerializeField]
    private TMP_Text subscription;

    [SerializeField]
    private GameObject Baloon;

    private const float SPEED = 3.0f;
    private const float UP_BORDER = 135f;

    protected override void OnEnable()
    {
        posY = 0f;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public void Play(bool player,int score)
    {
        ScoreAnimation(player,score);
    }

    private void ScoreAnimation(bool player, int score)
    {
        Baloon.SetActive(player);

        var count = player ? (score - GameValue.SCORE_RATE_PLAYER) / GameValue.SCORE_RATE_NUMBER : score / GameValue.SCORE_RATE_NUMBER;
        subscription.SetText((score - GameValue.SCORE_RATE_PLAYER).ToString());

        var posA = localPosition + new Vector3(20f,50f,0f);
        var posB = localPosition + new Vector3(-20f, 100f, 0f);
        var posC = localPosition + new Vector3(0f, 150f, 0f);

        transform.DOLocalPath(new Vector3[] {posA, posB, posC},1.5f,PathType.CatmullRom)
            .OnComplete(() => gameObject.SetActive(false));
    }

}
