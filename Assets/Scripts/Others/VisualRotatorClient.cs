using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualRotatorClient : MonoBehaviour
{
    public float Speed = 50;
    public Axis AX = Axis.X_Axis;

    // Start is called before the first frame update
    void Start()
    {        
        VisualRotatorManager.Instance.AddData(new VisualRotatorData(AX, transform.localEulerAngles, Speed, transform));
    }

}
