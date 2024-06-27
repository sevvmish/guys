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
                maps[i - 1].SetMap(LevelManager.GetLevelData((LevelTypes)i), true, true);
            }
            else
            {
                maps[i - 1].SetMap(LevelManager.GetLevelData((LevelTypes)i), false, true);
            }
        }

        if (Globals.MainPlayerData.XPN)
        {
            for (int i = 1; i < Globals.MainPlayerData.LvlA.Length; i++)
            {                
                if (Globals.MainPlayerData.LvlA[i] == 1)
                {
                    LevelData ld = LevelManager.GetLevelData((LevelTypes)i);

                    if (ld.LevelRestriction == MainMenu.GetCurrentLevel() && !Globals.MainPlayerData.AllMaps)
                    {
                        maps[i - 1].ShowNewMark();
                    }
                }                
            }

            Globals.MainPlayerData.XPN = false;
            SaveLoadManager.Save();
        }

        StartCoroutine(playSlide());

        
    }
    private IEnumerator playSlide()
    {
        yield return new WaitForSeconds(0);
        locationRect.anchoredPosition = new Vector2(10000, 0);
        /*
        for (int i = 0; i < 10000; i++)
        {
            locationRect.anchoredPosition = new Vector2(i, 0);
        }
        */
        yield return new WaitForSeconds(0);


        int currLvl = MainMenu.GetCurrentLevel();
        //locationRect.anchoredPosition = Vector2.zero;
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
        //mainMenu.GetCameraTransform.DOMove(new Vector3(-6, 0, -9), 0.5f).SetEase(Ease.Linear);
        mainMenu.MainPlayerSkin.SetActive(false);
        updateMapData();
    }

    private IEnumerator playNewLVLs()
    {
        yield return new WaitForSeconds(1);

        int newLevel = MainMenu.GetLevelByXP(Globals.MainPlayerData.XP);
        bool isChanged = false;
        for (int i = 1; i < Globals.MainPlayerData.LvlA.Length; i++)
        {
            if (Globals.MainPlayerData.LvlA[i] == 0)
            {
                LevelData ld = LevelManager.GetLevelData((LevelTypes)i);
                if (newLevel >= ld.LevelRestriction)
                {
                    Globals.MainPlayerData.LvlA[i] = 1;
                    GetRewardSystem.Instance.ShowEffect(RewardTypes.newMap, i);
                    isChanged = true;
                }
            }
        }

        if (isChanged) SaveLoadManager.Save();
    }

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;

            StartCoroutine(playNewLVLs());

            
            /*
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
            */

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
