using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interstitial : MonoBehaviour
{
    public Action OnEnded;

    public void ShowInterstitialVideo()
    {
        //YandexGame.OpenFullAdEvent = advStarted;
        //YandexGame.CloseFullAdEvent = advClosed;
        //YandexGame.ErrorFullAdEvent = advError;
        //YandexGame.FullscreenShow();
        advClosed();
    }

    private void advStarted()
    {
        
        Time.timeScale = 0;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 0;
        }

        print("interstitial started");
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

        print("interstitial error");
    }

    private void advClosed()
    {        
        Time.timeScale = 1;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 1;
        }

        Globals.TimeWhenLastInterstitialWas = DateTime.Now;

        print("interstitial OK");

        OnEnded?.Invoke();
        OnEnded = null;
    }
}
