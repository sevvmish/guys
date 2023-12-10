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

    [Header("skip level for rewarded")]
    [SerializeField] private Button offerSkipLevelForRewarded;
    [SerializeField] private TextMeshProUGUI offerSkipLevelText;
    [SerializeField] private GameObject skipLevelConfirmationPanel;
    [SerializeField] private Button skipLevelForRewardedOK;
    [SerializeField] private Button skipLevelForRewardedNO;
    [SerializeField] private TextMeshProUGUI skipLevelConfirmationText;
        

    // Start is called before the first frame update
    void Start()
    {        
        offerSkipLevelForRewarded.gameObject.SetActive(false);
        skipLevelConfirmationPanel.SetActive(false);

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

        if (Globals.Language != null)
        {
            letterUp.text = Globals.Language.UpArrowLetter;
            letterDown.text = Globals.Language.DownArrowLetter;
            letterLeft.text = Globals.Language.LeftArrowLetter;
            letterRight.text = Globals.Language.RightArrowLetter;
            signJump.text = Globals.Language.JumpLetter;

            offerSkipLevelText.text = Globals.Language.SkipLevelOffer;
            skipLevelConfirmationText.text = Globals.Language.SkipLevelConfirmation;
        }

        offerSkipLevelForRewarded.onClick.AddListener(() => 
        {
            if (skipLevelConfirmationPanel.activeSelf) return;

            skipLevelConfirmationPanel.SetActive(true);
            Globals.IsOptions = true;
        });

        skipLevelForRewardedOK.onClick.AddListener(() =>
        {
            skipLevelPhaseOK();

        });

        skipLevelForRewardedNO.onClick.AddListener(() =>
        {
            skipLevelConfirmationPanel.SetActive(false);
            Globals.IsOptions = false;
        });
    }

    public void OfferSkipLevelForRewarded()
    {
        if ((DateTime.Now - Globals.TimeWhenLastRewardedWas).TotalSeconds < Globals.REWARDED_COOLDOWN) return;
        SoundUI.Instance.PlayUISound(SoundsUI.pop);
        offerSkipLevelForRewarded.gameObject.SetActive(true);
    }

    private void skipLevelPhaseOK()
    {
        Globals.IsOptions = false;
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
