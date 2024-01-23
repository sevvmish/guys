using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainTitletext;
    [SerializeField] private TextMeshProUGUI aimText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private Image mainIcon;
    [SerializeField] private GameObject closer;
    [SerializeField] private TextMeshProUGUI lvlRestrictionText;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private GameObject newPanel;
    [SerializeField] private TextMeshProUGUI newText;

    private LevelData levelData;

    public void SetMap(LevelData data, bool isAvailable)
    {
        newPanel.SetActive(false);

        levelData = data;

        mainTitletext.text = levelData.LevelName;
        aimText.text = levelData.LevelAim + "!";

        for (int i = 0; i < stars.Length; i++)
        {
            if (i < levelData.Difficulty)
            {
                stars[i].SetActive(true);
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
    }

    public void ShowNewMark()
    {
        newPanel.SetActive(true);
        newText.text = Globals.Language.New;
    }
}
