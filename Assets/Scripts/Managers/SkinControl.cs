using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinControl : MonoBehaviour
{
    public CapsuleCollider[] ragdollColliders;
    public Animator _animator;

    public static GameObject GetSkinGameobject(Skins skin)
    {
        GameObject g = default;

        switch(skin)
        {
            case Skins.main_player_template:
                return Instantiate(Resources.Load<GameObject>("PlayerTemplate"));

            case Skins.pomni:
                return Instantiate(Resources.Load<GameObject>("Skins/Pomni LP"));

            case Skins.civilian_male_1:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_1);
                return g;

            case Skins.civilian_male_2:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_2);
                return g;

            case Skins.civilian_male_3:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_3);
                return g;

            case Skins.civilian_male_4:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_4);
                return g;

            case Skins.civilian_male_5:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_5);
                return g;

            case Skins.civilian_male_6:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_6);
                return g;

            case Skins.civilian_male_gold_0:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gold_0);
                return g;

            case Skins.civilian_male_gold_1:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gold_1);
                return g;

            case Skins.civilian_male_gold_2:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gold_2);
                return g;

            case Skins.civilian_male_gold_3:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gold_3);
                return g;

            case Skins.civilian_male_gold_4:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gold_4);
                return g;

            case Skins.civilian_male_gold_5:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gold_5);
                return g;

            case Skins.civilian_male_gem_1:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gem_1);
                return g;

            case Skins.civilian_male_gem_2:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gem_2);
                return g;

            case Skins.civilian_male_gem_3:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gem_3);
                return g;

            case Skins.civilian_male_gem_4:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gem_4);
                return g;

            case Skins.civilian_male_gem_5:
                g = Instantiate(Resources.Load<GameObject>("Skins/male skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gem_5);
                return g;








            case Skins.civilian_female_1:
                g = Instantiate(Resources.Load<GameObject>("Skins/female skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_1);
                return g;

            case Skins.civilian_female_2:
                g = Instantiate(Resources.Load<GameObject>("Skins/female skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_2);
                return g;

            case Skins.civilian_female_3:
                g = Instantiate(Resources.Load<GameObject>("Skins/female skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_3);
                return g;

            case Skins.civilian_female_4:
                g = Instantiate(Resources.Load<GameObject>("Skins/female skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_4);
                return g;

            case Skins.civilian_female_5:
                g = Instantiate(Resources.Load<GameObject>("Skins/female skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_5);
                return g;

            case Skins.civilian_female_gold_1:
                g = Instantiate(Resources.Load<GameObject>("Skins/female skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gold_1);
                return g;

            case Skins.civilian_female_gold_2:
                g = Instantiate(Resources.Load<GameObject>("Skins/female skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gold_2);
                return g;

            case Skins.civilian_female_gold_3:
                g = Instantiate(Resources.Load<GameObject>("Skins/female skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gold_3);
                return g;

            case Skins.civilian_female_gold_4:
                g = Instantiate(Resources.Load<GameObject>("Skins/female skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gold_4);
                return g;

            case Skins.civilian_female_gold_5:
                g = Instantiate(Resources.Load<GameObject>("Skins/female skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gold_5);
                return g;

            case Skins.civilian_female_gem_1:
                g = Instantiate(Resources.Load<GameObject>("Skins/female skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gem_1);
                return g;

            case Skins.civilian_female_gem_2:
                g = Instantiate(Resources.Load<GameObject>("Skins/female skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gem_2);
                return g;

            case Skins.civilian_female_gem_3:
                g = Instantiate(Resources.Load<GameObject>("Skins/female skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gem_3);
                return g;

            case Skins.civilian_female_gem_4:
                g = Instantiate(Resources.Load<GameObject>("Skins/female skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gem_4);
                return g;

            case Skins.civilian_female_gem_5:
                g = Instantiate(Resources.Load<GameObject>("Skins/female skins"));
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gem_5);
                return g;
        }
        return null;
    }
}

public enum Skins
{
    main_player_template,
    pomni,
    civilian_male_1,
    civilian_male_2,
    civilian_male_3,
    civilian_male_4,
    civilian_male_5,
    civilian_male_6,
    civilian_male_gold_0,
    civilian_male_gold_1,
    civilian_male_gold_2,
    civilian_male_gold_3,
    civilian_male_gold_4,
    civilian_male_gold_5,
    civilian_male_gem_1,
    civilian_male_gem_2,
    civilian_male_gem_3,
    civilian_male_gem_4,
    civilian_male_gem_5,




    civilian_female_1 = 25,
    civilian_female_2,
    civilian_female_3,
    civilian_female_4,
    civilian_female_5,
    civilian_female_gold_1,
    civilian_female_gold_2,
    civilian_female_gold_3,
    civilian_female_gold_4,
    civilian_female_gold_5,
    civilian_female_gem_1,
    civilian_female_gem_2,
    civilian_female_gem_3,
    civilian_female_gem_4,
    civilian_female_gem_5
}
