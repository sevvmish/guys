using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingNonPhysics : MonoBehaviour
{
    [SerializeField] private Transform _rigidbody;
    [SerializeField] private Axes RotationAxis;
    [SerializeField] private float HowLongFor360 = 1;
    [SerializeField] private int direction = 1;
    [SerializeField] private float DelayBeforeStart;
    [SerializeField] private Ease easeType = Ease.Linear;

    // Start is called before the first frame update
    void Start()
    {

        Invoke("rotate", DelayBeforeStart);
    }

    private void rotate()
    {
        switch (RotationAxis)
        {
                        
                
                case Axes.axis_X:
                    _rigidbody.DOLocalRotate(new Vector3(
                        direction * 360,
                        0,
                        0), HowLongFor360, RotateMode.FastBeyond360).SetUpdate(UpdateType.Fixed). SetEase(easeType).SetLoops(-1);
                    break;

                case Axes.axis_Y:

                    _rigidbody.DOLocalRotate(new Vector3(
                        0,
                        direction * 360,
                        0), HowLongFor360, RotateMode.FastBeyond360).SetUpdate(UpdateType.Fixed).SetEase(easeType).SetLoops(-1);

                    break;

                case Axes.axis_Z:
                    _rigidbody.DOLocalRotate(new Vector3(
                        0,
                        0,
                        direction * 360), HowLongFor360, RotateMode.FastBeyond360).SetUpdate(UpdateType.Fixed).SetEase(easeType).SetLoops(-1);
                    break;
        }
    }
}
