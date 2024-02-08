using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffersManager : MonoBehaviour
{
    [SerializeField] private GameObject noAdv;
    [SerializeField] private GameObject allSkins;
    [SerializeField] private GameObject starter;
    [SerializeField] private GameObject getAll;
    [SerializeField] private GameObject allMaps;
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

        if (!dailyOffer.activeSelf && (DateTime.Now - Globals.TimeWhenStartedPlaying).TotalMinutes > Globals.OFFER_UPDATE)
        {
            ShowOffer();
        }
        else
        {
            print("NO offer - " + dailyOffer.activeSelf + " - " + (DateTime.Now - Globals.TimeWhenStartedPlaying).TotalMinutes);
        }
    }

    private void ShowOffer()
    {
        print("offer show started");
        List<int> numbers = new List<int>();
        Globals.TimeWhenStartedPlaying = DateTime.Now;

        if (!Globals.MainPlayerData.AllSkins && !Globals.MainPlayerData.OM.Contains(2))
        {
            numbers.Add(2);
        }

        if (!Globals.MainPlayerData.AllMaps && !Globals.MainPlayerData.OM.Contains(5))
        {
            numbers.Add(5);
        }

        if ((!Globals.MainPlayerData.AllSkins || !Globals.MainPlayerData.AdvOff || !Globals.MainPlayerData.AllMaps) && !Globals.MainPlayerData.OM.Contains(4))
        {
            numbers.Add(4);
        }

        if (!Globals.MainPlayerData.AdvOff && !Globals.MainPlayerData.OM.Contains(1))
        {
            numbers.Add(1);
        }

        if (!Globals.MainPlayerData.OM.Contains(3)) numbers.Add(3);

        if (numbers.Count == 0) return;

        int ID = numbers[UnityEngine.Random.Range(0, numbers.Count)];
        Globals.MainPlayerData.OM = Globals.MainPlayerData.OM.Append(ID).ToArray();
        SaveLoadManager.Save();

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

            case 4: //get all
                g = Instantiate(getAll, location);
                g.SetActive(true);
                g.GetComponent<GetAllPurchase>().TurnOnCloseButton();
                break;

            case 5: //all maps
                g = Instantiate(allMaps, location);
                g.SetActive(true);
                g.GetComponent<AllMapsPurchase>().TurnOnCloseButton();
                break;
        }
    }
}
