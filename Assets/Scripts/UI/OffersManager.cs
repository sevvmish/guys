using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffersManager : MonoBehaviour
{
    [SerializeField] private GameObject noAdv;
    [SerializeField] private GameObject allSkins;
    [SerializeField] private GameObject starter;
    [SerializeField] private Transform location;
    [SerializeField] private GameObject dailyOffer;

    private bool isReady;

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;

            StartCoroutine(check());            
        }
    }

    private IEnumerator check()
    {
        yield return new WaitForSeconds(1.5f);

        if (!dailyOffer.activeSelf && (DateTime.Now - Globals.TimeWhenStartedPlaying).Minutes > Globals.OFFER_UPDATE)
        {
            ShowOffer();
        }
    }

    private void ShowOffer()
    {
        
        List<int> numbers = new List<int>();
        Globals.TimeWhenStartedPlaying = DateTime.Now;

        if (!Globals.MainPlayerData.AllSkins)
        {
            numbers.Add(2);
        }

        if (!Globals.MainPlayerData.AdvOff)
        {
            numbers.Add(1);
        }

        numbers.Add(3);

        int ID = numbers[UnityEngine.Random.Range(0, numbers.Count)];

        switch (ID)
        {
            case 1: //no adv
                GameObject g = Instantiate(noAdv, location);
                g.SetActive(true);
                g.GetComponent<NoAdsPurchase>().TurnOnCloseButton();
                break;

            case 2: //all skins
                g = Instantiate(allSkins, location);
                g.SetActive(true);
                g.GetComponent<AllSkinsPurchase>().TurnOnCloseButton();
                break;

            case 3: //starter pack
                g = Instantiate(starter, location);
                g.SetActive(true);
                g.GetComponent<StarterPackPurchase>().TurnOnCloseButton();
                break;
        }
    }
}
