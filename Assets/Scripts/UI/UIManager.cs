using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
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


    private GameManager gm;
    private LevelData levelData;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        HideAllControls();

        aimBeforeStart.SetActive(false);
        aimDuringGame.SetActive(false);
        endGameWin.SetActive(false);
        endGameLose.SetActive(false);
        rewardPanel.SetActive(false);

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
            //=
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

    private IEnumerator playStartLevel(string level)
    {
        ScreenSaver.Instance.HideScreen();
        yield return new WaitForSeconds(Globals.SCREEN_SAVER_AWAIT + 0.2f);
        SceneManager.LoadScene(level);
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
        t.DOAnchorPos3D(new Vector3(0, 320, 0), 0.5f).SetEase(Ease.OutSine);
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
            Globals.MainPlayerData.TutL = true;            
        }
        else
        {
            int place = gm.GetFinishPlace(gm.MainPlayerControl);
            Globals.MainPlayerData.WR.AddResult(new GameSessionResult(levelData.LevelType, levelData.GameType, place));
        }

        int xpReward = 0;
        int goldReward = 0;
        gm.AssessReward(out xpReward, out goldReward);

        if (xpReward > 0)
        {
            GetRewardSystem.Instance.ShowEffect(RewardTypes.xp, xpReward);
            Globals.AddXP(xpReward);
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
