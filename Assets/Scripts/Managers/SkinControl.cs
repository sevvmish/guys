using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinControl : MonoBehaviour
{
    public CapsuleCollider[] ragdollColliders;
    public Animator _animator;
    public GameObject RocketPack;
    public GameObject[] Ski;
    public GameObject[] ModelMesh;

    public static GameObject GetSkinGameobject(Skins skin)
    {
        GameObject g = default;
        /*
        if (skin == Skins.main_player_template)
        {
            return Instantiate(Resources.Load<GameObject>("PlayerTemplate"));
        }

        g = Instantiate(Resources.Load<GameObject>("Skins/male/old_male_01"));
        return g;
        //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_1);
        */
        
        switch(skin)
        {
            case Skins.main_player_template:
                return Instantiate(Resources.Load<GameObject>("PlayerTemplate"));

            //case Skins.pomni:
            //    return Instantiate(Resources.Load<GameObject>("Skins/Pomni LP"));

            case Skins.civilian_male_1:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/char2_3d_male"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_1);
                return g;

            case Skins.civilian_male_2:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/old_male_02"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_2);
                return g;

            case Skins.civilian_male_3:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/char2male1"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_3);
                return g;

            case Skins.civilian_male_4:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/char11_3d_male"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_4);
                return g;

            case Skins.civilian_male_5:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/old_male_03"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_5);
                return g;

            case Skins.civilian_male_6:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/char2male2"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_6);
                return g;

            case Skins.civilian_male_gold_0:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/old_male_04"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gold_0);
                return g;

            case Skins.civilian_male_gold_1:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/char71_3d_male"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gold_1);
                return g;

            case Skins.civilian_male_gold_2:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/old_male_01"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gold_2);
                return g;

            case Skins.civilian_male_gold_3:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/char56_3d_male"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gold_3);
                return g;

            case Skins.civilian_male_gold_4:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/char35_3d_male"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gold_4);
                return g;

            case Skins.civilian_male_gold_5:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/char83_3d_male"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gold_5);
                return g;

            case Skins.civilian_male_gem_1:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/char77_3d_male"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gem_1);
                return g;

            case Skins.civilian_male_gem_2:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/char70_3d_male"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gem_2);
                return g;

            case Skins.civilian_male_gem_3:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/char76_3d_male"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gem_3);
                return g;

            case Skins.civilian_male_gem_4:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/char86_3d_male"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gem_4);
                return g;

            case Skins.civilian_male_gem_5:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/char53_3d_all"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gem_5);
                return g;

            case Skins.civilian_male_gem_6:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/char78_3d_all"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_gem_5);
                return g;

            case Skins.civilian_male_gem_7:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/old_male_05"));
                return g;








            case Skins.civilian_female_1:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/char7_3d_female"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_1);
                return g;

            case Skins.civilian_female_2:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/old_female_03"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_2);
                return g;

            case Skins.civilian_female_3:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/char2fem1"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_3);
                return g;

            case Skins.civilian_female_4:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/char32_3d_female"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_4);
                return g;

            case Skins.civilian_female_5:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/char109_3d_female"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_5);
                return g;

            case Skins.civilian_female_gold_1:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/char2fem4"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gold_1);
                return g;

            case Skins.civilian_female_gold_2:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/char103_3d_female"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gold_2);
                return g;

            case Skins.civilian_female_gold_3:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/char2fem2"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gold_3);
                return g;

            case Skins.civilian_female_gold_4:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/char2fem3"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gold_4);
                return g;

            case Skins.civilian_female_gold_5:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/old_female_02"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gold_5);
                return g;

            case Skins.civilian_female_gem_1:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/char12_3d_female"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gem_1);
                return g;

            case Skins.civilian_female_gem_2:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/old_female_01"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gem_2);
                return g;

            case Skins.civilian_female_gem_3:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/char43_3d_female"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gem_3);
                return g;

            case Skins.civilian_female_gem_4:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/char47_3d_female"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gem_4);
                return g;

            case Skins.civilian_female_gem_5:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/char57_3d_female"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gem_5);
                return g;

            case Skins.civilian_female_gem_6:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/char60_3d_female"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gem_5);
                return g;

            case Skins.civilian_female_gem_7:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/char62_3d_female"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gem_5);
                return g;

            case Skins.civilian_female_gem_8:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/char77_3d_female"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gem_5);
                return g;

            case Skins.civilian_female_gem_9:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/charXX_3d_all"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gem_5);
                return g;

            case Skins.civilian_female_gem_10:
                g = Instantiate(Resources.Load<GameObject>("Skins/female/char69_3d_female"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_female_gem_5);
                return g;




            default:
                g = Instantiate(Resources.Load<GameObject>("Skins/male/char2_3d_male"));
                //g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_1);
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
    civilian_male_gem_6,
    civilian_male_gem_7,





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
    civilian_female_gem_5,

    civilian_female_gem_6,
    civilian_female_gem_7,
    civilian_female_gem_8,
    civilian_female_gem_9,
    civilian_female_gem_10

}
