using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinControl : MonoBehaviour
{
    public CapsuleCollider[] ragdollColliders;
    public Animator _animator;

    public static GameObject GetSkinGameobject(Skins skin)
    {
        switch(skin)
        {
            case Skins.main_player_template:
                return Resources.Load<GameObject>("PlayerTemplate");

            case Skins.pomni:
                return Resources.Load<GameObject>("Skins/Pomni LP");
        }
        return null;
    }
}

public enum Skins
{
    main_player_template,
    pomni
}
