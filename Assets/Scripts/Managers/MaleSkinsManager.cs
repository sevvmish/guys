using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaleSkinsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] civilian1;
    [SerializeField] private GameObject[] civilian2;
    [SerializeField] private GameObject[] civilian3;

    public Skins CurrentSkin { get; private set; }

    public void SetSkin(Skins skin)
    {
        CurrentSkin = skin;

        switch (skin)
        {            
            case Skins.civilian_male_1:
                makeActiveModel(civilian1);
                break;

            case Skins.civilian_male_2:
                makeActiveModel(civilian2);
                break;

            case Skins.civilian_male_3:
                makeActiveModel(civilian3);
                break;
        }
    }

    private void makeActiveModel(GameObject[] obj)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].SetActive(true);
        }
    }
}
