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



    civilian_female_1 = 25,

}
