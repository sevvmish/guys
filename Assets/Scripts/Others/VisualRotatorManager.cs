using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualRotatorManager : MonoBehaviour
{
    public static VisualRotatorManager Instance { get; private set; }

    private List<VisualRotatorData> data = new List<VisualRotatorData>();

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddData(VisualRotatorData _data)
    {
        if (!data.Contains(_data)) data.Add(_data);
    }

    public void RemoveData(VisualRotatorData _data)
    {
        if (data.Contains(_data)) data.Remove(_data);
    }

    private void Update()
    {
        if (data.Count > 0)
        {
            for (int i = 0; i < data.Count; i++)
            {
                switch(data[i].Ax)
                {
                    case Axis.X_Axis:
                        data[i].CurrentTransform.Rotate(Vector3.left, data[i].Speed * Time.deltaTime);

                        /*
                        data[i].CurrentTransform.localEulerAngles = new Vector3(
                            data[i].CurrentTransform.localEulerAngles.x + data[i].Speed * Time.deltaTime, 
                            data[i].CurrentTransform.localEulerAngles.y, 
                            data[i].CurrentTransform.localEulerAngles.z);*/
                        break;

                    case Axis.Y_Axis:
                        data[i].CurrentTransform.Rotate(Vector3.up, data[i].Speed * Time.deltaTime);

                        /*
                        data[i].CurrentTransform.localEulerAngles = new Vector3(
                            data[i].CurrentTransform.localEulerAngles.x,
                            data[i].CurrentTransform.localEulerAngles.y + data[i].Speed * Time.deltaTime,
                            data[i].CurrentTransform.localEulerAngles.z);*/
                        break;

                    case Axis.Z_Axis:
                        data[i].CurrentTransform.Rotate(Vector3.forward, data[i].Speed * Time.deltaTime);

                        /*
                        data[i].CurrentTransform.localEulerAngles = new Vector3(
                            data[i].CurrentTransform.localEulerAngles.x,
                            data[i].CurrentTransform.localEulerAngles.y,
                            data[i].CurrentTransform.localEulerAngles.z + data[i].Speed * Time.deltaTime);*/
                        break;
                }
            }

        }
    }
}

public struct VisualRotatorData
{
    public Axis Ax;
    public Vector3 CurrentRotation;
    public float Speed;
    public Transform CurrentTransform;

    public VisualRotatorData(Axis ax, Vector3 currentRotation, float speed, Transform _transform)
    {
        Ax = ax;
        CurrentRotation = currentRotation;
        Speed = speed;
        CurrentTransform = _transform;
    }
}
