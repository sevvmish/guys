using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using UnityEngine.SceneManagement;
using YG;

public class UIManager : MonoBehaviour
{
    [Header("mobile buttons")]
    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject jump;

    [Header("letters")]
    [SerializeField] private GameObject lettersHelper;
    [SerializeField] private GameObject mouseHelper;
    [SerializeField] private TextMeshProUGUI letterUp;
    [SerializeField] private TextMeshProUGUI letterDown;
    [SerializeField] private TextMeshProUGUI letterLeft;
    [SerializeField] private TextMeshProUGUI letterRight;
    [SerializeField] private TextMeshProUGUI signJump;
        
    [Header("informer")]
    [SerializeField] private GameObject informerPanel;
    [SerializeField] private TextMeshProUGUI informerText;

    [Header("timer")]
    [SerializeField] private GameObject timerPanel;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("mobile scaler")]
    [SerializeField] private GameObject scalerPanel;
    [SerializeField] private Button scalerPanelCallButton;
    [SerializeField] private Button scalerPanelCloseButton;
    [SerializeField] private Slider scalerSlider;
    [SerializeField] private TextMeshProUGUI scalerInfoText;

    [Header("Aims")]
    [SerializeField] private GameObject aimBeforeStart;
    [SerializeField] private TextMeshProUGUI aimBeforeStartText;
    [SerializeField] private GameObject aimDuringGame;
    [SerializeField] private TextMeshProUGUI aimDuringGameText;
    [SerializeField] private GameObject endGameWin;
    [SerializeField] private TextMeshProUGUI endGameWinText;
    [SerializeField] private GameObject endGameLose;
    [SerializeField] private TextMeshProUGUI endGameLoseText;

    [Header("ADV")]
    [SerializeField] private Rewarded rewarded;

    [Header("result reward")]
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private GameObject goldPanel;
    [SerializeField] private GameObject xpPanel;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button repeatButton;
    [SerializeField] private TextMeshProUGUI continueButtonText;
    [SerializeField] private TextMeshProUGUI mainMenuButtonText;
    [SerializeField] private TextMeshProUGUI repeatButtonText;


    [Header("ability button")]
    [SerializeField] private GameObject abilityButtonPanel;    
    [SerializeField] private Image abilityButtonImage;
    [SerializeField] private Vector2 placeWhenMobile;
    [SerializeField] private Vector2 placeWhenPC;
    [SerializeField] private Vector2 jumpStandartPlace = Vector2.zero;
    [SerializeField] private Vector2 jumpAbilityPlace;
    [SerializeField] private TextMeshProUGUI abilityButtonTextPC;
    [SerializeField] private Image circleForAbilityTimer;


    [Header("ability sprites")]
    [SerializeField] private Sprite accelerationSprite;
    [SerializeField] private Sprite rocketPackSprite;


    [Header("track record")]
    [SerializeField] private GameObject trackRecordPanel;
    [SerializeField] private GameObject percentPanel;
    [SerializeField] private TextMeshProUGUI trackRecordText;
    [SerializeField] private TextMeshProUGUI recordDataText;
    [SerializeField] private TextMeshProUGUI recordPercentText;


    [Header("ADV")]
    [SerializeField] private Interstitial interstitial;
    private string whatLevelToLoadAfterAdv;


    private GameManager gm;
    private LevelData levelData;
    private AbilityManager mainPlayerAbilityManager;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        Globals.IsLevelChangeStarted = false;
        //jumpStandartPlace = jump.GetComponent<RectTransform>().anchoredPosition;
        circleForAbilityTimer = abilityButtonPanel.transform.GetChild(0).GetComponent<Image>();
        jump.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        interstitial.OnEnded = null;

        HideAllControls();

        aimBeforeStart.SetActive(false);
        aimDuringGame.SetActive(false);
        timerPanel.SetActive(false);
        endGameWin.SetActive(false);
        endGameLose.SetActive(false);
        rewardPanel.SetActive(false);
        trackRecordPanel.SetActive(false);
        percentPanel.SetActive(false);

        continueButton.gameObject.SetActive(false);
        repeatButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);

        levelData = LevelManager.GetLevelData(gm.GetLevelManager().GetCurrentLevelType());

        aimBeforeStart.SetActive(true);
        aimBeforeStartText.text = Globals.Language.Aim + ": " + levelData.LevelAim + "!";
                
        scalerSlider.value = Globals.MainPlayerData.Zoom;

        if (Globals.Language != null)
        {
            letterUp.text = Globals.Language.UpArrowLetter;
            letterDown.text = Globals.Language.DownArrowLetter;
            letterLeft.text = Globals.Language.LeftArrowLetter;
            letterRight.text = Globals.Language.RightArrowLetter;
            signJump.text = Globals.Language.JumpLetter;
            abilityButtonTextPC.text = Globals.Language.PressForAbilityButton;
            scalerInfoText.text = Globals.Language.CameraScalerInfo;
        }

        scalerPanelCallButton.onClick.AddListener(() =>
        {
            if (scalerPanel.activeSelf) return;

            SoundUI.Instance.PlayUISound(SoundsUI.click);
            scalerPanel.SetActive(true);
            scalerPanelCallButton.gameObject.SetActive(false);
        });

        scalerPanelCloseButton.onClick.AddListener(() =>
        {
            if (!scalerPanel.activeSelf) return;
            
            SoundUI.Instance.PlayUISound(SoundsUI.click);
            scalerPanel.SetActive(false);
            scalerPanelCallButton.gameObject.SetActive(Globals.IsMobile);
        });

        scalerSlider.onValueChanged.AddListener(scaleCameraDistance);

        continueButton.onClick.AddListener(() =>
        {            
            SoundUI.Instance.PlayUISound(SoundsUI.positive);
            continueButton.interactable = false;
            repeatButton.interactable = false;
            mainMenuButton.interactable = false;

            if (levelData.GameType == GameTypes.Tutorial)
            {
                StartCoroutine(playStartLevel("level1"));
            }
            else
            {
                StartCoroutine(playStartLevel("LevelSetter"));
            }
        });

        repeatButton.onClick.AddListener(() =>
        {
            SoundUI.Instance.PlayUISound(SoundsUI.positive);
            continueButton.interactable = false;
            repeatButton.interactable = false;
            mainMenuButton.interactable = false;
            StartCoroutine(playStartLevel(levelData.LevelInInspector));
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            SoundUI.Instance.PlayUISound(SoundsUI.positive);
            continueButton.interactable = false;
            repeatButton.interactable = false;
            mainMenuButton.interactable = false;
            StartCoroutine(playStartLevel("MainMenu"));
        });
    }
    private IEnumerator hideAimAfterSec(float sec)
    {        
        yield return new WaitForSeconds(sec);
        aimBeforeStart.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            HideAllControls();
            aimDuringGame.SetActive(false);
        }
    }


    private IEnumerator playStartLevel(string level)
    {
        Globals.IsLevelChangeStarted = true;
        ScreenSaver.Instance.HideScreen();
        AmbientMusic.Instance.StopAll();
        yield return new WaitForSeconds(Globals.SCREEN_SAVER_AWAIT + 0.2f);

        //print("seconds: " + (DateTime.Now - Globals.TimeWhenLastInterstitialWas).TotalSeconds);

        if (!Globals.MainPlayerData.AdvOff && (DateTime.Now - Globals.TimeWhenLastInterstitialWas).TotalSeconds >= Globals.INTERSTITIAL_COOLDOWN)
        {
            playInterstitial(level);
            yield break;
        }
        else
        {
            print("ordinary load level from UI");
            SceneManager.LoadScene(level);
        }
        
    }

    private void playInterstitial(string level)
    {
        print("in playInterstitial");
        
        whatLevelToLoadAfterAdv = level;
        interstitial.OnEnded = continueAfterInterstitial;
        interstitial.ShowInterstitialVideo();
    }

    private void continueAfterInterstitial()
    {
        print("in continueAfterInterstitial - " + Time.timeScale);
        SceneManager.LoadScene(whatLevelToLoadAfterAdv);
    }

    public void SetMainPlayerAbilityManager(AbilityManager manager) => mainPlayerAbilityManager = manager;
    
    public void ShowAbilityButton(AbilityTypes _type)
    {
        abilityButtonPanel.SetActive(true);
        abilityButtonPanel.transform.GetChild(0).localScale = Vector3.one;
        circleForAbilityTimer.fillAmount = 1;

        switch (_type)
        {
            case AbilityTypes.Acceleration:
                abilityButtonImage.sprite = accelerationSprite;
                break;

            case AbilityTypes.RocketBack:
                abilityButtonImage.sprite = rocketPackSprite;
                break;
        }

        if (Globals.IsMobile)
        {
            jump.GetComponent<RectTransform>().anchoredPosition = jumpAbilityPlace;
            abilityButtonPanel.GetComponent<RectTransform>().anchoredPosition = placeWhenMobile;
            abilityButtonTextPC.gameObject.SetActive(false);
        }
        else
        {
            abilityButtonPanel.GetComponent<RectTransform>().anchoredPosition = placeWhenPC;
            abilityButtonTextPC.gameObject.SetActive(true);
        }
    }

    public void SetFillAmountAbilityTimer(float amount)
    {
        circleForAbilityTimer.fillAmount = amount;
    }

    public void HideAbilityButton(bool isPressed)
    {
        StartCoroutine(playPressAbility(isPressed));
    }
    private IEnumerator playPressAbility(bool isPressed)
    {
        if (!isPressed)
        {
            SoundUI.Instance.PlayUISound(SoundsUI.error);

            abilityButtonPanel.transform.GetChild(0).DOScale(Vector3.one * 0.1f, 0.3f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.3f);

            abilityButtonPanel.SetActive(false);

            if (Globals.IsMobile)
            {
                jump.GetComponent<RectTransform>().anchoredPosition = jumpStandartPlace;
            }

            yield break;
        }
        


        SoundUI.Instance.PlayUISound(SoundsUI.success3);

        abilityButtonPanel.transform.GetChild(0).DOPunchPosition(new Vector3(20, 20, 1), 0.15f, 30).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.15f);

        abilityButtonPanel.transform.GetChild(0).DOScale(Vector3.one * 3.5f, 0.25f).SetEase(Ease.Linear);
        abilityButtonPanel.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.25f).SetEase(Ease.Linear);
        abilityButtonPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().DOFade(0, 0.25f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.25f);

        
        abilityButtonPanel.transform.GetChild(0).GetComponent<Image>().DOFade(1, 0).SetEase(Ease.Linear);
        abilityButtonPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().DOFade(1, 0).SetEase(Ease.Linear);

        abilityButtonPanel.SetActive(false);

        if (Globals.IsMobile)
        {
            jump.GetComponent<RectTransform>().anchoredPosition = jumpStandartPlace;
        }
    }

    private void scaleCameraDistance(float val)
    {
        Globals.MainPlayerData.Zoom = val;
        gm.GetCameraControl().Zoom(val);
    }

    public void StartTheGame()
    {
        
        ShowAllControls();
                
        aimBeforeStart.SetActive(false);
        
        if (levelData.GameType != GameTypes.Tutorial)
        {
            aimDuringGame.SetActive(true);
            aimDuringGameText.text = levelData.LevelAim + "!";
        }
        
    }

    public void EndGame(bool isWin)
    {
        
        HideAllControls();
        aimBeforeStart.SetActive(false);
        aimDuringGame.SetActive(false);
        //timerText.text = "0";
        //timerText.color = Color.white;
        timerPanel.SetActive(false);
        AmbientMusic.Instance.StopAll();

        RectTransform r = default;

        if (isWin)
        {
            endGameWin.SetActive(true);

            if (levelData.GameType == GameTypes.Tutorial)
            {
                endGameWinText.text = Globals.Language.TutorialDone;
                //Analytics
                string dataForA = "tutd";
                YandexMetrica.Send(dataForA);
            }
            else
            {
                endGameWinText.text = Globals.Language.WinText;
                //Analytics
                string dataForA = "lvl" + (int)levelData.LevelType + "w";
                YandexMetrica.Send(dataForA);
            }
            
            
            
            r = endGameWin.GetComponent<RectTransform>();
            SoundUI.Instance.PlayUISound(SoundsUI.success);

            
        }
        else
        {
            endGameLose.SetActive(true);
            endGameLoseText.text = Globals.Language.LoseText;
            r = endGameLose.GetComponent<RectTransform>();
            SoundUI.Instance.PlayUISound(SoundsUI.lose);

            //Analytics
            string dataForA = "lvl" + (int)levelData.LevelType + "l";
            YandexMetrica.Send(dataForA);
        }


        //save results
        if (levelData.GameType == GameTypes.Tutorial)
        {
            //repeatButton.gameObject.SetActive(false);
            //mainMenuButton.gameObject.SetActive(false);
            Globals.MainPlayerData.TutL = true;
        }
        else
        {
            print("here saving winrate");

            if (levelData.GameType == GameTypes.Finish_line)
            {
                int place = gm.GetFinishPlace(gm.MainPlayerControl);
                Globals.MainPlayerData.WR = Globals.MainPlayerData.WR.Append(new GameSessionResult(levelData.LevelType, levelData.GameType, place)).ToArray();
            }
            else if (levelData.GameType == GameTypes.Dont_fall || levelData.GameType == GameTypes.Challenge)
            {
                int place = isWin ? 1 : 0;
                Globals.MainPlayerData.WR = Globals.MainPlayerData.WR.Append(new GameSessionResult(levelData.LevelType, levelData.GameType, place)).ToArray();
            }
            
        }

        SaveLoadManager.Save();

        StartCoroutine(playLastAim(r, isWin));
    }
    private IEnumerator playLastAim(RectTransform t, bool isWin)
    {
        t.anchoredPosition3D = new Vector3 (0, 0, 0);
        t.DOPunchScale(new Vector3(50,50,50), 0.3f, 30).SetEase(Ease.OutSine);

        yield return new WaitForSeconds(1);

        if ((levelData.GameType == GameTypes.Finish_line || levelData.GameType == GameTypes.Challenge) && isWin)
        {
            trackRecordPanel.SetActive(true);
            trackRecordPanel.transform.localScale = Vector3.zero;
            trackRecordPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutSine);

            int prevResult = Globals.MainPlayerData.TR[(int)levelData.LevelType];
            int currResult = Mathf.RoundToInt(gm.GameSecondsPlayed);

            if (prevResult == 0 || currResult < prevResult)
            {
                trackRecordText.text = Globals.Language.TrackNewRecord + ":";
                Globals.MainPlayerData.TR[(int)levelData.LevelType] = currResult;
            }
            else
            {
                trackRecordText.text = Globals.Language.TrackRecord + ":";
            }
                        
            recordDataText.text = currResult.ToString() + " " + Globals.Language.Sec;
            

            if (prevResult > 0)
            {
                
                float percent = (float)currResult / (float)prevResult * 100;
                
                if (percent < 100)
                {
                    percentPanel.SetActive(true);
                    percent -= 100;
                    recordPercentText.color = Color.green;
                    recordPercentText.text = "+" + Mathf.Abs(percent).ToString("f0") + "%";
                }
                else if(percent > 100)
                {
                    percentPanel.SetActive(true);
                    percent -= 100;
                    recordPercentText.color = Color.red;
                    recordPercentText.text = "-" + Mathf.Abs(percent).ToString("f0") + "%";
                }                
            }
        }

        yield return new WaitForSeconds(2);
        t.DOAnchorPos3D(new Vector3(0, 340, 0), 0.5f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.3f);

        rewards();
    }

    private void rewards()
    {
        if (!Globals.IsMobile)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        rewardPanel.SetActive(true);
        rewardPanel.transform.localScale = Vector3.zero;
        rewardPanel.transform.DOScale(Vector3.one, 0.2f).SetEase (Ease.OutSine);

        goldPanel.SetActive(true);
        xpPanel.SetActive(true);

        continueButtonText.text = Globals.Language.ContinueCamelCase;
        mainMenuButtonText.text = Globals.Language.ToMenu;
        repeatButtonText.text = Globals.Language.Repeat;
                
        int xpReward = 0;
        int goldReward = 0;
        gm.AssessReward(out xpReward, out goldReward);

        if (xpReward > 0)
        {            
            Globals.AddXP(xpReward);            
        }

        if (goldReward > 0)
        {
            GetRewardSystem.Instance.ShowEffect(RewardTypes.gold, goldReward);
            Globals.MainPlayerData.G += goldReward;            
        }

        xpText.text = xpReward.ToString();
        goldText.text = goldReward.ToString();

        //FPS
        if (FPSController.Instance != null)
        {
            float fps = FPSController.Instance.GetAverage();
            Globals.MainPlayerData.FPS = fps;
            print("FPS during the game is " + fps);
        }
        

        SaveLoadManager.Save();

        if (levelData.GameType == GameTypes.Tutorial)
        {
            continueButton.gameObject.SetActive(true);
        }
        else
        {
            continueButton.gameObject.SetActive(true);
            repeatButton.gameObject.SetActive(true);
            mainMenuButton.gameObject.SetActive(true);
        }
    }

    public void ShowTimerData(float _time)
    {
        if (!timerPanel.activeSelf) timerPanel.SetActive(true);
        if (_time < 0) _time = 0;

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

        if (newTime <= 5)
        {
            timerText.color = Color.red;
            SoundUI.Instance.PlayUISound(SoundsUI.tick);
        }
        else if (newTime <= 10)
        {
            timerText.color = Color.yellow;
        }
        else
        {
            timerText.color = Color.white;
        }
    }

    public void ShowAllControls()
    {
        scalerPanelCallButton.gameObject.SetActive(Globals.IsMobile);

        if (Globals.IsMobile)
        {
            lettersHelper.SetActive(false);
            mouseHelper.SetActive(false);
            joystick.gameObject.SetActive(true);
            jump.gameObject.SetActive(true);
        }
        else
        {
            //RETURN LATER
            mouseHelper.SetActive(true);
            lettersHelper.SetActive(true);
            joystick.gameObject.SetActive(false);
            jump.gameObject.SetActive(false);
        }
    }

    public void HideAllControls()
    {
        scalerPanel.SetActive(false);
        scalerPanelCallButton.gameObject.SetActive(false);

        lettersHelper.SetActive(false);
        mouseHelper.SetActive(false);
        joystick.gameObject.SetActive(false);
        jump.gameObject.SetActive(false);
        abilityButtonPanel.SetActive(false);
    }

    public void ShowInformer(string text)
    {
        StartCoroutine(playInformer(text));
    }
    private IEnumerator playInformer(string _text)
    {
        informerPanel.SetActive(true);
        informerText.text = "";

        yield return new WaitForSeconds(0.1f);
        SoundUI.Instance.PlayUISound(SoundsUI.beep_out);

        int nums = _text.Length;
        string[] letters = new string[nums];
        string result = "";

        for (int i = 0; i < nums; i++) 
        {
            letters[i] = _text.Substring(i, 1);
        }

        for (int i = 0;i < nums; i++)
        {
            result += letters[i];
            //string adder = new string((char)32, nums - i);
            informerText.text = result;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(1);

        for (int i = 0; i < nums; i++)
        {
            result = _text.Substring(0, nums - i);
            //string adder = new string((char)32, nums - i);
            informerText.text = result;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        informerPanel.SetActive(false);
        informerText.text = "";
    }

   
}
