using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearWithScaleAnimation : MonoBehaviour
{
    [SerializeField] private float x = 1;
    [SerializeField] private float y = 1;
    [SerializeField] private float _time = 0.3f;
    [SerializeField] private Ease ease = Ease.InOutSine;

    private void OnEnable()
    {
        transform.DOShakeScale(_time, 1, 10).SetEase(ease);
        SoundUI.Instance.PlayUISound(SoundsUI.positive);
    }
}
