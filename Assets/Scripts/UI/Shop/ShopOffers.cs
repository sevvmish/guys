using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopOffers : MonoBehaviour
{
    [SerializeField] private GameObject noAdv;
    [SerializeField] private GameObject allSkins;
    [SerializeField] private GameObject starter;
    [SerializeField] private GameObject getAll;
    [SerializeField] private GameObject allMaps;
    [SerializeField] private GameObject somethingForAdv;
    [SerializeField] private Transform shopLocation;

    private bool isReady;

    


    public void ShowProducts()
    {
        print("started showing products!");

        GameObject g1 = Instantiate(somethingForAdv, shopLocation);
        g1.SetActive(true);
        g1.GetComponent<SomethingForAdv>().SetData(RewardForAdv.Gold);

        g1 = Instantiate(somethingForAdv, shopLocation);
        g1.SetActive(true);
        g1.GetComponent<SomethingForAdv>().SetData(RewardForAdv.XP);

        g1 = Instantiate(starter, shopLocation);
        g1.SetActive(true);

        if (Globals.ShopPurchases.Count > 0)
        {
            if (Globals.ShopPurchases.Any(p => p.tag == "all_maps"))
            {
                Globals.MainPlayerData.AllMaps = true;
            }

            if (Globals.ShopPurchases.Any(p => p.tag == "no_adv"))
            {
                Globals.MainPlayerData.AdvOff = true;
            }

            if (Globals.ShopPurchases.Any(p => p.tag == "all_skins"))
            {
                Globals.MainPlayerData.AllSkins = true;
            }

            if (Globals.ShopPurchases.Any(p => p.tag == "get_all"))
            {
                Globals.MainPlayerData.AllMaps = true;
                Globals.MainPlayerData.AdvOff = true;
                Globals.MainPlayerData.AllSkins = true;
            }
        }

        if (!Globals.MainPlayerData.AllMaps)
        {
            GameObject g = Instantiate(allMaps, shopLocation);
            g.SetActive(true);
        }

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

        if (!Globals.MainPlayerData.AllSkins && !Globals.MainPlayerData.AdvOff && !Globals.MainPlayerData.AllMaps)
        {
            GameObject g = Instantiate(getAll, shopLocation);
            g.SetActive(true);
        }
    }

}
