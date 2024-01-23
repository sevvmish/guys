using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject mapExample;
    [SerializeField] private Transform location;
    private RectTransform locationRect;
    
    List <MapUI> maps = new List<MapUI>();
    private bool isReady;

    // Start is called before the first frame update
    void Start()
    {
        back.SetActive(false);
        mainMenu.OnBackToMainMenu += ReturnBack;
        locationRect = location.GetComponent<RectTransform>();
    }

    private void updateMapData()
    {
        for (int i = 1; i < Globals.MainPlayerData.LvlA.Length; i++)
        {
            if (Globals.MainPlayerData.LvlA[i] == 1)
            {
                maps[i - 1].SetMap(LevelManager.GetLevelData((LevelTypes)i), true);
            }
            else
            {
                maps[i - 1].SetMap(LevelManager.GetLevelData((LevelTypes)i), false);
            }
        }

        if (Globals.MainPlayerData.XPN)
        {
            for (int i = 1; i < Globals.MainPlayerData.LvlA.Length; i++)
            {                
                if (Globals.MainPlayerData.LvlA[i] == 1)
                {
                    LevelData ld = LevelManager.GetLevelData((LevelTypes)i);

                    if (ld.LevelRestriction == MainMenu.GetCurrentLevel())
                    {
                        maps[i - 1].ShowNewMark();
                    }
                }                
            }

            Globals.MainPlayerData.XPN = false;
            SaveLoadManager.Save();
        }

        int currLvl = MainMenu.GetCurrentLevel();
        if (currLvl <= 1)
        {
            locationRect.anchoredPosition = new Vector2(locationRect.anchoredPosition.x - 900, locationRect.anchoredPosition.y);
        }
        else
        {
            locationRect.anchoredPosition = new Vector2(locationRect.anchoredPosition.x - 900 - 835 * (currLvl-1), locationRect.anchoredPosition.y);
        }
    }

    public void SetOn()
    {
        back.SetActive(true);
        mainMenu.GetCameraTransform.DOMove(new Vector3(-6, 0, -9), 0.5f).SetEase(Ease.Linear);
        mainMenu.MainPlayerSkin.SetActive(false);
        updateMapData();
    }

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;
            for (int i = 1; i < Globals.MainPlayerData.LvlA.Length; i++)
            {
                GameObject map = Instantiate(mapExample, location);
                map.SetActive(true);
                maps.Add(map.GetComponent<MapUI>());
            }            
        }
    }

    private void ReturnBack()
    {
        back.SetActive(false);
        mainMenu.BackToMainMenu();
    }
}
