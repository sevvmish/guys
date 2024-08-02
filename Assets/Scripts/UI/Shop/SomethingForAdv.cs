using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SomethingForAdv : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI mainDescription;
    [SerializeField] private Rewarded rewarded;
    [SerializeField] private Button getButton;

    [SerializeField] private GameObject goldPanel;
    [SerializeField] private GameObject xpPanel;

    [SerializeField] private GameObject offPanel;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private GameObject errorPanel;
    [SerializeField] private Button errorOK;
    [SerializeField] private TextMeshProUGUI advErrorText;
    [SerializeField] private TextMeshProUGUI closeErrorButtonText;

    private RewardForAdv currentReward;
    private readonly int GoldAmount = 150;
    private readonly int XPAmount = 100;
    private bool isReady;
    private bool isSet;
    private bool isSet2;
    private float _timer;

    private void Start()
    {
        errorPanel.SetActive(false);
        goldPanel.SetActive(false);
        xpPanel.SetActive(false);
        offPanel.SetActive(false);

        errorOK.onClick.AddListener(() =>
        {
            SoundUI.Instance.PlayUISound(SoundsUI.click);

            if (errorPanel.activeSelf) errorPanel.SetActive(false);
        });

        
    }

    public void SetData(RewardForAdv rew)
    {
        
        currentReward = rew;
        isSet = true;
    }
        

    private void Update()
    {
        if (offPanel.activeSelf)
        {
            if (_timer > 1)
            {
                _timer = 0;
                float timeToShow = (float)(DateTime.Now - Globals.TimeWhenLastRewardedWas).TotalSeconds;

                if (timeToShow >= Globals.REWARDED_COOLDOWN)
                {
                    offPanel.SetActive(false);
                    getButton.interactable = true;
                }
                else
                {
                    showTimerData(Globals.REWARDED_COOLDOWN - timeToShow);
                }
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }
        else
        {
            if (_timer > 1)
            {
                _timer = 0;

                if ((DateTime.Now - Globals.TimeWhenLastRewardedWas).TotalSeconds < Globals.REWARDED_COOLDOWN)
                {
                    offPanel.SetActive(true);
                    getButton.interactable = false;
                }
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }
        

        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;
            if ((DateTime.Now - Globals.TimeWhenLastRewardedWas).TotalSeconds >= Globals.REWARDED_COOLDOWN)
            {
                offPanel.SetActive(false);
                getButton.interactable = true;
            }
            else
            {
                offPanel.SetActive(true);
                getButton.interactable = false;
            }
        }


        if (isSet && isReady && !isSet2)
        {
            isSet2 = true;


            switch(currentReward)
            {
                case RewardForAdv.Gold:
                    title.text = Globals.Language.Gold;
                    mainDescription.text = Globals.Language.RewardForAdvGoldDescription;
                    goldPanel.SetActive(true);
                    break;

                case RewardForAdv.XP:
                    title.text = Globals.Language.XP;
                    mainDescription.text = Globals.Language.RewardForAdvXPDescription;
                    xpPanel.SetActive(true);
                    break;
            }

            getButton.onClick.AddListener(() =>
            {
                SoundUI.Instance.PlayUISound(SoundsUI.click);
                getButton.interactable = false;

                switch (currentReward)
                {
                    case RewardForAdv.Gold:
                        rewarded.OnRewardedEndedOK = giveGoldForAdv;
                        rewarded.OnError = showError;
                        rewarded.ShowRewardedVideo();
                        break;

                    case RewardForAdv.XP:
                        rewarded.OnRewardedEndedOK = giveXPForAdv;
                        rewarded.OnError = showError;
                        rewarded.ShowRewardedVideo();
                        break;
                }
            });

            advErrorText.text = Globals.Language.RewardedADVError;
            closeErrorButtonText.text = Globals.Language.Close;
        }
    }

    private void giveGoldForAdv()
    {
        Globals.MainPlayerData.G += GoldAmount;
        GetRewardSystem.Instance.ShowEffect(RewardTypes.gold, GoldAmount);
        SaveLoadManager.Save();
        if (MenuOptions.Instance != null) MenuOptions.Instance.UpdateCurrencyData();
    }

    private void giveXPForAdv()
    {
        Globals.AddXP(XPAmount);        
        SaveLoadManager.Save();
        if (MenuOptions.Instance != null) MenuOptions.Instance.UpdateCurrencyData();
    }

    private void showError()
    {
        SoundUI.Instance.PlayUISound(SoundsUI.error);
        errorPanel.SetActive(true);
        getButton.interactable = true;
    }
        

    private void showTimerData(float _time)
    {        
        int newTime = Mathf.RoundToInt(_time);
        string result = "";

        if (newTime < 60)
        {
            result = newTime.ToString();
        }
        else
        {
            int m = newTime / 60;
            int s = newTime % 60;

            string min = m < 10 ? "0" + m.ToString() : m.ToString();
            string sec = s < 10 ? "0" + s.ToString() : s.ToString();

            result = min + ":" + sec.ToString();
        }

        timerText.text = result;
    }
}

public enum RewardForAdv
{
    Gold,
    XP
}
