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
    [SerializeField] private Button skipButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;

    private GameManager gm;
    private LevelManager lm;

    // Start is called before the first frame update
    void Start()
    {
        optionsButton.gameObject.SetActive(false);
        optionsPanel.SetActive(false);

        gm = GameManager.Instance;
        lm = gm.GetLevelManager();

        if (!Globals.IsMobile)
        {
            optionsButton.transform.localScale = Vector3.one * 0.8f;
        }

        if (lm.GetCurrentLevelType() == LevelTypes.tutorial)
        {
            skipButton.gameObject.SetActive(false);
        }
        else
        {
            skipButton.gameObject.SetActive(true);
        }

        //options
        optionsButton.onClick.AddListener(() =>
        {
            openOptions();
        });

        skipButton.onClick.AddListener(() =>
        {
            GameManager.Instance.SetPause(false);
            //SceneManager.LoadScene("LevelSetter");
            StartCoroutine(playLevelSetter());
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
            GameManager.Instance.SetPause(false);
            //SceneManager.LoadScene("MainMenu");
            StartCoroutine(playMainMenu());
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

    private IEnumerator playLevelSetter()
    {
        ScreenSaver.Instance.HideScreen();
        yield return new WaitForSeconds(Globals.SCREEN_SAVER_AWAIT + 0.2f);
        SceneManager.LoadScene("LevelSetter");
    }

    private IEnumerator playMainMenu()
    {
        ScreenSaver.Instance.HideScreen();
        yield return new WaitForSeconds(Globals.SCREEN_SAVER_AWAIT + 0.2f);
        SceneManager.LoadScene("MainMenu");
    }

    private void continuePlay()
    {
        GameManager.Instance.SetPause(false);

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

        GameManager.Instance.SetPause(true);

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
        skipButton.transform.localScale = Vector3.zero;
        soundButton.transform.localScale = Vector3.zero;
        homeButton.transform.localScale = Vector3.zero;
        musicButton.transform.localScale = Vector3.zero;

        continueButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        skipButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
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

        if (lm.GetCurrentLevelType() != LevelTypes.tutorial)
        {
            optionsButton.gameObject.SetActive(true);
        }        
    }

    public void TurnAllOff()
    {
        Globals.IsOptions = false;
        optionsPanel.SetActive(false);
        optionsButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStarted) return;

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
