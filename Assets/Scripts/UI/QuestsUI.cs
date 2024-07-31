using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsUI : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private GameObject back;
    [SerializeField] private RectTransform mainRect;
    [SerializeField] private Transform location;
    [SerializeField] private GameObject questExample;

    [SerializeField] private RectTransform forResize1;
    [SerializeField] private RectTransform forResize2;

    private bool isReady;
    private List<QuestPanelUI> quests = new List<QuestPanelUI>();

    // Start is called before the first frame update
    void Start()
    {
        back.SetActive(false);
        mainMenu.OnBackToMainMenu += ReturnBack;
        questExample.SetActive(false);
    }

    public void SetOn()
    {
        back.SetActive(true);
        
        mainMenu.MainPlayerSkin.SetActive(false);
        UpdateData();
        if (Globals.IsShowQuestNotification)
        {
            Globals.IsShowQuestNotification = false;
        }
    }

    public void UpdateData()
    {
        DataForQuests data = getDataForQuests();
        
        for (int i = 0; i < quests.Count; i++)
        {
            bool isShow = quests[i].UpdateData(data);

            if (isShow)
            {
                Globals.IsShowQuestNotification = true;
            }
        }

        location.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1000);
    }

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;

            for (int i = (Globals.MainPlayerData.QRT.Length-1); i >= 0; i--)
            {
                GameObject g = Instantiate(questExample, location);
                g.SetActive(true);
                QuestPanelUI q = g.GetComponent<QuestPanelUI>();
                q.Create(i, Globals.MainPlayerData.QRT[i] == 1);
                quests.Add(q);
            }

            UpdateData();

        }
    }

    private DataForQuests getDataForQuests()
    {
        int allGames = 0;
        int gamesWithFinish = 0;
        int gamesWithDontDie = 0;
        int firstInGamesWithFinish = 0;
        int winInDontDie = 0;
        int inThreeInGamesWithFinish = 0;

        if (Globals.MainPlayerData.WR.Length > 0)
        {
            for (int i = 0; i < Globals.MainPlayerData.WR.Length; i++)
            {
                allGames++;

                if (Globals.MainPlayerData.WR[i].GT == GameTypes.Finish_line) //finish
                {
                    gamesWithFinish++;
                    if (Globals.MainPlayerData.WR[i].Pl == 1)
                    {
                        firstInGamesWithFinish++;
                    }

                    if (Globals.MainPlayerData.WR[i].Pl > 1 && Globals.MainPlayerData.WR[i].Pl <= 3)
                    {
                        inThreeInGamesWithFinish++;
                    }
                }
                else if (Globals.MainPlayerData.WR[i].GT == GameTypes.Dont_fall) //dont fall
                {
                    gamesWithDontDie++;
                    if (Globals.MainPlayerData.WR[i].Pl == 1)
                    {
                        winInDontDie++;
                    }
                }

            }
        }

        //print("all: " + allGames + ", finish: " + gamesWithFinish + ", dont die all: " + gamesWithDontDie + ", firstInFin: " + firstInGamesWithFinish + ", winDontD: " + winInDontDie + ", inThree: " + inThreeInGamesWithFinish);

        return new DataForQuests(allGames, gamesWithFinish, gamesWithDontDie, firstInGamesWithFinish, winInDontDie, inThreeInGamesWithFinish);
        
    }

    private void ReturnBack()
    {
        back.SetActive(false);
        mainMenu.BackToMainMenu();
    }

    
}


