using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotate : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Axis whichAxis = Axis.Y_Axis;
    [SerializeField] private float delayBeforeStart = 0;
    [SerializeField] private float rotateSpeed = 50;

    private float _timer;
    private float _angle;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_timer >  delayBeforeStart)
        {
            _angle += rotateSpeed * Time.fixedDeltaTime;
            Vector3 vec = _transform.eulerAngles;
            switch (whichAxis)
            {
                case Axis.X_Axis:
                                        
                    _transform.localEulerAngles = new Vector3(
                        _angle,
                        vec.y,
                        vec.z);
                    break;

                case Axis.Y_Axis:
                    _transform.eulerAngles = new Vector3(
                        vec.x,
                        vec.y + rotateSpeed * Time.fixedDeltaTime,
                        vec.z);
                    break;

                case Axis.Z_Axis:
                    _transform.eulerAngles = new Vector3(
                        vec.x,
                        vec.y,
                        vec.z + rotateSpeed * Time.fixedDeltaTime);
                    break;
            }
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }
}

public enum Axis
{
    X_Axis,
    Y_Axis, 
    Z_Axis
}
