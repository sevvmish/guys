using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetter : MonoBehaviour
{
        
    [SerializeField] private GameObject mapExample;
    [SerializeField] private Transform location;
    private RectTransform locationRect;

    List<MapUI> maps = new List<MapUI>();
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < Globals.MainPlayerData.LvlA.Length; i++)
        {
            GameObject map = Instantiate(mapExample, location);
            map.SetActive(true);
            maps.Add(map.GetComponent<MapUI>());
        }

        locationRect = location.GetComponent<RectTransform>();
        getNextLevel();
        updateMapData();
    }

    private LevelTypes getNextLevel()
    {


        List<int> whatLVLsPlayed = new List<int>();
        for (int i = 0; i < Globals.MainPlayerData.LvlA.Length; i++)
        {
            whatLVLsPlayed.Add(Globals.MainPlayerData.LvlA[i]);
            whatLVLsPlayed[whatLVLsPlayed.Count - 1] = 0;
        }
                
        if (Globals.MainPlayerData.WR.Length > 0)
        {
            List<GameSessionResult> results = new List<GameSessionResult>(Globals.MainPlayerData.WR);
            for (int i = 0; i < results.Count; i++)
            {
                whatLVLsPlayed[(int)results[i].LevelType] = 1;
            }
        }
                       

        List<int> levelToChose = new List<int>();
        for (int i = 1; i < whatLVLsPlayed.Count; i++)
        {            
            if (whatLVLsPlayed[i] == 0 && Globals.MainPlayerData.LvlA[i] == 1)
            {
                levelToChose.Add(i);
                print("level " + i + " to account");
            }
        }

        if (levelToChose.Count > 0)
        {
            int rnd = UnityEngine.Random.Range(0, levelToChose.Count);

            print("result: " + (LevelTypes)levelToChose[rnd]);
            return (LevelTypes)levelToChose[rnd];
        }
        else
        {
            for (int i = 0; i < Globals.MainPlayerData.LvlA.Length; i++)
            {
                if (Globals.MainPlayerData.LvlA[i] == 1)
                {
                    levelToChose.Add(i);
                }                
            }

            int rnd = UnityEngine.Random.Range(0, levelToChose.Count);

            print("after second row result: " + (LevelTypes)levelToChose[rnd]);
            return (LevelTypes)levelToChose[rnd];
        }

        
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


}
