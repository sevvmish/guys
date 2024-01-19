using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumPhysicsByAngle : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Axes RotationAxis;
    [SerializeField] private float HowLongFor360 = 2;
    [SerializeField] private float PendulumAngle;
    [SerializeField] private float DelayBeforeStart;
    [SerializeField] private Ease easeType = Ease.InOutSine;

    float delta = 0;

    private void OnEnable()
    {                
        StartCoroutine(playEffect());
    }

    private IEnumerator playEffect()
    {
        yield return new WaitForSeconds(DelayBeforeStart);

        switch (RotationAxis)
        {
            case Axes.axis_X:
                delta = PendulumAngle * 2 / (HowLongFor360 / Time.fixedDeltaTime);

                while (true)
                {
                    for (float i = PendulumAngle; i > -PendulumAngle; i -= delta)
                    {
                        float koeff = 1f - Mathf.Abs(i / PendulumAngle);

                        if (koeff != 0)
                        {
                            i -= (koeff * 3);
                        }

                        _rigidbody.MoveRotation(Quaternion.AngleAxis(i, Vector3.left));
                        yield return new WaitForSeconds(Time.fixedDeltaTime);
                    }

                    for (float i = -PendulumAngle; i < PendulumAngle; i += delta)
                    {
                        float koeff = 1f - Mathf.Abs(i / PendulumAngle);

                        if (koeff != 0)
                        {
                            i += (koeff * 3);
                        }

                        _rigidbody.MoveRotation(Quaternion.AngleAxis(i, Vector3.left));
                        yield return new WaitForSeconds(Time.fixedDeltaTime);
                    }


                }
                

            case Axes.axis_Y:
                
                break;

            case Axes.axis_Z:

                delta = PendulumAngle * 2 / (HowLongFor360 / Time.fixedDeltaTime);

                while (true)
                {
                    for (float i = PendulumAngle; i > -PendulumAngle; i -= delta)
                    {
                        float koeff = 1f - Mathf.Abs(i / PendulumAngle);
                        
                        if (koeff != 0)
                        {
                            i -= (koeff * 3);
                        }                    

                        _rigidbody.MoveRotation(Quaternion.AngleAxis(i, Vector3.forward));
                        yield return new WaitForSeconds(Time.fixedDeltaTime);
                    }

                    for (float i = -PendulumAngle; i < PendulumAngle; i += delta)
                    {
                        float koeff = 1f - Mathf.Abs(i / PendulumAngle);

                        if (koeff != 0)
                        {
                            i += (koeff * 3);
                        }

                        _rigidbody.MoveRotation(Quaternion.AngleAxis(i, Vector3.forward));
                        yield return new WaitForSeconds(Time.fixedDeltaTime);
                    }

                    
                }
        }
        
    }



    /*
    private void rotate()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.SetLoops(-1, LoopType.Restart);
        sequence.SetUpdate(UpdateType.Fixed);

        switch (RotationAxis)
        {
            case Axes.axis_X:
                _rigidbody.transform.DOLocalRotate(new Vector3(180 - PendulumAngle, 0, 0), 0);


                sequence.Append(
                    _rigidbody.transform.DOLocalRotate(new Vector3(
                    180 + PendulumAngle,
                    0,
                    0), HowLongFor360).SetEase(easeType)
                    );

                sequence.Append(
                    _rigidbody.transform.DOLocalRotate(new Vector3(
                    180 - PendulumAngle,
                    0,
                    0), HowLongFor360).SetEase(easeType)
                    );
                break;

            case Axes.axis_Y:
                _rigidbody.transform.DOLocalRotate(new Vector3(0, 180 - PendulumAngle, 0), 0);


                sequence.Append(
                    _rigidbody.transform.DOLocalRotate(new Vector3(
                    0,
                    180 + PendulumAngle,
                    0), HowLongFor360).SetEase(easeType)
                    );

                sequence.Append(
                    _rigidbody.transform.DOLocalRotate(new Vector3(
                    0,
                    180 - PendulumAngle,
                    0), HowLongFor360).SetEase(easeType)
                    );

                break;

            case Axes.axis_Z:
                //_rigidbody.DORotate(new Vector3(0, 0, 180-PendulumAngle), 0);
                //Vector3 from
                //_rigidbody.transform.localEulerAngles = new Vector3

                sequence.Append(
                    _rigidbody.DORotate(new Vector3(
                    0,
                    0,
                    45), 0, RotateMode.Fast).SetEase(easeType)
                    );


                sequence.Append(
                    _rigidbody.DORotate(new Vector3(
                    0,
                    0,
                    315), HowLongFor360, RotateMode.Fast).SetEase(easeType)
                    );

                sequence.Append(
                    _rigidbody.DORotate(new Vector3(
                    0,
                    0,
                    45), HowLongFor360, RotateMode.Fast).SetEase(easeType)
                    );

              

                break;
        }
    }*/
}
