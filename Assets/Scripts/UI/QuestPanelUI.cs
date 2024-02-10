using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanelUI : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI sliderText;
    [SerializeField] private Button claimButton;
    [SerializeField] private TextMeshProUGUI claimButtonText;
    [SerializeField] private GameObject toggle;

    [Header("rewards for Q")]
    [SerializeField] private GameObject rewardsPanel;
    [SerializeField] private GameObject goldPanel;
    [SerializeField] private GameObject gemPanel;
    [SerializeField] private GameObject xpPanel;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI gemText;
    [SerializeField] private TextMeshProUGUI xpText;

    private DataForQuests dataForQuest;

    private Quest quest;    
    private int rewardAmountGold;
    private int rewardAmountGem;
    private int rewardAmountXP;
    private bool isRewardTaken;
    private bool isRewardReady;
    private bool isInProgress;

    public void Create(int id, bool isRewardTkn)
    {        
        quest = GetQuestByID(id);
        claimButton.gameObject.SetActive(true);
        toggle.SetActive(false);

        progressSlider.minValue = 0;
        progressSlider.maxValue = 1;

        Title.text = quest.Name;
        Description.text = quest.Description;
        claimButtonText.text = Globals.Language.Take;

        isRewardTaken = isRewardTkn;
        if (isRewardTkn) rewardTaken();

        rewardAmountGold = quest.RewardAmountGold;
        rewardAmountGem = quest.RewardAmountGem;
        rewardAmountXP = quest.RewardAmountXP;

        claimButton.onClick.AddListener(() =>
        {
            claimButton.interactable = false;
            SoundUI.Instance.PlayUISound(SoundsUI.positive);

            if (rewardAmountGold > 0)
            {
                Globals.MainPlayerData.G += rewardAmountGold;
                GetRewardSystem.Instance.ShowEffect(RewardTypes.gold, rewardAmountGold);
            }

            if (rewardAmountGem > 0)
            {
                Globals.MainPlayerData.D += rewardAmountGem;
                GetRewardSystem.Instance.ShowEffect(RewardTypes.gem, rewardAmountGem);
            }

            if (rewardAmountXP > 0)
            {
                Globals.AddXP(rewardAmountXP);

                /*
                GetRewardSystem.Instance.ShowEffect(RewardTypes.xp, rewardAmountXP);

                if (isLvl)
                {
                    GetRewardSystem.Instance.ShowEffect(RewardTypes.newLvl, MainMenu.GetCurrentLevel());
                }*/
            }

            Globals.MainPlayerData.QRT[quest.ID] = 1;
            rewardTaken();
            SaveLoadManager.Save();

        });
    }

    public bool UpdateData(DataForQuests data)
    {
        if (isRewardTaken) return false;
        dataForQuest = data;

        

        switch(quest.ID)
        {
            case 0:
                if (dataForQuest.FirstInFinishGames >= 15)
                {
                    rewardReady();
                    return true;
                }
                else
                {
                    rewardInProgress((float)dataForQuest.FirstInFinishGames / 15f, dataForQuest.FirstInFinishGames + "/15");
                    return false;
                }

            case 1:
                if (dataForQuest.WinnerInDontDie >= 15)
                {
                    rewardReady();
                    return true;
                }
                else
                {
                    rewardInProgress((float)dataForQuest.WinnerInDontDie / 15f, dataForQuest.WinnerInDontDie + "/15");
                    return false;
                }

            case 2:
                if (dataForQuest.AllPlayedGames >= 30)
                {
                    rewardReady();
                    return true;
                }
                else
                {
                    rewardInProgress((float)dataForQuest.AllPlayedGames / 30f, dataForQuest.AllPlayedGames + "/30");
                    return false;
                }

            case 3:
                if (dataForQuest.InThreeInGamesWithFinish >= 15)
                {
                    rewardReady();
                    return true;
                }
                else
                {
                    rewardInProgress((float)dataForQuest.InThreeInGamesWithFinish / 15f, dataForQuest.InThreeInGamesWithFinish + "/15");
                    return false;
                }

            case 4:
                if (dataForQuest.AllPlayedGames >= 20)
                {
                    rewardReady();
                    return true;
                }
                else
                {
                    rewardInProgress((float)dataForQuest.AllPlayedGames / 20f, dataForQuest.AllPlayedGames + "/20");
                    return false;
                }

            case 5:
                if (dataForQuest.GamesWithDontDie >= 10)
                {
                    rewardReady();
                    return true;
                }
                else
                {
                    rewardInProgress((float)dataForQuest.GamesWithDontDie / 10f, dataForQuest.GamesWithDontDie + "/10");
                    return false;
                }

            case 6:
                if (dataForQuest.GamesWithFinish >= 10)
                {
                    rewardReady();
                    return true;
                }
                else
                {
                    rewardInProgress((float)dataForQuest.GamesWithFinish / 10f, dataForQuest.GamesWithFinish + "/10");
                    return false;
                }

            case 7:
                if (dataForQuest.AllPlayedGames >= 10)
                {
                    rewardReady();
                    return true;
                }
                else
                {
                    rewardInProgress((float)dataForQuest.AllPlayedGames / 10f, dataForQuest.AllPlayedGames + "/10");
                    return false;
                }

            case 8:
                if (dataForQuest.WinnerInDontDie >= 3)
                {
                    rewardReady();
                    return true;
                }
                else
                {
                    rewardInProgress((float)dataForQuest.WinnerInDontDie / 3f, dataForQuest.WinnerInDontDie + "/3");
                    return false;
                }

            case 9:
                if (dataForQuest.FirstInFinishGames >= 3)
                {
                    rewardReady();
                    return true;
                }
                else
                {
                    rewardInProgress((float)dataForQuest.FirstInFinishGames / 3f, dataForQuest.FirstInFinishGames + "/3");
                    return false;
                }

        }


        return false;
    }

    private void rewardReady()
    {
        isRewardReady = true;
        isRewardTaken = false;
        isInProgress = false;

        rewardsPanel.SetActive(true);
        claimButton.gameObject.SetActive(true);
        toggle.SetActive(false);
        progressSlider.gameObject.SetActive(false);

        //
        goldPanel.SetActive(false);
        gemPanel.SetActive(false);
        xpPanel.SetActive(false);

        if (rewardAmountGold > 0)
        {
            goldText.text = rewardAmountGold.ToString();
            goldPanel.SetActive(true);
        }

        if (rewardAmountGem > 0)
        {
            gemText.text = rewardAmountGem.ToString();
            gemPanel.SetActive(true);
        }

        if (rewardAmountXP > 0)
        {
            xpText.text = rewardAmountXP.ToString();
            xpPanel.SetActive(true);
        }
    }

    private void rewardInProgress(float koeff, string koeffDescription)
    {
        isRewardReady = false;
        isRewardTaken = false;
        isInProgress = true;
                
        rewardsPanel.SetActive(false);
        claimButton.gameObject.SetActive(false);
        toggle.SetActive(false);
        progressSlider.gameObject.SetActive(true);
        progressSlider.value = koeff;
        sliderText.text = koeffDescription;
    }

    private void rewardTaken()
    {
        isRewardReady = false;
        isRewardTaken = true;
        isInProgress = false;

        rewardsPanel.SetActive(false);
        claimButton.gameObject.SetActive(false);
        toggle.SetActive(true);
        progressSlider.gameObject.SetActive(false);
    }

    public static Quest GetQuestByID(int id)
    {
        switch (id)
        {
            case 0:
                return new Quest(id, 1, Globals.Language.Quest0Name, Globals.Language.Quest0Descr, 0,3,0);

            case 1:
                return new Quest(id, 1, Globals.Language.Quest1Name, Globals.Language.Quest1Descr, 0,2,0);

            case 2:
                return new Quest(id, 1, Globals.Language.Quest2Name, Globals.Language.Quest2Descr, 0,1,0);

            case 3:
                return new Quest(id, 1, Globals.Language.Quest3Name, Globals.Language.Quest3Descr, 80, 0, 100);

            case 4:
                return new Quest(id, 1, Globals.Language.Quest4Name, Globals.Language.Quest4Descr, 80, 0, 100);

            case 5:
                return new Quest(id, 1, Globals.Language.Quest5Name, Globals.Language.Quest5Descr, 80, 0, 100);

            case 6:
                return new Quest(id, 1, Globals.Language.Quest6Name, Globals.Language.Quest6Descr, 80, 0, 100);

            case 7:
                return new Quest(id, 1, Globals.Language.Quest7Name, Globals.Language.Quest7Descr, 50, 0, 70);

            case 8:
                return new Quest(id, 1, Globals.Language.Quest8Name, Globals.Language.Quest8Descr, 50, 0, 70);

            case 9:
                return new Quest(id, 1, Globals.Language.Quest9Name, Globals.Language.Quest9Descr, 50, 0, 70);
        }


        return new Quest();
    }
}


public struct Quest
{
    public int ID;
    public int Raiting;
    public string Name;
    public string Description;
    public int RewardAmountGold;
    public int RewardAmountGem;
    public int RewardAmountXP;

    public Quest(int iD, int raiting, string name, string description, int rewardAmountGold, int rewardAmountGem, int rewardAmountXP)
    {
        ID = iD;
        Raiting = raiting;
        Name = name;
        Description = description;
        RewardAmountGold = rewardAmountGold;
        RewardAmountGem = rewardAmountGem;
        RewardAmountXP = rewardAmountXP;
    }
}

public struct DataForQuests
{
    public int AllPlayedGames;
    public int GamesWithFinish;
    public int GamesWithDontDie;
    public int FirstInFinishGames;
    public int WinnerInDontDie;
    public int InThreeInGamesWithFinish;

    public DataForQuests(
        int allPlayedGames, 
        int gamesWithFinish, 
        int gamesWithDontDie, 
        int firstInFinishGames, 
        int winnerInDontDie,
        int inThreeFinish
        )
    {
        AllPlayedGames = allPlayedGames;
        GamesWithFinish = gamesWithFinish;
        GamesWithDontDie = gamesWithDontDie;
        FirstInFinishGames = firstInFinishGames;
        WinnerInDontDie = winnerInDontDie;
        InThreeInGamesWithFinish = inThreeFinish;
    }
}