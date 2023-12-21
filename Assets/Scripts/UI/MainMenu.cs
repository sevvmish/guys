using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class MainMenu : MonoBehaviour
{    
    [Header("Menu")]
    [SerializeField] private Button playB;
    [SerializeField] private TextMeshProUGUI playBText;
    

    [Header("Main Player positions")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform location;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ScreenSaver.Instance.ShowScreen();

        mainCamera.orthographicSize = 1.5f;
        mainCamera.transform.position = new Vector3(0, 0, -9);
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

        //main player template
        GameObject g = Instantiate(SkinControl.GetSkinGameobject((Skins)Globals.MainPlayerData.CS), location);
        g.transform.position = Globals.UIPlayerPosition;
        g.transform.eulerAngles = Globals.UIPlayerRotation;
    }

    private void startTheGame()
    {
        YandexGame.StickyAdActivity(true);
    }
    private IEnumerator playStart()
    {
        ScreenSaver.Instance.HideScreen();
        yield return new WaitForSeconds(Globals.SCREEN_SAVER_AWAIT + 0.2f);
        SceneManager.LoadScene("level1");
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

            Localize();
            playWhenInitialized();
            startTheGame();
        }
    }

    private void Localize()
    {
        Globals.Language = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();
        playBText.text = Globals.Language.Play;

    }
}
