using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public static FPSController Instance { get; private set; }

    private List<float> fps = new List<float>();
    private float _timer;
    private float _preTimer = 2;

    private GameManager gm;

    private bool isCleared;

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

    private void Start()
    {
        gm = GameManager.Instance;
    }

    private void Update()
    {
        //if (!gm.IsGameStarted) return;
        if (_preTimer > 0)
        {
            _preTimer-=Time.deltaTime;
            return;
        }

        if (!isCleared)
        {
            isCleared = true;
            Resources.UnloadUnusedAssets();
        }

        if (_timer > 0.1f)
        {
            _timer = 0;
            if (EasyFpsCounter.EasyFps != null) fps.Add(EasyFpsCounter.EasyFps.FPS);
            if (fps.Count > 50)
            {
                fps.Remove(fps[0]);
            }

            if (!Globals.IsLowFPS)
            {
                float ave = GetAverage();

                if (fps.Count > 30 && ave > 5 && ave < 50)
                {
                    
                    Globals.IsLowFPS = true;

                    Globals.MainPlayerData.IsLowFPSOn = true;
                    SaveLoadManager.Save();

                    QualitySettings.shadows = ShadowQuality.Disable;
                    
                }
            }
            
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }
}
