using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Globals : MonoBehaviour
{
    public static GameTypes CurrentGameType;

    public static bool IsMobile;
    public static bool IsDevelopmentBuild = true;

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

    public const float SCREEN_SAVER_AWAIT = 1;

    public static readonly Vector3 BasePosition = new Vector3(0, 6, -8);
    public static readonly Vector3 BaseRotation = new Vector3(30, 0, 0);

    public static readonly LayerMask ignoreTriggerMask = LayerMask.GetMask(new string[] { "trigger", "player", "ragdoll" });
}

public enum GameTypes
{
    Dont_fall,
    Finish_line
}
