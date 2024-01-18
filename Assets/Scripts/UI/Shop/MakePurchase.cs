using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

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
        }
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
                Globals.IsDontShowIntro = true;
                SceneManager.LoadScene("MainMenu");
                break;

            case "starter":
                Globals.MainPlayerData.G += 1000;
                Globals.MainPlayerData.D += 20;
                SaveLoadManager.Save();
                GetRewardSystem.Instance.ShowEffect(RewardTypes.gold, 1000);
                GetRewardSystem.Instance.ShowEffect(RewardTypes.gem, 20);
                //Globals.IsDontShowIntro = true;
                //SceneManager.LoadScene("MainMenu");
                break;

            case "all_skins":
                for (int i = 2; i <= 18; i++)
                {
                    Globals.MainPlayerData.Skins[i] = 1;
                }

                for (int i = 25; i <= 39; i++)
                {
                    Globals.MainPlayerData.Skins[i] = 1;
                }
                Globals.MainPlayerData.AllSkins = true;
                SaveLoadManager.Save();
                Globals.IsDontShowIntro = true;
                SceneManager.LoadScene("MainMenu");
                break;
        }
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
    }

}
