using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightUIEffect : MonoBehaviour
{
    public float delta = 50f;
    public float speed = 0.5f;

    private RectTransform rect;
    private Vector2 initPlace;

  

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        initPlace = rect.anchoredPosition;
    }

    private void OnEnable()
    {
        StartCoroutine(playStart());
    }

    private void OnDisable()
    {
        StopCoroutine(playStart());
    }

    private IEnumerator playStart()
    {
        while (true)
        {
            rect.DOAnchorPos(new Vector2(initPlace.x + delta, initPlace.y), speed).SetEase(Ease.OutSine);
            yield return new WaitForSeconds(speed);
            rect.DOAnchorPos(initPlace, speed).SetEase(Ease.OutSine);
            yield return new WaitForSeconds(speed);
        }        
    }


}
