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

        
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
                
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
            //RETURN LATER
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
