using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Globals : MonoBehaviour
{
    public static bool IsMobile;

    public const float BASE_SPEED = 6f;
    public const float JUMP_POWER = 40f;
    public const float GRAVITY_KOEFF = 2.75f;

    public const float SHADOW_Y_DISTANCE = 8f;

    public const float MASS = 1.5f; //3
    public const float DRAG = 5f; //1
    public const float ANGULAR_DRAG = 5f;

    public const float RAGDOLL_MASS = 0.25f;
    public const float RAGDOLL_DRAG = 1f;
    public const float RAGDOLL_ANGULAR_DRAG = 0.5f;

    public const int LAYER_HELPER = 6;
    public const int LAYER_DANGER = 7;

    public static readonly LayerMask ignoreTriggerMask = LayerMask.GetMask(new string[] { "trigger" });
}

public enum GameTypes
{
    Dont_fall,
    Finish_line
}
