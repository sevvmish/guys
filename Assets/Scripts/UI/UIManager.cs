using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using UnityEngine.SceneManagement;

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


    private GameManager gm;
    private LevelData levelData;
    private AbilityManager mainPlayerAbilityManager;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        //jumpStandartPlace = jump.GetComponent<RectTransform>().anchoredPosition;
        circleForAbilityTimer = abilityButtonPanel.transform.GetChild(0).GetComponent<Image>();
        jump.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        

        HideAllControls();

        aimBeforeStart.SetActive(false);
        aimDuringGame.SetActive(false);
        endGameWin.SetActive(false);
        endGameLose.SetActive(false);
        rewardPanel.SetActive(false);

        levelData = LevelManager.GetLevelData(gm.GetLevelManager().GetCurrentLevelType());

        aimBeforeStart.SetActive(true);
        aimBeforeStartText.text = Globals.Language.Aim + ": " + levelData.LevelAim + "!";

        if (gm.GetLevelManager().GetCurrentLevelType() == LevelTypes.level4)
        {
            StartCoroutine(hideAimAfterSec(5));
        }

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

            if (levelData.GameType == GameTypes.Tutorial)
            {
                StartCoroutine(playStartLevel("level1"));
            }
        });

        repeatButton.onClick.AddListener(() =>
        {
            SoundUI.Instance.PlayUISound(SoundsUI.positive);
            repeatButton.interactable = false;
            StartCoroutine(playStartLevel(levelData.LevelInInspector));
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            SoundUI.Instance.PlayUISound(SoundsUI.positive);
            mainMenuButton.interactable = false;
            StartCoroutine(playStartLevel("MainMenu"));
        });
    }
    private IEnumerator hideAimAfterSec(float sec)
    {        
        yield return new WaitForSeconds(sec);
        aimBeforeStart.SetActive(false);
    }


    private IEnumerator playStartLevel(string level)
    {
        ScreenSaver.Instance.HideScreen();
        yield return new WaitForSeconds(Globals.SCREEN_SAVER_AWAIT + 0.2f);
        SceneManager.LoadScene(level);
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
        print("startted in UI");
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
        print("ended in UI");
        HideAllControls();
        aimBeforeStart.SetActive(false);
        aimDuringGame.SetActive(false);

        RectTransform r = default;

        if (isWin)
        {
            endGameWin.SetActive(true);

            if (levelData.GameType == GameTypes.Tutorial)
            {
                endGameWinText.text = Globals.Language.TutorialDone;
            }
            else
            {
                endGameWinText.text = Globals.Language.WinText;
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
        }

        StartCoroutine(playLastAim(r));
    }
    private IEnumerator playLastAim(RectTransform t)
    {
        t.anchoredPosition3D = new Vector3 (0, 0, 0);
        t.DOPunchScale(new Vector3(50,50,50), 0.3f, 30).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(3);
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

        if (levelData.GameType == GameTypes.Tutorial)
        {
            repeatButton.gameObject.SetActive(false);
            mainMenuButton.gameObject.SetActive(false);
            Globals.MainPlayerData.TutL = true;            
        }
        else
        {
            print("here saving");
            int place = gm.GetFinishPlace(gm.MainPlayerControl);
            Globals.MainPlayerData.WR = Globals.MainPlayerData.WR.Append(new GameSessionResult(levelData.LevelType, levelData.GameType, place)).ToArray();
        }

        int xpReward = 0;
        int goldReward = 0;
        gm.AssessReward(out xpReward, out goldReward);

        if (xpReward > 0)
        {
            GetRewardSystem.Instance.ShowEffect(RewardTypes.xp, xpReward);
            bool isLvl = Globals.AddXP(xpReward);
            if (isLvl)
            {
                GetRewardSystem.Instance.ShowEffect(RewardTypes.newLvl, MainMenu.GetCurrentLevel());
            }
        }

        if (goldReward > 0)
        {
            GetRewardSystem.Instance.ShowEffect(RewardTypes.gold, goldReward);
            Globals.MainPlayerData.G += goldReward;            
        }

        xpText.text = xpReward.ToString();
        goldText.text = goldReward.ToString();

        SaveLoadManager.Save();
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
