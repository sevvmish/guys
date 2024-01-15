using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class MakePurchase : MonoBehaviour
{
    private bool isReady;


    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;
            YandexGame.PurchaseSuccessEvent = SuccessPurchased;
            YandexGame.PurchaseFailedEvent = FailedPurchased;
            YandexGame.ConsumePurchases();
        }
    }

    public void Buy(string id)
    {
        YandexGame.BuyPayments(id);
    }

    private void SuccessPurchased(string id)
    {
        print("OK TO BUY: " + id);

        if (id == "test")
        {
            print("TEST OK");
        }
    }

    private void FailedPurchased(string id)
    {
        print("FAILED TO BUY: " + id);
    }
}
