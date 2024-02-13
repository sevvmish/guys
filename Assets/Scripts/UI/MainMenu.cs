using DG.Tweening;
using System;
using System.Linq;
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
    [SerializeField] private Button customizeB;
    [SerializeField] private TextMeshProUGUI customizeBText;
    [SerializeField] private Button shopB;
    [SerializeField] private TextMeshProUGUI shopBText;
    [SerializeField] private Button rewardsB;
    [SerializeField] private TextMeshProUGUI rewardsBText;


    [SerializeField] private MenuOptions menuOptions;
    [SerializeField] private ShopUI shop;
    [SerializeField] private CustomizeUI customize;


    [Header("Main Player positions")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform location;
    [SerializeField] private PointerBase pointer;


    [Header("Progress")]
    [SerializeField] private ProgressUI progressUI;
    [SerializeField] private Button progressB;
    [SerializeField] private TextMeshProUGUI progressBText;
    [SerializeField] private TextMeshProUGUI levelFromText;
    [SerializeField] private TextMeshProUGUI levelToText;
    [SerializeField] private TextMeshProUGUI XPText;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private GameObject notification;


    [Header("UIs")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject customizeUI;


    [SerializeField] private QuestsUI questsUI;
    [SerializeField] private GameObject questNotificator;

    [Header("Notificator arrows")]
    [SerializeField] private GameObject playArrowNotificator;

    [SerializeField] private Button resetB;
    [SerializeField] private Button level1B;
    [SerializeField] private Button level2B;
    [SerializeField] private Button level3B;
    [SerializeField] private Button level4B;
    [SerializeField] private Button level5B;
    [SerializeField] private Button level6B;
    [SerializeField] private Button level7B;
    [SerializeField] private Button level8B;
    [SerializeField] private Button level9B;
    [SerializeField] private Button level10B;
    [SerializeField] private Button level11B;
    [SerializeField] private Button level12B;
    [SerializeField] private Button level13B;


    
    public Action OnBackToMainMenu;
    public GameObject MainPlayerSkin;
    public Transform GetCameraTransform => mainCamera.transform;

    private bool isToUpdate;

    private static int[] levelXP = new int[] {0,
        0, //1 lvl
        100, //2 lvl
        220,
        370,
        520,
        720,
        970,
        1220,
        1470,
        1770, //10 lvl
        2070,
        2370,
        2670,
        3020,
        3370,
        3720,
        4070,
        4470,
        4870,
        10000,
        20000,
        30000
    };

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        mainCamera.orthographicSize = 1.5f;
        mainCamera.transform.position = new Vector3(0, 0, -9);
    }

    private void showProgress()
    {        
        if (!Globals.MainPlayerData.AllMaps) notification.SetActive(Globals.MainPlayerData.XPN);
        int lvl = GetLevelByXP(Globals.MainPlayerData.XP);
        levelFromText.text = lvl.ToString();
        int nextLvl = lvl + 1;
        levelToText.text = nextLvl.ToString();

        int prevXP = 0;

        if (lvl > 1)
        {
            int prevLVL = lvl;
            prevXP = GetMaxXPByLVL(prevLVL);
        }

        XPText.text = (Globals.MainPlayerData.XP - prevXP) + "/" + (GetMaxXPByLVL(nextLvl) - prevXP);
        progressSlider.value = (float)(Globals.MainPlayerData.XP-prevXP) / (GetMaxXPByLVL(nextLvl) - prevXP);
    }


    private void Start()
    {
        mainMenuUI.SetActive(true);
        shopUI.SetActive(false);
        customizeUI.SetActive(false);
        notification.SetActive(false);
        playArrowNotificator.SetActive(false);
        questNotificator.SetActive(false);

        resetB.gameObject.SetActive(true);
        level1B.gameObject.SetActive(true);
        level2B.gameObject.SetActive(true);
        level3B.gameObject.SetActive(true);
        level4B.gameObject.SetActive(true);
        level5B.gameObject.SetActive(true);
        level6B.gameObject.SetActive(true);
        level7B.gameObject.SetActive(true);
        level8B.gameObject.SetActive(true);
        level9B.gameObject.SetActive(true);
        level10B.gameObject.SetActive(true);
        level11B.gameObject.SetActive(true);
        level12B.gameObject.SetActive(true);
        level13B.gameObject.SetActive(true);

        level1B.onClick.AddListener(() => { SceneManager.LoadScene("level1"); });
        level2B.onClick.AddListener(() => { SceneManager.LoadScene("level2"); });
        level3B.onClick.AddListener(() => { SceneManager.LoadScene("level3"); });
        level4B.onClick.AddListener(() => { SceneManager.LoadScene("level4"); });
        level5B.onClick.AddListener(() => { SceneManager.LoadScene("level5"); });
        level6B.onClick.AddListener(() => { SceneManager.LoadScene("level6"); });
        level7B.onClick.AddListener(() => { SceneManager.LoadScene("level7"); });
        level8B.onClick.AddListener(() => { SceneManager.LoadScene("level8"); });
        level9B.onClick.AddListener(() => { SceneManager.LoadScene("level9"); });
        level10B.onClick.AddListener(() => { SceneManager.LoadScene("level10"); });
        level11B.onClick.AddListener(() => { SceneManager.LoadScene("level11"); });
        level12B.onClick.AddListener(() => { SceneManager.LoadScene("level12"); });
        level13B.onClick.AddListener(() => { SceneManager.LoadScene("level13"); });

        playB.onClick.AddListener(() =>
        {
            playB.interactable = false;
            SoundUI.Instance.PlayUISound(SoundsUI.positive);
            StartCoroutine(playStart());
        });

        customizeB.onClick.AddListener(() =>
        {
            customizeB.interactable = false;
            SoundUI.Instance.PlayUISound(SoundsUI.positive);
            

            menuOptions.SetBackButtonSign(Globals.Language.CustomizeButton);

            mainMenuUI.SetActive(false);
            shopUI.SetActive(false);
            customizeUI.SetActive(true);
            customize.SetOn();
        });

        shopB.onClick.AddListener(() =>
        {            
            shopB.interactable = false;
            SoundUI.Instance.PlayUISound(SoundsUI.positive);

            menuOptions.SetBackButtonSign(Globals.Language.ShopButton);

            mainMenuUI.SetActive(false);
            shopUI.SetActive(true);
            customizeUI.SetActive(false);
            shop.SetOn();
        });

        rewardsB.onClick.AddListener(() =>
        {
            rewardsB.interactable = false;
            SoundUI.Instance.PlayUISound(SoundsUI.positive);

            menuOptions.SetBackButtonSign(Globals.Language.QuestButton);

            mainMenuUI.SetActive(false);
            shopUI.SetActive(false);
            customizeUI.SetActive(false);
            questsUI.SetOn();            
        });

        resetB.onClick.AddListener(() =>
        {
            resetB.interactable = false;
            SoundUI.Instance.PlayUISound(SoundsUI.positive);

            Globals.MainPlayerData = new PlayerData();
            SaveLoadManager.Save();

            SceneManager.LoadScene("MainMenu");
        });

        progressB.onClick.AddListener(() =>
        {
            progressB.interactable = false;
            SoundUI.Instance.PlayUISound(SoundsUI.positive);

            menuOptions.SetBackButtonSign(Globals.Language.ProgressButton);

            mainMenuUI.SetActive(false);
            shopUI.SetActive(false);
            customizeUI.SetActive(false);
            progressUI.SetOn();
        });

        if (Globals.IsInitiated)
        {
            playWhenInitialized();
            Localize();
        }        
    }

    public void BackToMainMenu()
    {
        if (isToUpdate)
        {
            Globals.IsDontShowIntro = true;
            SceneManager.LoadScene("MainMenu");            
        }
            
        pointer.gameObject.SetActive(true);
        mainMenuUI.SetActive(true);
        shopUI.SetActive(false);
        customizeUI.SetActive(false);

        playB.interactable = true;
        customizeB.interactable = true;
        shopB.interactable = true;
        progressB.interactable = true;
        rewardsB.interactable = true;

        MainPlayerSkin.SetActive(true);
        MainPlayerSkin.transform.DOMove(Globals.UIPlayerPosition, 0.3f).SetEase(Ease.Linear);
        MainPlayerSkin.transform.DORotate(Globals.UIPlayerRotation, 0.3f).SetEase(Ease.Linear);
        GetCameraTransform.DOMove(new Vector3(0, 0, -9), 0.3f).SetEase(Ease.Linear);

        showProgress();
    }
    
    public void UpdateCurrencies()
    {
        menuOptions.UpdateCurrencyData();
    }
    public void UpdateXP()
    {
        showProgress();
    }

    private void resetAnalytics()
    {
        if (Globals.MainPlayerData.LDA == 0 || Mathf.Abs(DateTime.Now.Day - Globals.MainPlayerData.LDA) > 0)
        {
            Globals.MainPlayerData.LDA = DateTime.Now.Day;
            Globals.MainPlayerData.WR = new GameSessionResult[0];

            for (int i = 0; i < Globals.MainPlayerData.QRT.Length; i++)
            {
                Globals.MainPlayerData.QRT[i] = 0;
            }
         
            SaveLoadManager.Save();
        }
    }

    private void playWhenInitialized()
    {        
        resetAnalytics();

        if (!Globals.MainPlayerData.TutL)
        {            
            SceneManager.LoadScene("tutorial");
            return;
        }
        

        if (Globals.IsDontShowIntro)
        {
            ScreenSaver.Instance.FastShowScreen();
        }
        else
        {
            ScreenSaver.Instance.ShowScreen();
        }

        
        YandexGame.StickyAdActivity(!Globals.MainPlayerData.AdvOff);
        showProgress();

        if (Globals.IsMusicOn)
        {
            int ambMusic = UnityEngine.Random.Range(0, 3);
            switch (ambMusic)
            {
                case 0:
                    AmbientMusic.Instance.PlayAmbient(AmbientMelodies.loop_melody3);
                    break;

                case 1:
                    AmbientMusic.Instance.PlayAmbient(AmbientMelodies.loop_melody4);
                    break;

                case 2:
                    AmbientMusic.Instance.PlayAmbient(AmbientMelodies.loop_melody5);
                    break;
            }
        }
        

        //main player template
        if (MainPlayerSkin == null)
        {
            MainPlayerSkin = SkinControl.GetSkinGameobject((Skins)Globals.MainPlayerData.CS);
            MainPlayerSkin.transform.parent = location;
            MainPlayerSkin.GetComponent<Animator>().Play("IdlePlus");
        }
            
        MainPlayerSkin.transform.position = Globals.UIPlayerPosition;
        MainPlayerSkin.transform.eulerAngles = Globals.UIPlayerRotation;
    }

    public void ChangeMainSkin(bool isToUpdate)
    {
        this.isToUpdate = isToUpdate;
        Globals.IsDontShowIntro = true;

        Vector3 pos = MainPlayerSkin.transform.position;
        Vector3 rot = MainPlayerSkin.transform.eulerAngles;
        Vector3 scale = MainPlayerSkin.transform.localScale;

        Destroy(MainPlayerSkin);

        MainPlayerSkin = SkinControl.GetSkinGameobject((Skins)Globals.MainPlayerData.CS);
        MainPlayerSkin.transform.parent = location;

        MainPlayerSkin.transform.position = pos;
        MainPlayerSkin.transform.eulerAngles = rot;
        MainPlayerSkin.transform.localScale = scale;
    }

    
    private IEnumerator playStart()
    {
        ScreenSaver.Instance.HideScreen();
        yield return new WaitForSeconds(Globals.SCREEN_SAVER_AWAIT + 0.2f);
        SceneManager.LoadScene("LevelSetter");
    }


    private void rotateCharacters(Vector2 delta)
    {
        float speed = 5f;
        float civilY = 0;

        if (delta.x < 0)
        {
            civilY = MainPlayerSkin.transform.eulerAngles.y + speed;
        }
        else
        {
            civilY = MainPlayerSkin.transform.eulerAngles.y - speed;
        }

        MainPlayerSkin.transform.eulerAngles = new Vector3(MainPlayerSkin.transform.eulerAngles.x, civilY, MainPlayerSkin.transform.eulerAngles.z);
    }


    private void Update()
    {
        if (Globals.IsShowQuestNotification && !questNotificator.activeSelf)
        {
            questNotificator.SetActive(true);
        }
        else if (!Globals.IsShowQuestNotification && questNotificator.activeSelf)
        {
            questNotificator.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            Globals.MainPlayerData = new PlayerData();
            SaveLoadManager.Save();

            SceneManager.LoadScene("MainMenu");
        }

        Vector2 delta = pointer.DeltaPosition;
        if (delta.x != 0) rotateCharacters(delta);

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
                Globals.TimeWhenLastRewardedWas = DateTime.Now.Subtract(new TimeSpan(1,0,0));
            }

            Localize();
            playWhenInitialized();
        }
    }

    public static int GetLevelByXP(int xp)
    {
        if (xp < levelXP[2]) return 1;
        if (xp >= levelXP[levelXP.Length - 1]) return levelXP.Length;
        int result = 1;

        for (int i = 0; i < levelXP.Length; i++)
        {
            if (xp == levelXP[i] || ((i+1) < levelXP.Length && xp > levelXP[i] && xp < levelXP[i+1]))
            {
                return i;
            }
        }

        return result;
    }

    public static int GetCurrentLevel()
    {        
        return GetLevelByXP(Globals.MainPlayerData.XP);
    }

    public static int GetMaxXPByLVL(int lvl)
    {
        return levelXP[lvl];
    }

    private void Localize()
    {
        Globals.Language = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();
        playBText.text = Globals.Language.Play;

        customizeBText.text = Globals.Language.CustomizeButton;
        shopBText.text = Globals.Language.ShopButton;
        rewardsBText.text = Globals.Language.RewardButton;
        progressBText.text = Globals.Language.Progress;
    }
}
