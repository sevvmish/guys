using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ShopOffers : MonoBehaviour
{
    [SerializeField] private GameObject noAdv;
    [SerializeField] private GameObject allSkins;
    [SerializeField] private GameObject starter;
    [SerializeField] private Transform shopLocation;

    private bool isReady;

    private void Start()
    {
        noAdv.SetActive(false);
        allSkins.SetActive(false);
    }

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;

            GameObject g1 = Instantiate(starter, shopLocation);
            g1.SetActive(true);

            if (!Globals.MainPlayerData.AdvOff)
            {
                GameObject g = Instantiate(noAdv, shopLocation);
                g.SetActive(true);
            }

            if (!Globals.MainPlayerData.AllSkins)
            {
                GameObject g = Instantiate(allSkins, shopLocation);
                g.SetActive(true);
            }
        }
    }

}
