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
                return Resources.Load<GameObject>("PlayerTemplate");

            case Skins.pomni:
                return Resources.Load<GameObject>("Skins/Pomni LP");

            case Skins.civilian_male_1:
                g = Resources.Load<GameObject>("Skins/male skins");
                g.GetComponent<MaleSkinsManager>().SetSkin(Skins.civilian_male_1);
                return g;
        }
        return null;
    }
}

public enum Skins
{
    main_player_template,
    pomni,
    civilian_male_1
}
