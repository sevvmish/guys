using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject mapExample;
    [SerializeField] private Transform location;
    [SerializeField] private RectTransform mainRect;
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
        locationRect.anchoredPosition = Vector2.zero;
        if (currLvl <= 1)
        {
            //locationRect.anchoredPosition = new Vector2(locationRect.anchoredPosition.x - 900, locationRect.anchoredPosition.y);
            locationRect.DOAnchorPos(new Vector2(locationRect.anchoredPosition.x - 900, locationRect.anchoredPosition.y), 0.3f).SetEase(Ease.Linear);
        }
        else
        {
            //locationRect.anchoredPosition = new Vector2(locationRect.anchoredPosition.x - 900 - 835 * (currLvl-1), locationRect.anchoredPosition.y);
            locationRect.DOAnchorPos(new Vector2(locationRect.anchoredPosition.x - 900 - 835 * (currLvl - 1), locationRect.anchoredPosition.y), 0.5f).SetEase(Ease.Linear);
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

            if (Globals.IsMobile)
            {
                mainRect.anchoredPosition = new Vector2(0, -70);
            }
            else
            {
                if (!Globals.MainPlayerData.AdvOff)
                {
                    mainRect.anchoredPosition = new Vector2(0, 50);
                }
                else
                {
                    mainRect.anchoredPosition = new Vector2(0, 0);
                }
                
            }


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
