using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

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
    

    [Header("ADV")]
    [SerializeField] private Rewarded rewarded;

    [Header("skip level for rewarded")]
    [SerializeField] private Button offerSkipLevelForRewarded;
    [SerializeField] private TextMeshProUGUI offerSkipLevelText;
    [SerializeField] private GameObject skipLevelConfirmationPanel;
    [SerializeField] private Button skipLevelForRewardedOK;
    [SerializeField] private Button skipLevelForRewardedNO;
    [SerializeField] private TextMeshProUGUI skipLevelConfirmationText;
    [SerializeField] private TextMeshProUGUI InfoWithButtonSkipLevelText;
    [SerializeField] private GameObject InfoWithButtonSkipLevel;    
    
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        offerSkipLevelForRewarded.gameObject.SetActive(false);
        skipLevelConfirmationPanel.SetActive(false);
        InfoWithButtonSkipLevel.SetActive(false);
        scalerPanel.SetActive(false);
        scalerPanelCallButton.gameObject.SetActive(Globals.IsMobile);
        scalerSlider.value = Globals.MainPlayerData.Zoom;

        if (Globals.Language != null)
        {
            letterUp.text = Globals.Language.UpArrowLetter;
            letterDown.text = Globals.Language.DownArrowLetter;
            letterLeft.text = Globals.Language.LeftArrowLetter;
            letterRight.text = Globals.Language.RightArrowLetter;
            signJump.text = Globals.Language.JumpLetter;
            InfoWithButtonSkipLevelText.text = Globals.Language.PressButtonWhenSkipLevel;

            offerSkipLevelText.text = Globals.Language.SkipLevelOffer;
            skipLevelConfirmationText.text = Globals.Language.SkipLevelConfirmation;

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

        offerSkipLevelForRewarded.onClick.AddListener(() => 
        {
            if (skipLevelConfirmationPanel.activeSelf) return;

            SoundUI.Instance.PlayUISound(SoundsUI.click);
            skipLevelConfirmationPanel.SetActive(true);
            offerSkipLevelForRewarded.gameObject.SetActive(false);
            Globals.IsOptions = true;
        });

        skipLevelForRewardedOK.onClick.AddListener(() =>
        {
            skipLevelPhaseOK();            
        });

        skipLevelForRewardedNO.onClick.AddListener(() =>
        {
            SoundUI.Instance.PlayUISound(SoundsUI.click);
            InfoWithButtonSkipLevel.SetActive(false);
            skipLevelConfirmationPanel.SetActive(false);
            offerSkipLevelForRewarded.gameObject.SetActive(false);
            Globals.IsOptions = false;

            if (!Globals.IsMobile)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        });
    }

    private void scaleCameraDistance(float val)
    {
        Globals.MainPlayerData.Zoom = val;
        gm.GetCameraControl().Zoom(val);
    }

    public void StartTheGame()
    {
        ShowAllControls();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) 
            && offerSkipLevelForRewarded.gameObject.activeSelf 
            && !skipLevelConfirmationPanel.activeSelf)
        {            
            SoundUI.Instance.PlayUISound(SoundsUI.click);
            skipLevelConfirmationPanel.SetActive(true);
            Globals.IsOptions = true;
            offerSkipLevelForRewarded.gameObject.SetActive(false);

            if (!Globals.IsMobile)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }
    }

    public void ResetOfferSkipLevel()
    {
        offerSkipLevelForRewarded.gameObject.SetActive(false);
    }

    public void OfferSkipLevelForRewarded()
    {
        int newRespID = RespawnManager.Instance.GetCurrentIndex;
        if (newRespID < 2 || newRespID >= 24) return;
        
        if ((DateTime.Now - Globals.TimeWhenLastRewardedWas).TotalSeconds < Globals.REWARDED_COOLDOWN) return;
        if (offerSkipLevelForRewarded.gameObject.activeSelf || skipLevelConfirmationPanel.activeSelf) return;

        

        if (!Globals.IsMobile) InfoWithButtonSkipLevel.SetActive(true);
        SoundUI.Instance.PlayUISound(SoundsUI.pop);
        offerSkipLevelForRewarded.gameObject.SetActive(true);

    }

    private void skipLevelPhaseOK()
    {
        InfoWithButtonSkipLevel.SetActive(false);
        skipLevelConfirmationPanel.SetActive(false);
        offerSkipLevelForRewarded.gameObject.SetActive(false);
        
        

        rewarded.OnError = rewardNotOK;
        rewarded.OnRewardedEndedOK = rewardOKSkipLevel;
        rewarded.ShowRewardedVideo();

    }
    private void rewardOKSkipLevel()
    {
        print("started increasing resp point");

        int currInd = RespawnManager.Instance.GetCurrentIndex;

        if (currInd == 7)
        {
            currInd += 2;
        }
        else
        {
            currInd++;
        }
                

        PlayerControl pc = gm.GetMainPlayerTransform().GetComponent<PlayerControl>();
        pc.SetPlayerDirection(transform.eulerAngles.y);
        pc.transform.position = RespawnManager.Instance.GetRespawnPoint(currInd).transform.position;
        SoundUI.Instance.PlayUISound(SoundsUI.positive);

        Globals.IsOptions = false;
        if (!Globals.IsMobile)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void rewardNotOK()
    {
        SoundUI.Instance.PlayUISound(SoundsUI.error);
        Globals.IsOptions = false;
        if (!Globals.IsMobile)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ShowAllControls()
    {
        if (Globals.IsMobile)
        {
            lettersHelper.SetActive(false);
            mouseHelper.SetActive(false);
            joystick.gameObject.SetActive(true);
            jump.gameObject.SetActive(true);
        }
        else
        {
            mouseHelper.SetActive(true);
            lettersHelper.SetActive(true);
            joystick.gameObject.SetActive(false);
            jump.gameObject.SetActive(false);
        }
    }

    public void HideAllControls()
    {
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
