using GamePush;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MakePurchase : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private TextMeshProUGUI errorButtonText;
    [SerializeField] private Button errorButton;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private ShopOffers shopOffers;


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

    private void OnEnable()
    {
        GP_Payments.OnFetchProducts += OnFetchProducts;
        GP_Payments.OnFetchProductsError += OnFetchProductsError;
        GP_Payments.OnFetchPlayerPurchases += OnFetchPlayerPurchases;
    }
    //Отписка от событий
    private void OnDisable()
    {
        GP_Payments.OnFetchProducts -= OnFetchProducts;
        GP_Payments.OnFetchProductsError -= OnFetchProductsError;
        GP_Payments.OnFetchPlayerPurchases -= OnFetchPlayerPurchases;
    }

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;
            //YandexGame.PurchaseSuccessEvent = SuccessPurchased;
            //YandexGame.PurchaseFailedEvent = FailedPurchased;
            //YandexGame.ConsumePurchases();

            errorText.text = Globals.Language.PurchaseError;
            errorButtonText.text = Globals.Language.Close;

            if (Globals.IsAllRestarter)
            {
                Globals.IsAllRestarter = false;
                GetRewardSystem.Instance.ShowEffect(RewardTypes.all_skins, 0);
                GetRewardSystem.Instance.ShowEffect(RewardTypes.all_maps, 0);
                GetRewardSystem.Instance.ShowEffect(RewardTypes.no_adv, 0);
            }

            if (!GP_Payments.IsPaymentsAvailable())
            {
                GP_Payments.Fetch();
            }
        }
    }

    
        

    public void Buy(string id)
    {
        //FetchProducts product = Globals.ShopProducts.FirstOrDefault(p => p.tag == id);

        GP_Payments.Purchase(id, OnPurchaseSuccess, FailedPurchased);
        consume(id);
    }

    

    private void OnPurchaseSuccess(string productIdOrTag)
    {
        SuccessPurchased(productIdOrTag);
    }

    private void consume(string id) => GP_Payments.Consume(id, OnConsumeSuccess, OnConsumeError);

    private void OnConsumeSuccess(string productIdOrTag) => Debug.Log("CONSUME: SUCCESS: " + productIdOrTag);
    
    private void OnConsumeError() => Debug.Log("CONSUME: ERROR");




    private void SuccessPurchased(string id)
    {
        SoundUI.Instance.PlayUISound(SoundsUI.cash);

        switch(id)
        {
            case "no_adv":
                
                Globals.MainPlayerData.AdvOff = true;
                SaveLoadManager.Save();
                

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
                StartCoroutine(startMainMenu());
                //SceneManager.LoadScene("MainMenu");

                
                break;
        }

        Analitycs.Instance.Send(id);
    }

    private IEnumerator startMainMenu()
    {        
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("MainMenu");
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


    private void FailedPurchased()
    {
        //print("FAILED TO BUY: " + id);
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

    private void OnFetchPlayerPurchases(List<FetchPlayerPurchases> purcahses)
    {
        for (int i = 0; i < purcahses.Count; i++)
        {
            Debug.Log("PLAYER PURCHASES: PRODUCT TAG: " + purcahses[i].tag);
            Debug.Log("PLAYER PURCHASES: PRODUCT ID: " + purcahses[i].productId);
            Debug.Log("PLAYER PURCHASES: PAYLOAD: " + purcahses[i].payload);
            Debug.Log("PLAYER PURCHASES: CREATED AT: " + purcahses[i].createdAt);
            Debug.Log("PLAYER PURCHASES: EXPIRED AT: " + purcahses[i].expiredAt);
            Debug.Log("PLAYER PURCHASES: GIFT: " + purcahses[i].gift);
            Debug.Log("PLAYER PURCHASES: SUBSCRIBED: " + purcahses[i].subscribed);
        }
    }

    private void OnFetchProducts(List<FetchProducts> products)
    {
        Globals.ShopProducts = products;

        for (int i = 0; i < products.Count; i++)
        {
            Debug.Log("PRODUCT: ID: " + products[i].id);
            Debug.Log("PRODUCT: TAG: " + products[i].tag);
            Debug.Log("PRODUCT: NAME: " + products[i].name);
            Debug.Log("PRODUCT: DESCRIPTION: " + products[i].description);
            Debug.Log("PRODUCT: ICON: " + products[i].icon);
            Debug.Log("PRODUCT: ICON SMALL: " + products[i].iconSmall);
            Debug.Log("PRODUCT: PRICE: " + products[i].price);
            Debug.Log("PRODUCT: CURRENCY: " + products[i].currency);
            Debug.Log("PRODUCT: CURRENCY SYMBOL: " + products[i].currencySymbol);
            Debug.Log("PRODUCT: IS SUBSCRIPTION: " + products[i].isSubscription);
            Debug.Log("PRODUCT: PERIOD: " + products[i].period);
            Debug.Log("PRODUCT: TRIAL PERIOD: " + products[i].trialPeriod);
        }

        if (products.Count > 0)
        {
            shopOffers.ShowProducts();
        }
    }
    // Ошибки при получении
    private void OnFetchProductsError()
    {
        Debug.Log("FETCH PRODUCTS: ERROR");
    }

}
