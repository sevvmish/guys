using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class OffersManager : MonoBehaviour
{
    [SerializeField] private GameObject noAdv;
    [SerializeField] private GameObject allSkins;
    [SerializeField] private GameObject starter;
    [SerializeField] private GameObject getAll;
    [SerializeField] private GameObject allMaps;
    [SerializeField] private Transform location;
    [SerializeField] private GameObject dailyOffer;

    [Header("Tutorial")]
    [SerializeField] private GameObject progressHint;
    [SerializeField] private TextMeshProUGUI progressHintText;
    [SerializeField] private GameObject questHint;
    [SerializeField] private TextMeshProUGUI questHintText;

    private bool isReady;

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;

            StartCoroutine(check());

            progressHintText.text = Globals.Language.progressHint;
            questHintText.text = Globals.Language.questHint;
        }

        /*
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject g = Instantiate(starter, location);
            g.SetActive(true);
            g.GetComponent<StarterPackPurchase>().TurnOnCloseButton();
        }
        */
        

    }

    private IEnumerator check()
    {
        yield return new WaitForSeconds(1.5f);

        if (!dailyOffer.activeSelf )
        {
            if (!Globals.MainPlayerData.Tut1)
            {
                Globals.MainPlayerData.Tut1 = true;
                SaveLoadManager.Save();
                progressHint.SetActive(true);
            }
            else if (!Globals.MainPlayerData.Tut2)
            {
                Globals.MainPlayerData.Tut2 = true;
                SaveLoadManager.Save();
                questHint.SetActive(true);
            }
            else if ((DateTime.Now - Globals.TimeWhenStartedPlaying).TotalMinutes > Globals.OFFER_UPDATE)
            {
                ShowOffer();
            }            
        }     
        else
        {
            if (!Globals.MainPlayerData.Tut1 || !Globals.MainPlayerData.Tut2)
            {
                StartCoroutine(playTutAfterDailyRew());
            }
        }
    }
    private IEnumerator playTutAfterDailyRew()
    {
        for (float i = 0; i < 20; i+=0.2f)
        {
            if (!dailyOffer.activeSelf) break;
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1);

        if (!Globals.MainPlayerData.Tut1)
        {
            Globals.MainPlayerData.Tut1 = true;
            SaveLoadManager.Save();
            progressHint.SetActive(true);
        }
        else if (!Globals.MainPlayerData.Tut2)
        {
            Globals.MainPlayerData.Tut2 = true;
            SaveLoadManager.Save();
            questHint.SetActive(true);
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
