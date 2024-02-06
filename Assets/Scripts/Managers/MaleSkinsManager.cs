using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaleSkinsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] civilian1;
    [SerializeField] private GameObject[] civilian2;
    [SerializeField] private GameObject[] civilian3;
    [SerializeField] private GameObject[] civilian4;
    [SerializeField] private GameObject[] civilian5;
    [SerializeField] private GameObject[] civilian6;

    [SerializeField] private GameObject[] civilianForGold0;
    [SerializeField] private GameObject[] civilianForGold1;
    [SerializeField] private GameObject[] civilianForGold2;
    [SerializeField] private GameObject[] civilianForGold3;
    [SerializeField] private GameObject[] civilianForGold4;
    [SerializeField] private GameObject[] civilianForGold5;

    [SerializeField] private GameObject[] civilianForGem1;
    [SerializeField] private GameObject[] civilianForGem2;
    [SerializeField] private GameObject[] civilianForGem3;
    [SerializeField] private GameObject[] civilianForGem4;
    [SerializeField] private GameObject[] civilianForGem5;


    [SerializeField] private GameObject[] female1;
    [SerializeField] private GameObject[] female2;
    [SerializeField] private GameObject[] female3;
    [SerializeField] private GameObject[] female4;
    [SerializeField] private GameObject[] female5;

    [SerializeField] private GameObject[] femaleGold1;
    [SerializeField] private GameObject[] femaleGold2;
    [SerializeField] private GameObject[] femaleGold3;
    [SerializeField] private GameObject[] femaleGold4;
    [SerializeField] private GameObject[] femaleGold5;

    [SerializeField] private GameObject[] femaleGem1;
    [SerializeField] private GameObject[] femaleGem2;
    [SerializeField] private GameObject[] femaleGem3;
    [SerializeField] private GameObject[] femaleGem4;
    [SerializeField] private GameObject[] femaleGem5;

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

            case Skins.civilian_male_4:
                makeActiveModel(civilian4);
                break;

            case Skins.civilian_male_5:
                makeActiveModel(civilian5);
                break;

            case Skins.civilian_male_6:
                makeActiveModel(civilian6);
                break;

            case Skins.civilian_male_gold_0:
                makeActiveModel(civilianForGold0);
                break;

            case Skins.civilian_male_gold_1:
                makeActiveModel(civilianForGold1);
                break;

            case Skins.civilian_male_gold_2:
                makeActiveModel(civilianForGold2);
                break;

            case Skins.civilian_male_gold_3:
                makeActiveModel(civilianForGold3);
                break;

            case Skins.civilian_male_gold_4:
                makeActiveModel(civilianForGold4);
                break;

            case Skins.civilian_male_gold_5:
                makeActiveModel(civilianForGold5);
                break;

            case Skins.civilian_male_gem_1:
                makeActiveModel(civilianForGem1);
                break;

            case Skins.civilian_male_gem_2:
                makeActiveModel(civilianForGem2);
                break;

            case Skins.civilian_male_gem_3:
                makeActiveModel(civilianForGem3);
                break;

            case Skins.civilian_male_gem_4:
                makeActiveModel(civilianForGem4);
                break;

            case Skins.civilian_male_gem_5:
                makeActiveModel(civilianForGem5);
                break;




            case Skins.civilian_female_1:
                makeActiveModel(female1);
                break;

            case Skins.civilian_female_2:
                makeActiveModel(female2);
                break;

            case Skins.civilian_female_3:
                makeActiveModel(female3);
                break;

            case Skins.civilian_female_4:
                makeActiveModel(female4);
                break;

            case Skins.civilian_female_5:
                makeActiveModel(female5);
                break;

            case Skins.civilian_female_gold_1:
                makeActiveModel(femaleGold1);
                break;

            case Skins.civilian_female_gold_2:
                makeActiveModel(femaleGold2);
                break;

            case Skins.civilian_female_gold_3:
                makeActiveModel(femaleGold3);
                break;

            case Skins.civilian_female_gold_4:
                makeActiveModel(femaleGold4);
                break;

            case Skins.civilian_female_gold_5:
                makeActiveModel(femaleGold5);
                break;

            case Skins.civilian_female_gem_1:
                makeActiveModel(femaleGem1);
                break;

            case Skins.civilian_female_gem_2:
                makeActiveModel(femaleGem2);
                break;

            case Skins.civilian_female_gem_3:
                makeActiveModel(femaleGem3);
                break;

            case Skins.civilian_female_gem_4:
                makeActiveModel(femaleGem4);
                break;

            case Skins.civilian_female_gem_5:
                makeActiveModel(femaleGem5);
                break;
        }
    }

    public GameObject[] GetSkin(Skins skin)
    {
        
        switch (skin)
        {
            case Skins.civilian_male_1:
                return civilian1;
                

            case Skins.civilian_male_2:
                return civilian2;

            case Skins.civilian_male_3:
                return civilian3;

            case Skins.civilian_male_4:
                return civilian4;

            case Skins.civilian_male_5:
                return civilian5;

            case Skins.civilian_male_6:
                return civilian6;

            case Skins.civilian_male_gold_0:
                return civilianForGold0;

            case Skins.civilian_male_gold_1:
                return civilianForGold1;

            case Skins.civilian_male_gold_2:
                return civilianForGold2;

            case Skins.civilian_male_gold_3:
                return civilianForGold3;

            case Skins.civilian_male_gold_4:
                return civilianForGold4;

            case Skins.civilian_male_gold_5:
                return civilianForGold5;

            case Skins.civilian_male_gem_1:
                return civilianForGem1;

            case Skins.civilian_male_gem_2:
                return civilianForGem2;

            case Skins.civilian_male_gem_3:
                return civilianForGem3;

            case Skins.civilian_male_gem_4:
                return civilianForGem4;

            case Skins.civilian_male_gem_5:
                return civilianForGem5;




            case Skins.civilian_female_1:
                return female1;

            case Skins.civilian_female_2:
                return female2;

            case Skins.civilian_female_3:
                return female3;

            case Skins.civilian_female_4:
                return female4;

            case Skins.civilian_female_5:
                return female5;

            case Skins.civilian_female_gold_1:
                return femaleGold1;

            case Skins.civilian_female_gold_2:
                return femaleGold2;

            case Skins.civilian_female_gold_3:
                return femaleGold3;

            case Skins.civilian_female_gold_4:
                return femaleGold4;

            case Skins.civilian_female_gold_5:
                return femaleGold5;

            case Skins.civilian_female_gem_1:
                return femaleGem1;

            case Skins.civilian_female_gem_2:
                return femaleGem2;

            case Skins.civilian_female_gem_3:
                return femaleGem3;

            case Skins.civilian_female_gem_4:
                return femaleGem4;

            case Skins.civilian_female_gem_5:
                return femaleGem5;
        }

        return new GameObject[0];
    }

    private void makeActiveModel(GameObject[] obj)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].SetActive(true);
        }
    }
}
