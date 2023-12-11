using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private ScreenSaver screenSaver;

    private void Awake()
    {
        if (Globals.IsInitiated)
        {
            Localize();
            startTheGame();
        }
    }

    private void Start()
    {
        AmbientMusic.Instance.PlayAmbient(AmbientMelodies.forest);
    }

    private void startTheGame()
    {
        YandexGame.StickyAdActivity(true);
        StartCoroutine(playStart());
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
            startTheGame();
        }
    }

    private void Localize()
    {
        Globals.Language = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();        
    }
}
