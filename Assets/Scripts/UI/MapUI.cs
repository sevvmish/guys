using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    public LevelTypes LevelType { get; private set; }

    [SerializeField] private TextMeshProUGUI mainTitletext;
    [SerializeField] private TextMeshProUGUI aimText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private Image mainIcon;
    [SerializeField] private GameObject closer;
    [SerializeField] private TextMeshProUGUI lvlRestrictionText;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private GameObject newPanel;
    [SerializeField] private TextMeshProUGUI newText;
    [SerializeField] private Button unblockB;
    [SerializeField] private TextMeshProUGUI unblockText;
    [SerializeField] private TextMeshProUGUI unblockPriceText;
    [SerializeField] private Button playB;
    [SerializeField] private TextMeshProUGUI playBText;
    [SerializeField] private Sprite grey;
    [SerializeField] private Sprite green;

    private LevelData levelData;

    public void SetMap(LevelData data, bool isAvailable, bool isUnlockable)
    {
        newPanel.SetActive(false);

        levelData = data;
        LevelType = data.LevelType;

        mainTitletext.text = levelData.LevelName;
        aimText.text = levelData.LevelAim + "!";
        Color starColor = Color.green;

        if (levelData.Difficulty <= 2)
        {
            starColor = Color.green;
        }
        else if (levelData.Difficulty <= 3)
        {
            starColor = Color.yellow;
        }
        else if (levelData.Difficulty <= 4)
        {
            starColor = new Color(1, 0.5f, 0, 1);
        }
        else
        {
            starColor = Color.red;
        }

        for (int i = 0; i < stars.Length; i++)
        {
            if (i < levelData.Difficulty)
            {
                stars[i].SetActive(true);
                stars[i].GetComponent<Image>().color = starColor;
            }
            else
            {
                stars[i].SetActive(false);
            }
        }

        difficultyText.text = Globals.Language.Difficulty + ":";

        mainIcon.sprite = levelData.ImageSprite;                
        closer.SetActive(!isAvailable);
        

        if (!isAvailable)
        {
            lvlRestrictionText.text = levelData.LevelRestriction.ToString() + " " + Globals.Language.LevelShort;            
        }

        unblockB.gameObject.SetActive(false);

        if (!isAvailable && data.UblockGemPrice > 0 && isUnlockable)
        {
            unblockText.text = Globals.Language.Unblock;
            unblockPriceText.text = data.UblockGemPrice.ToString();
            unblockB.gameObject.SetActive(true);

            if (Globals.MainPlayerData.D < data.UblockGemPrice)
            {
                unblockB.GetComponent<Image>().sprite = grey;
            }
            else
            {
                unblockB.GetComponent<Image>().sprite = green;
            }

            unblockB.onClick.AddListener(() => 
            {
                if (Globals.MainPlayerData.D < data.UblockGemPrice)
                {
                    SoundUI.Instance.PlayUISound(SoundsUI.error);                    
                    return;
                }

                Globals.MainPlayerData.D -= data.UblockGemPrice;
                SoundUI.Instance.PlayUISound(SoundsUI.cash);
                Globals.MainPlayerData.LvlA[(int)data.LevelType] = 1;
                SaveLoadManager.Save();

                closer.SetActive(false);
                unblockB.gameObject.SetActive(false);
            });
        }

        playB.gameObject.SetActive(false);

        if (isUnlockable && isAvailable)
        {
            playB.gameObject.SetActive(true);
            playBText.text = Globals.Language.Play;

            playB.onClick.AddListener(() =>
            {
                SoundUI.Instance.PlayUISound(SoundsUI.positive);
                playB.interactable = false;
                StartCoroutine(playStartLevel(levelData.LevelInInspector));
            });            
        }
    }

    private IEnumerator playStartLevel(string level)
    {
        ScreenSaver.Instance.HideScreen();
        yield return new WaitForSeconds(Globals.SCREEN_SAVER_AWAIT + 0.2f);

        SceneManager.LoadScene(level);
    }

    public void ShowNewMark()
    {
        newPanel.SetActive(true);
        newText.text = Globals.Language.New;        
    }
}
