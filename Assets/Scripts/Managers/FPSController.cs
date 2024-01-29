using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public static FPSController Instance { get; private set; }

    private List<float> fps = new List<float>();
    private float _timer;

    public float GetAverage()
    {
        float result = 0;

        if (fps.Count < 10) return 0;

        result = fps.Average();

        return result;
    }

    private void Awake()
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

    private void Update()
    {
        if (_timer > 0.5f)
        {
            _timer = 0;
            if (EasyFpsCounter.EasyFps != null) fps.Add(EasyFpsCounter.EasyFps.FPS);
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }
}
