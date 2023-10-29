using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private float delayBeforeStart = 0;
    [SerializeField] private float delayBetweenPlatforms = 1;
    [SerializeField] private float movementSpeed = 2;
    [SerializeField] private Transform FromPoint, ToPoint;
    [SerializeField] private Ease easeType = Ease.Linear;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("Init", delayBeforeStart);
    }


    private void Init()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.SetUpdate(UpdateType.Fixed);
        sequence.SetLoops(-1, LoopType.Restart);


        _transform.position = FromPoint.position;
        sequence.Append(_transform.DOMove(ToPoint.position, movementSpeed).SetEase(easeType));
        sequence.AppendInterval(delayBetweenPlatforms);
        sequence.Append(_transform.DOMove(FromPoint.position, movementSpeed).SetEase(easeType));
        sequence.AppendInterval(delayBetweenPlatforms);
    }
}
