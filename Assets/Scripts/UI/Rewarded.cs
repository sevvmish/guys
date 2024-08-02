using GamePush;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewarded : MonoBehaviour
{
    public Action OnRewardedEndedOK;
    public Action OnError;


    private void OnEnable()
    {
        GP_Ads.OnRewardedStart += rewardStarted;
        GP_Ads.OnRewardedClose += advRewardedClosed;
        GP_Ads.OnRewardedReward += rewardedReward;
        
    }

    private void OnDisable()
    {
        GP_Ads.OnRewardedStart -= rewardStarted;
        GP_Ads.OnRewardedClose -= advRewardedClosed;
        GP_Ads.OnRewardedReward -= rewardedReward;
    }

    public void ShowRewardedVideo()
    {
        if (GP_Ads.IsRewardedAvailable())
        {
            GP_Ads.ShowRewarded("reward");
        }
        else
        {
            OnError?.Invoke();
            OnRewardedEndedOK = null;
            OnError = null;
        }        
    }

    private void rewardStarted()
    {
        print("reward started OK");
        Time.timeScale = 0;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 0;
        }
    }

    private void rewardedReward(string value)
    {        
        if (value == "reward")
        {            
            print("reward given");
            OnRewardedEndedOK?.Invoke();
            OnRewardedEndedOK = null;
            OnError = null;
        }

        Globals.TimeWhenLastRewardedWas = DateTime.Now;
    }

    private void advRewardedClosed(bool isOK)
    {
        print("rewarded was closed ok");
        Time.timeScale = 1;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 1;
        }


        if (isOK)
        {
            print("close was OK");
        }
        else
        {
            print("close was NOT OK");

            OnError?.Invoke();
            OnRewardedEndedOK = null;
            OnError = null;
        }
                
        

    }

    /*
    private void advRewardedError()
    {
        print("rewarded was ERROR!");
        Time.timeScale = 1;
        if (Globals.IsSoundOn)
        {
            AudioListener.volume = 1;
        }

        OnError?.Invoke();
        OnRewardedEndedOK = null;
        OnError = null;

    }*/
}
