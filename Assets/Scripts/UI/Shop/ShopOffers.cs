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
        GameObject g1 = Instantiate(somethingForAdv, shopLocation);
        g1.SetActive(true);
        g1.GetComponent<SomethingForAdv>().SetData(RewardForAdv.Gold);

        g1 = Instantiate(somethingForAdv, shopLocation);
        g1.SetActive(true);
        g1.GetComponent<SomethingForAdv>().SetData(RewardForAdv.XP);

        g1 = Instantiate(starter, shopLocation);
        g1.SetActive(true);

        if (!Globals.MainPlayerData.AllMaps && !Globals.ShopPurchases.Any(p => p.tag == "all_maps"))
        {
            GameObject g = Instantiate(allMaps, shopLocation);
            g.SetActive(true);
        }

        if (!Globals.MainPlayerData.AdvOff && !Globals.ShopPurchases.Any(p => p.tag == "no_adv"))
        {
            GameObject g = Instantiate(noAdv, shopLocation);
            g.SetActive(true);
        }

        if (!Globals.MainPlayerData.AllSkins && !Globals.ShopPurchases.Any(p => p.tag == "all_skins"))
        {
            GameObject g = Instantiate(allSkins, shopLocation);
            g.SetActive(true);
        }

        if (!Globals.MainPlayerData.AllSkins && !Globals.MainPlayerData.AdvOff && !Globals.MainPlayerData.AllMaps && !Globals.ShopPurchases.Any(p => p.tag == "get_all"))
        {
            GameObject g = Instantiate(getAll, shopLocation);
            g.SetActive(true);
        }
    }

}
