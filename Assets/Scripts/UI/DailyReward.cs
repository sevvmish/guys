using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
    [SerializeField] private MenuOptions mainMenu;
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject panelExample;
    [SerializeField] private Transform location;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI getPrizeText;

    [SerializeField] private Button getPrize;

    
    private bool isReady;
    private DailyRewardTypes currentDailyReward;

    // Start is called before the first frame update
    void Start()
    {        
        back.SetActive(false);
        panelExample.SetActive(false);
                
    }

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;
            titleText.text = Globals.Language.DailyRewards;
            getPrizeText.text = Globals.Language.Take;

            int cur = Globals.MainPlayerData.DR;
            currentDailyReward = GetDailyReward(cur);

            int fromLimit = 0;
            int rewardLimit = 12;

            if (cur >= 12 && cur < 24)
            {
                fromLimit = 12;
                rewardLimit = 24;
            }
            else if (cur >= 24)
            {
                fromLimit = 24;
                rewardLimit = 36;
            }

            for (int i = fromLimit; i < rewardLimit; i++)
            {
                GameObject g = Instantiate(panelExample, location);
                g.SetActive(true);
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Globals.Language.Day + " " + (i + 1).ToString();

                if (Globals.MainPlayerData.DR > 0 && i < Globals.MainPlayerData.DR)
                {
                    g.transform.GetChild(5).gameObject.SetActive(true);
                    g.transform.GetChild(6).gameObject.SetActive(true);
                }
                else if (i == Globals.MainPlayerData.DR)
                {
                    g.transform.localScale = Vector3.one * 1.2f;
                    g.transform.GetChild(7).gameObject.SetActive(true);
                }

                DailyRewardTypes drt = GetDailyReward(i);
                switch(drt.RewardType)
                {
                    case DailyRewardTypes.RewardsTypes.Gold:
                        g.transform.GetChild(2).gameObject.SetActive(true);
                        g.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = drt.Amount.ToString();
                        break;

                    case DailyRewardTypes.RewardsTypes.Gem:
                        g.transform.GetChild(3).gameObject.SetActive(true);
                        g.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = drt.Amount.ToString();
                        break;

                    case DailyRewardTypes.RewardsTypes.XP:
                        g.transform.GetChild(4).gameObject.SetActive(true);
                        g.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = drt.Amount.ToString();
                        break;
                }
            }

            if (Globals.MainPlayerData.DR < 36 && (Globals.MainPlayerData.DR == 0 || Mathf.Abs(DateTime.Now.Day - Globals.MainPlayerData.LDR) > 0))
            {
                StartCoroutine(playShow());
            }

            getPrize.onClick.AddListener(() =>
            {                
                switch (currentDailyReward.RewardType)
                {
                    case DailyRewardTypes.RewardsTypes.Gold:
                        Globals.MainPlayerData.G += currentDailyReward.Amount;
                        GetRewardSystem.Instance.ShowEffect(RewardTypes.gold, currentDailyReward.Amount);
                        break;

                    case DailyRewardTypes.RewardsTypes.Gem:
                        Globals.MainPlayerData.D += currentDailyReward.Amount;
                        GetRewardSystem.Instance.ShowEffect(RewardTypes.gem, currentDailyReward.Amount);
                        break;

                    case DailyRewardTypes.RewardsTypes.XP:
                        Globals.AddXP(currentDailyReward.Amount);
                        
                        break;
                }

                Globals.MainPlayerData.DR++;
                Globals.MainPlayerData.LDR = DateTime.Now.Day;
                SaveLoadManager.Save();

                mainMenu.UpdateCurrencyData();
                back.SetActive(false);
            });
        }
    }
    private IEnumerator playShow()
    {
        yield return new WaitForSeconds(1f);
        back.SetActive(true);
        back.transform.localScale = Vector3.zero;
        back.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutQuad);
    }

    
    public static DailyRewardTypes GetDailyReward(int day)
    {


        switch(day)
        {
            case 0:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 30);

            case 1:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 50);

            case 2:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.XP, 100);

            case 3:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gem, 1);

            case 4:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 80);

            case 5:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 100);

            case 6:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.XP, 200);

            case 7:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gem, 3);

            case 8:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 130);

            case 9:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 150);

            case 10:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.XP, 300);

            case 11:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gem, 5);





            case 12:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 180);

            case 13:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 220);

            case 14:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.XP, 400);

            case 15:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gem, 7);

            case 16:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 250);

            case 17:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 300);

            case 18:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.XP, 500);

            case 19:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gem, 8);

            case 20:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 350);

            case 21:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 400);

            case 22:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.XP, 600);

            case 23:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gem, 10);






            case 24:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 700);

            case 25:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 850);

            case 26:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.XP, 700);

            case 27:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gem, 12);

            case 28:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 1000);

            case 29:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 1200);

            case 30:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.XP, 850);

            case 31:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gem, 14);

            case 32:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 1500);

            case 33:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 1700);

            case 34:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.XP, 1000);

            case 35:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gem, 17);

        }


        return new DailyRewardTypes();
    }
}

public struct DailyRewardTypes
{
    public enum RewardsTypes
    {
        Gold,
        Gem,
        Level,
        XP
    }

    public RewardsTypes RewardType;
    public int Amount;

    public DailyRewardTypes(RewardsTypes rewardType, int amount)
    {
        RewardType = rewardType;
        Amount = amount;
    }
}
