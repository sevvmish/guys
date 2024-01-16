using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button back;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;
    [SerializeField] private GameObject NOAds;

    [Header("Data")]
    [SerializeField] private TextMeshProUGUI gemsText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI signName;
    [SerializeField] private GameObject backPart;

    [SerializeField] private MainMenu mainMenu;

    private bool isDataUpdated;

    private string previousSignName;
    private bool isBackActive;

    // Start is called before the first frame update
    void Start()
    {
        NOAds.SetActive(false);
        signName.text = "";

        optionsButton.gameObject.SetActive(true);
        optionsPanel.SetActive(false);

        optionsButton.onClick.AddListener(() =>
        {
            openOptions();
        });

        back.onClick.AddListener(() =>
        {
            SoundUI.Instance.PlayUISound(SoundsUI.click);

            mainMenu.OnBackToMainMenu?.Invoke();

            if (!optionsPanel.activeSelf)
            {
                backPart.SetActive(false);
                signName.text = "";
            }
            else
            {
                backPart.SetActive(isBackActive);
                signName.text = previousSignName;
            }
            

            if (!optionsPanel.activeSelf) mainMenu.BackToMainMenu();

            optionsButton.gameObject.SetActive(true);
            optionsPanel.SetActive(false);
            
            
        });

        soundButton.onClick.AddListener(() =>
        {
            if (Globals.IsSoundOn)
            {
                Globals.IsSoundOn = false;
                soundButton.transform.GetChild(0).GetComponent<Image>().sprite = soundOffSprite;
                AudioListener.volume = 0;
            }
            else
            {
                SoundUI.Instance.PlayUISound(SoundsUI.click);
                Globals.IsSoundOn = true;
                soundButton.transform.GetChild(0).GetComponent<Image>().sprite = soundOnSprite;
                AudioListener.volume = 1f;
            }

            SaveLoadManager.Save();
        });

        musicButton.onClick.AddListener(() =>
        {
            if (Globals.IsMusicOn)
            {
                Globals.IsMusicOn = false;
                musicButton.transform.GetChild(0).GetComponent<Image>().sprite = musicOffSprite;
                AmbientMusic.Instance.StopAll();
            }
            else
            {
                SoundUI.Instance.PlayUISound(SoundsUI.click);
                Globals.IsMusicOn = true;
                musicButton.transform.GetChild(0).GetComponent<Image>().sprite = musicOnSprite;
                AmbientMusic.Instance.ContinuePlaying();
            }

            SaveLoadManager.Save();
        });
    }

    public void UpdateCurrencyData()
    {
        gemsText.text = Globals.MainPlayerData.D.ToString();
        goldText.text = Globals.MainPlayerData.G.ToString();
        NOAds.SetActive(Globals.MainPlayerData.AdvOff);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Globals.MainPlayerData = new PlayerData();
            SaveLoadManager.Save();

            SceneManager.LoadScene("MainMenu");
        }

        if (Globals.IsInitiated && !isDataUpdated)
        {
            isDataUpdated = true;
            UpdateCurrencyData();
        }
    }

    private void openOptions()
    {
        isBackActive = backPart.activeSelf;
        backPart.SetActive(true);

        previousSignName = signName.text;
        signName.text = Globals.Language.Options;

        if (Globals.IsSoundOn)
        {
            soundButton.transform.GetChild(0).GetComponent<Image>().sprite = soundOnSprite;
        }
        else
        {
            soundButton.transform.GetChild(0).GetComponent<Image>().sprite = soundOffSprite;
        }

        if (Globals.IsMusicOn)
        {
            musicButton.transform.GetChild(0).GetComponent<Image>().sprite = musicOnSprite;
        }
        else
        {
            musicButton.transform.GetChild(0).GetComponent<Image>().sprite = musicOffSprite;
        }

        SoundUI.Instance.PlayUISound(SoundsUI.click);
        optionsButton.gameObject.SetActive(false);
        optionsPanel.SetActive(true);

        soundButton.transform.localScale = Vector3.zero;
        musicButton.transform.localScale = Vector3.zero;

        soundButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        musicButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
    }

    public void SetBackButtonSign(string sign)
    {
        previousSignName = signName.text;
        signName.text = sign;

        isBackActive = backPart.activeSelf;
        backPart.SetActive(true);
    }
}
