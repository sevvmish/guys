using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    [Header("options menu")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;


    // Start is called before the first frame update
    void Start()
    {
        optionsButton.gameObject.SetActive(false);
        optionsPanel.SetActive(false);

        GameManager gm = GameManager.Instance;

        //options
        optionsButton.onClick.AddListener(() =>
        {
            openOptions();
        });

        continueButton.onClick.AddListener(() =>
        {
            continuePlay();
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

        homeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainMenu");
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

    private void continuePlay()
    {
        if (!Globals.IsMobile)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Globals.IsOptions = false;

        SoundUI.Instance.PlayUISound(SoundsUI.click);
        optionsButton.gameObject.SetActive(Globals.IsMobile);
        optionsPanel.SetActive(false);
    }



    private void openOptions()
    {
        if (!GameManager.Instance.IsGameStarted) return;

        if (!Globals.IsMobile)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        Globals.IsOptions = true;

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


        continueButton.transform.localScale = Vector3.zero;
        soundButton.transform.localScale = Vector3.zero;
        homeButton.transform.localScale = Vector3.zero;
        musicButton.transform.localScale = Vector3.zero;

        continueButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        soundButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        homeButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        musicButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);

        if (GameManager.Instance.GetLevelManager().GetCurrentLevelType() == LevelTypes.tutorial)
        {
            homeButton.gameObject.SetActive(false);
        }
    }

    public void TurnAllOn()
    {
        
        Globals.IsOptions = false;
        optionsPanel.SetActive(false);
        optionsButton.gameObject.SetActive(Globals.IsMobile);
    }

    public void TurnAllOff()
    {
        Globals.IsOptions = false;
        optionsPanel.SetActive(false);
        optionsButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Globals.IsOptions)
            {
                
                openOptions();
            }
            else
            {
                
                continuePlay();
            }            
        }
    }
}
