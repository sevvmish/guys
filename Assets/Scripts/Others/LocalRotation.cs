using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalRotation : MonoBehaviour
{
    [SerializeField] private Transform[] transforms;
    [SerializeField] private float speed = 50;
    [SerializeField] private Axis axis = Axis.X_Axis;

    private float deltaAngle;

    private void Start()
    {
        if (transforms.Length == 0) gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        deltaAngle = Time.deltaTime * speed;
        switch(axis)
        {
            case Axis.X_Axis:
                for (int i = 0; i < transforms.Length; i++)
                {
                    
                }
                break;
        }
    }
}
