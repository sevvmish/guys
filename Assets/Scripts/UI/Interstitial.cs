using GamePush;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interstitial : MonoBehaviour
{
    public Action OnEnded;

    private void OnEnable()
    {
        GP_Ads.OnFullscreenStart += advStarted;
        GP_Ads.OnFullscreenClose += advClosed;
    }

    private void OnDisable()
    {
        GP_Ads.OnFullscreenStart -= advStarted;
        GP_Ads.OnFullscreenClose -= advClosed;
    }

    public void ShowInterstitialVideo()
    {
        //YandexGame.OpenFullAdEvent = advStarted;
        //YandexGame.CloseFullAdEvent = advClosed;
        //YandexGame.ErrorFullAdEvent = advError;
        //YandexGame.FullscreenShow();

        GP_Ads.ShowFullscreen();

        //advClosed(true);
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

    private void advClosed(bool isSuccess)
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
