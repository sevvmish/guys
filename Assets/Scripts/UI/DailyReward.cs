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

    private int rewardLimit = 12;
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

            for (int i = 0; i < rewardLimit; i++)
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
                        g.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+"+ drt.Amount.ToString();
                        break;

                    case DailyRewardTypes.RewardsTypes.Gem:
                        g.transform.GetChild(3).gameObject.SetActive(true);
                        g.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + drt.Amount.ToString();
                        break;

                    case DailyRewardTypes.RewardsTypes.Level:
                        g.transform.GetChild(4).gameObject.SetActive(true);
                        break;
                }
            }

            if (Globals.MainPlayerData.DR == 0 || Mathf.Abs(DateTime.Now.Day - Globals.MainPlayerData.LDR) > 0)
            {
                StartCoroutine(playShow());
            }

            getPrize.onClick.AddListener(() =>
            {
                SoundUI.Instance.PlayUISound(SoundsUI.cash);
                

                switch (currentDailyReward.RewardType)
                {
                    case DailyRewardTypes.RewardsTypes.Gold:
                        Globals.MainPlayerData.G += currentDailyReward.Amount;
                        break;

                    case DailyRewardTypes.RewardsTypes.Gem:
                        Globals.MainPlayerData.D += currentDailyReward.Amount;
                        break;

                    case DailyRewardTypes.RewardsTypes.Level:

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
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 20);

            case 1:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 35);

            case 2:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 50);

            case 3:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gem, 1);

            case 4:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 80);

            case 5:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 100);

            case 6:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 150);

            case 7:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gem, 5);

            case 8:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 200);

            case 9:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 230);

            case 10:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gold, 270);

            case 11:
                return new DailyRewardTypes(DailyRewardTypes.RewardsTypes.Gem, 10);

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
        Level
    }

    public RewardsTypes RewardType;
    public int Amount;

    public DailyRewardTypes(RewardsTypes rewardType, int amount)
    {
        RewardType = rewardType;
        Amount = amount;
    }
}
