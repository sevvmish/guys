using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private ScreenSaver screenSaver;

    [Header("Menu")]
    [SerializeField] private Button playB;

    private void Awake()
    {        
        screenSaver.ShowScreen();        
    }


    private void Start()
    {
        playB.onClick.AddListener(() =>
        {
            playB.interactable = false;
            SoundUI.Instance.PlayUISound(SoundsUI.positive);
            StartCoroutine(playStart());
        });

        if (Globals.IsInitiated)
        {
            playWhenInitialized();
            Localize();
            startTheGame();
        }
    }

    private void playWhenInitialized()
    {
        AmbientMusic.Instance.PlayAmbient(AmbientMelodies.forest);
    }

    private void startTheGame()
    {
        YandexGame.StickyAdActivity(true);
        //StartCoroutine(playStart());
    }
    private IEnumerator playStart()
    {
        screenSaver.HideScreen();
        yield return new WaitForSeconds(Globals.SCREEN_SAVER_AWAIT + 0.2f);
        SceneManager.LoadScene("circus1");
    }

    private void Update()
    {
        if (YandexGame.SDKEnabled && !Globals.IsInitiated)
        {
            Globals.IsInitiated = true;

            SaveLoadManager.Load();

            print("SDK enabled: " + YandexGame.SDKEnabled);
            Globals.CurrentLanguage = YandexGame.EnvironmentData.language;
            print("language set to: " + Globals.CurrentLanguage);

            Globals.IsMobile = YandexGame.EnvironmentData.isMobile;
            print("platform mobile: " + Globals.IsMobile);

            if (Globals.MainPlayerData.S == 1)
            {
                Globals.IsSoundOn = true;
                AudioListener.volume = 1;
            }
            else
            {
                Globals.IsSoundOn = false;
                AudioListener.volume = 0;
            }

            if (Globals.MainPlayerData.Mus == 1)
            {
                Globals.IsMusicOn = true;
            }
            else
            {
                Globals.IsMusicOn = false;
            }

            print("sound is: " + Globals.IsSoundOn);

            if (Globals.TimeWhenStartedPlaying == DateTime.MinValue)
            {
                Globals.TimeWhenStartedPlaying = DateTime.Now;
                Globals.TimeWhenLastInterstitialWas = DateTime.Now;
                Globals.TimeWhenLastRewardedWas = DateTime.Now;
            }

            //Globals.CurrentMapCircus = Globals.MainPlayerData.CM;

            Localize();
            playWhenInitialized();
            startTheGame();
        }
    }

    private void Localize()
    {
        Globals.Language = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();        
    }
}
