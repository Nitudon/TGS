using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour{

    [SerializeField]
    private Animator GameStartAnimator;

    [SerializeField]
    private GameObject SystemCanvas;

    [SerializeField]
    private GameObject GameStartUI;

    [SerializeField]
    private GameObject GameEndUI;

    private enum BattleState{StartAnimation,PlayingGame,EndAnimation}

    public bool isStartingGame
    {
        get
        {
            return GameStartAnimator.GetCurrentAnimatorStateInfo(0).IsName(BattleState.StartAnimation.ToString());
        }
    }

    public bool isPlayingGame
    {
        get
        {
            return GameStartAnimator.GetCurrentAnimatorStateInfo(0).IsName(BattleState.PlayingGame.ToString());
        }
    }

    public bool isEndingGame
    {
        get
        {
            return GameStartAnimator.GetCurrentAnimatorStateInfo(0).IsName(BattleState.EndAnimation.ToString());
        }
    }

    public void GameStart()
    {
        GameStartUI.SetActive(false);
    }

    public void GameEnd()
    {
        GameEndUI.SetActive(false);
    }
}
