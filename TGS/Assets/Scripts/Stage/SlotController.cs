using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UdonCommons;
using UniRx;
using SystemParameter;

public class SlotController : MonoBehaviour {

    private static readonly float SPEED_UP_SCALE = 2f;
    private static readonly float SPEED_DOWN_SCALE = 0.5f;
    private static readonly float SLOT_SPIN_TIME = 2.5f;
    private static readonly float SLOT_SPIN_LOT = 5f;
    private static readonly float SLOT_SPAN = 20f;
    private static readonly float SLOT_SPACE = 0.1667f;
    private static readonly int SLOT_VALUE_NUM = 6;

    [SerializeField]
    private Renderer SlotRenderer;

    [SerializeField]
    private TresureGenerator[] Generators;

    [SerializeField]
    private TresureGenerator[] HideGenerators;

    private int _slotIndex;

    private IDisposable SlotSpiner;

    private void Start()
    {
        _slotIndex = 0;
        for (int i = 0; i < HideGenerators.Length; ++i)
        {
            HideGenerators[i].gameObject.SetActive(false);
        }
        SlotRenderer.sharedMaterial.mainTextureOffset = new Vector2();
        StartCoroutine(SpinCoroutine());
    }

    private IEnumerator SpinCoroutine()
    {
        yield return new WaitUntil(() => SystemManager.Instance.IsGame);

        StartSlot();

        SlotSpiner = Observable.Interval(TimeSpan.FromSeconds(SLOT_SPAN))
            .Where(_ => SystemManager.Instance.IsGame)
            .Subscribe(_ => StartSlot());
    }

    private void ResetSlotEffect()
    {
        if(_slotIndex % 3 == 1)
        {
            for (int i = 0; i < HideGenerators.Length; ++i)
            {
                HideGenerators[i].Dispose();
                HideGenerators[i].gameObject.SetActive(false);
            }
        }
        else if (_slotIndex == 5)
        {
            CharacterManager.Instance.AllCharacterSetVisible(true);
        }
        else
        {
            CharacterManager.Instance.AllCharacterSetSpeedScale(1.0f);
        }
    }

    private void StartSlot()
    {
        var SlotIndex = (int)RandMT.GenerateNext(6);
        var SpinOffset = SlotRenderer.sharedMaterial.mainTextureOffset.y - SLOT_SPACE * (SLOT_VALUE_NUM * SLOT_SPIN_LOT + ((SlotIndex > _slotIndex) ? SlotIndex - _slotIndex : SLOT_VALUE_NUM - _slotIndex + SlotIndex));

        DOTween.To(
            () => SlotRenderer.sharedMaterial.mainTextureOffset,
            offset => SlotRenderer.sharedMaterial.mainTextureOffset = offset,
            new Vector2(0, SpinOffset),
            SLOT_SPIN_TIME
            ).OnComplete(() => AffectSlot(SlotIndex));
    }

    private void AffectSlot(int SlotIndex)
    {
        ResetSlotEffect();
        _slotIndex = SlotIndex;
        SlotRenderer.sharedMaterial.mainTextureOffset = new Vector2(0, -SLOT_SPACE * _slotIndex);
        SlotEffect(_slotIndex);
    }

    private void SlotEffect(int index)
     {
         switch (index%3) {
             case 0:
                SpeedUpEffect();
                break;

            case 1:
                InsertTresureEffect();
                break;

            case 2:
                if(index == 2)
                {
                    SpeedDownEffect();
                    break;
                }
                else
                {
                    InvisibleEffect();
                    break;
                }
         }
     }

    private void SpeedUpEffect()
    {
        CharacterManager.Instance.AllCharacterSetSpeedScale(SPEED_UP_SCALE);
    }

    private void SpeedDownEffect()
    {
        CharacterManager.Instance.AllCharacterSetSpeedScale(SPEED_DOWN_SCALE);
    }

    private void InsertTresureEffect()
    {
        for (int i = 0; i< HideGenerators.Length; ++i)
        {
            HideGenerators[i].gameObject.SetActive(true);
        }
    }

    private void InvisibleEffect()
    {
        CharacterManager.Instance.AllCharacterSetVisible(false);
    }
}
