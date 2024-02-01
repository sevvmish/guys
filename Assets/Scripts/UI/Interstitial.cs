using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class Interstitial : MonoBehaviour
{
    public Action OnEnded;

    public void ShowInterstitialVideo()
    {
        YandexGame.OpenFullAdEvent = advStarted;
        YandexGame.CloseFullAdEvent = advClosed;//nextLevelAction;
        YandexGame.ErrorFullAdEvent = advError;//nextLevelAction;
        YandexGame.FullscreenShow();
    }

    private void advStarted()
    {        
        Time.timeScale = 0;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 0;
        }
    }

    private void advError()
    {        
        Time.timeScale = 1;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 1;
        }
        OnEnded?.Invoke();
        OnEnded = null;
    }

    private void advClosed()
    {        
        Time.timeScale = 1;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 1;
        }

        Globals.TimeWhenLastInterstitialWas = DateTime.Now;

        OnEnded?.Invoke();        
    }
}
