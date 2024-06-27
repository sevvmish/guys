using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;
using YG.Utils.Pay;

public class MakePurchase : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private TextMeshProUGUI errorButtonText;
    [SerializeField] private Button errorButton;
    [SerializeField] private GameObject errorPanel;


    private bool isReady;

    private void Start()
    {
        errorPanel.SetActive(false);

        errorButton.onClick.AddListener(() =>
        {
            SoundUI.Instance.PlayUISound(SoundsUI.click);
            errorPanel.SetActive(false);
            SceneManager.LoadScene("MainMenu");
        });
    }


    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;
            YandexGame.PurchaseSuccessEvent = SuccessPurchased;
            YandexGame.PurchaseFailedEvent = FailedPurchased;
            YandexGame.ConsumePurchases();

            errorText.text = Globals.Language.PurchaseError;
            errorButtonText.text = Globals.Language.Close;

            if (Globals.IsAllRestarter)
            {
                Globals.IsAllRestarter = false;
                GetRewardSystem.Instance.ShowEffect(RewardTypes.all_skins, 0);
                GetRewardSystem.Instance.ShowEffect(RewardTypes.all_maps, 0);
                GetRewardSystem.Instance.ShowEffect(RewardTypes.no_adv, 0);
            }

            //StartCoroutine(playTest());
        }

        /*
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(advRestarter());
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Globals.IsDontShowIntro = true;
            Globals.IsAllRestarter = true;
            SceneManager.LoadScene("MainMenu");
        }*/
    }

    private IEnumerator playTest()
    {
        yield return new WaitForSeconds(2);

        //TESTETSTETETS
        Purchase[] purchases = YandexGame.purchases;
        for (int i = 0; i < YandexGame.purchases.Length; i++)
        {
            if (purchases[i] != null)
            {
                Debug.Log(i + ": " + purchases[i].description + " = " + purchases[i].priceValue);
            }
            else
            {
                Debug.Log(i + " is NULL");
            }
        }

        if (purchases.Length == 0) Debug.Log("NO PURCHASE DATA!");

        Purchase p = YandexGame.PurchaseByID("all_skins");
        Debug.Log(p.id + " = " + p.description + " = " + p.priceValue);
    }

    public void Buy(string id)
    {
        YandexGame.BuyPayments(id);
    }

    private void SuccessPurchased(string id)
    {
        SoundUI.Instance.PlayUISound(SoundsUI.cash);

        switch(id)
        {
            case "no_adv":
                
                Globals.MainPlayerData.AdvOff = true;
                SaveLoadManager.Save();
                //Globals.IsDontShowIntro = true;
                //SceneManager.LoadScene("MainMenu");

                StartCoroutine(advRestarter());

                break;

            case "starter":
                Globals.MainPlayerData.G += 500;
                Globals.MainPlayerData.D += 5;
                SaveLoadManager.Save();
                GetRewardSystem.Instance.ShowEffect(RewardTypes.gold, 500);
                GetRewardSystem.Instance.ShowEffect(RewardTypes.gem, 5);
                
                break;

            case "all_maps":
                for (int i = 0; i < Globals.MainPlayerData.LvlA.Length; i++)
                {
                    Globals.MainPlayerData.LvlA[i] = 1;
                }

                Globals.MainPlayerData.AllMaps = true;

                SaveLoadManager.Save();
                //Globals.IsDontShowIntro = true;
                //SceneManager.LoadScene("MainMenu");

                //GetRewardSystem.Instance.ShowEffect(RewardTypes.all_maps, 0);
                StartCoroutine(mapsRestarter());

                break;

            case "all_skins":
                for (int i = (int)Globals.MaleSkins.x; i <= (int)Globals.MaleSkins.y; i++)
                {
                    Globals.MainPlayerData.Skins[i] = 1;
                }

                for (int i = (int)Globals.FemaleSkins.x; i <= (int)Globals.FemaleSkins.y; i++)
                {
                    Globals.MainPlayerData.Skins[i] = 1;
                }
                Globals.MainPlayerData.AllSkins = true;
                SaveLoadManager.Save();
                //Globals.IsDontShowIntro = true;
                //SceneManager.LoadScene("MainMenu");

                //GetRewardSystem.Instance.ShowEffect(RewardTypes.all_skins, 0);
                StartCoroutine(skinsRestarter());

                break;

            case "get_all":


                for (int i = 0; i < Globals.MainPlayerData.LvlA.Length; i++)
                {
                    Globals.MainPlayerData.LvlA[i] = 1;
                }

                Globals.MainPlayerData.AllMaps = true;

                Globals.MainPlayerData.AdvOff = true;

                for (int i = (int)Globals.MaleSkins.x; i <= (int)Globals.MaleSkins.y; i++)
                {
                    Globals.MainPlayerData.Skins[i] = 1;
                }

                for (int i = (int)Globals.FemaleSkins.x; i <= (int)Globals.FemaleSkins.y; i++)
                {
                    Globals.MainPlayerData.Skins[i] = 1;
                }

                Globals.MainPlayerData.AllSkins = true;
                SaveLoadManager.Save();
                Globals.IsDontShowIntro = true;
                Globals.IsAllRestarter = true;
                SceneManager.LoadScene("MainMenu");

                //StartCoroutine(allRestarter());
                
                break;
        }

        YandexMetrica.Send(id);
    }

    
    private IEnumerator advRestarter()
    {
        GetRewardSystem.Instance.ShowEffect(RewardTypes.no_adv, 0);
        yield return new WaitForSeconds(1f);
        Globals.IsDontShowIntro = true;
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator skinsRestarter()
    {
        GetRewardSystem.Instance.ShowEffect(RewardTypes.all_skins, 0);
        yield return new WaitForSeconds(1f);
        Globals.IsDontShowIntro = true;
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator mapsRestarter()
    {
        GetRewardSystem.Instance.ShowEffect(RewardTypes.all_maps, 0);
        yield return new WaitForSeconds(1f);
        Globals.IsDontShowIntro = true;
        SceneManager.LoadScene("MainMenu");
    }


    private void FailedPurchased(string id)
    {
        print("FAILED TO BUY: " + id);
        StartCoroutine(playError());
    }
    private IEnumerator playError()
    {
        yield return new WaitForSeconds(0.3f);
        SoundUI.Instance.PlayUISound(SoundsUI.error);
        errorPanel.SetActive(true);

        yield return new WaitForSeconds(5);
        errorPanel.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

}
