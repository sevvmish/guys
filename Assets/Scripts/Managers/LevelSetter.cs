using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSetter : MonoBehaviour
{
        
    [SerializeField] private GameObject mapExample;
        
    [SerializeField] private TextMeshProUGUI bonusText;
    [SerializeField] private TextMeshProUGUI bonusExplainText;
    
    [SerializeField] private Transform location;
    [SerializeField] private Transform locationMain;
    private RectTransform locationRect;

    List<MapUI> maps = new List<MapUI>();

    private LevelTypes levelToPlay;
    private int howManyLevels;

    private bool isBonus;
    
    // Start is called before the first frame update
    void Start()
    {
        bonusText.transform.parent.gameObject.SetActive(false);
        bonusExplainText.transform.parent.gameObject.SetActive(false);
        bonusText.gameObject.SetActive(true);
        bonusExplainText.gameObject.SetActive(true);

        if (Globals.IsMusicOn)
        {
            int ambMusic = UnityEngine.Random.Range(0, 3);
            switch (ambMusic)
            {
                case 0:
                    AmbientMusic.Instance.PlayAmbient(AmbientMelodies.loop_melody3);
                    break;

                case 1:
                    AmbientMusic.Instance.PlayAmbient(AmbientMelodies.loop_melody4);
                    break;

                case 2:
                    AmbientMusic.Instance.PlayAmbient(AmbientMelodies.loop_melody5);
                    break;
            }
        }
        


        for (int i = 1; i < Globals.MainPlayerData.LvlA.Length; i++)
        {
            //if (Globals.MainPlayerData.LvlA[i] < 1) continue;
            GameObject map = Instantiate(mapExample, location);
            map.SetActive(true);
            maps.Add(map.GetComponent<MapUI>());
        }

        locationRect = location.GetComponent<RectTransform>();

        if (Globals.LevelsPlayedForBonusCountAmount >= 4)
        {
            Globals.LevelsPlayedForBonusCountAmount = 0;

            List<int> hidenLvls = new List<int>();
            for (int i = 1; i < Globals.MainPlayerData.LvlA.Length; i++)
            {
                if (Globals.MainPlayerData.LvlA[i] == 0 && !Globals.LevelsPlayedForBonusCount.Contains(i))
                {
                    hidenLvls.Add(i);
                }
            }

            if (hidenLvls.Count > 1)
            {
                int lvlToShow = hidenLvls[UnityEngine.Random.Range(0, hidenLvls.Count - 1)];
                levelToPlay = (LevelTypes)lvlToShow;                
                Globals.LevelsPlayedForBonusCount.Add(lvlToShow);
                isBonus = true;
            }
            else
            {
                levelToPlay = getNextLevel();
            }
        }
        else
        {
            levelToPlay = getNextLevel();
        }

                
        updateMapData();
    }

    private LevelTypes getNextLevel()
    {
        if (MainMenu.GetCurrentLevel() == 1) return LevelTypes.level1;

        Dictionary<LevelTypes, int> levels = new Dictionary<LevelTypes, int>();

        for (int i = 1; i < Globals.MainPlayerData.LvlA.Length; i++)
        {
            if (Globals.MainPlayerData.LvlA[i] > 0 && (int)Globals.LastPlayedLevel != i)
            {
                levels.Add((LevelTypes)i, 0);
            }
        }

        if (Globals.MainPlayerData.WR.Length > 0)
        {
            List<GameSessionResult> results = new List<GameSessionResult>(Globals.MainPlayerData.WR);
            for (int i = 0; i < results.Count; i++)
            {
                if (levels.ContainsKey(results[i].LT))
                {
                    levels[results[i].LT]++;
                }
                /*
                else
                {
                    levels.Add(results[i].LT, 1);
                }*/
            }
        }

        int min = int.MaxValue;
        LevelTypes result = LevelTypes.level1;

        foreach (LevelTypes item in levels.Keys)
        {
            if (levels[item] < min)
            {
                min = levels[item];
                result = item;
            }
        }

        List<LevelTypes> preResults = new List<LevelTypes>();

        foreach (LevelTypes item in levels.Keys)
        {
            if (levels[item] == min)
            {
                preResults.Add(item);
            }
        }

        int rnd = UnityEngine.Random.Range(0, preResults.Count);
        result = preResults[rnd];
        print("after second row result: " + result);

        return result;

    }

    private void updateMapData()
    {
        for (int i = 1; i < Globals.MainPlayerData.LvlA.Length; i++)
        {
            if (Globals.MainPlayerData.LvlA[i] == 1)
            {
                maps[i - 1].SetMap(LevelManager.GetLevelData((LevelTypes)i), true, false);
                howManyLevels++;
            }
            else
            {
                maps[i - 1].SetMap(LevelManager.GetLevelData((LevelTypes)i), false, false);
                howManyLevels++;
                //continue;
            }
        }

        StartCoroutine(playShow());
         
    }
    private IEnumerator playShow()
    {
        locationRect.anchoredPosition = new Vector2(8000, 0);

        int howLong = howManyLevels > 6 ? 6 : howManyLevels;
        //locationRect.anchoredPosition = Vector2.zero;
        locationRect.DOAnchorPos(new Vector2(locationRect.anchoredPosition.x - 2800 - /*1300*/800 * (howManyLevels-1) - 2800, locationRect.anchoredPosition.y), howLong).SetEase(Ease.Linear);

        float waiter = howLong > 4 ? 4 : howLong;

        yield return new WaitForSeconds(waiter);

        if (isBonus)
        {
            bonusText.transform.parent.gameObject.SetActive(true);
            bonusExplainText.transform.parent.gameObject.SetActive(true);
            bonusText.text = Globals.Language.BonusText;
            bonusExplainText.text = Globals.Language.BonusExplainText;
        }

        GameObject map = Instantiate(mapExample, locationMain);
        map.SetActive(true);
        LevelData data = LevelManager.GetLevelData(levelToPlay);
        map.GetComponent<MapUI>().SetMap(data, true, false);
        //map.GetComponent<RectTransform>().anchoredPosition = new Vector2(1000, 500);
        map.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        map.transform.localScale = Vector3.zero;
        float scaleKoeff = Globals.IsMobile ? 1.35f : 1.5f;
        map.transform.DOScale(Vector3.one * scaleKoeff, 0.2f).SetEase(Ease.InOutFlash);

        SoundUI.Instance.PlayUISound(SoundsUI.success);

        yield return new WaitForSeconds(3);

        map.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutFlash);
        //yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene(data.LevelInInspector);
    }


}
