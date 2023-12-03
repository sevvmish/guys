using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Globals : MonoBehaviour
{
    public static PlayerData MainPlayerData;
    public static bool IsSoundOn;
    public static bool IsMusicOn;
    public static bool IsInitiated;
    public static string CurrentLanguage;
    public static Translation Language;

    //public static int CurrentMapCircus = 0;
    //public static int CurrentRespawnPointOnMap = 0;

    public static DateTime TimeWhenStartedPlaying;
    public static DateTime TimeWhenLastInterstitialWas;
    public static DateTime TimeWhenLastRewardedWas;

    public static GameTypes CurrentGameType;

    public static bool IsMobile;
    public static bool IsDevelopmentBuild = true;

    public const float BASE_SPEED = 7f;
    public const float JUMP_POWER = 40f;
    public const float GRAVITY_KOEFF = 2.75f;

    public const float SHADOW_Y_DISTANCE = 8f;

    public const float MASS = 1.5f; //3
    public const float DRAG = 5f; //1
    public const float ANGULAR_DRAG = 5f;

    public const float RAGDOLL_MASS = 0.25f;
    public const float RAGDOLL_DRAG = 1f;
    public const float RAGDOLL_ANGULAR_DRAG = 0.5f;

    public const float MOUSE_X_SENS = 26f;
    public const float MOUSE_Y_SENS = 13f;

    public const float MAX_HIT_IMPULSE_MAGNITUDE = 60f;
    public const float MIN_HIT_IMPULSE_MAGNITUDE = 20f;

    public const int LAYER_HELPER = 6;
    public const int LAYER_DANGER = 7;
    public const int LAYER_PLAYER = 9;

    public const float ZOOM_DELTA = 0.22f;
    public const float ZOOM_LIMIT = 6f;

    public const float SCREEN_SAVER_AWAIT = 1;

    public const float PLAYERS_COLLIDE_FORCE = 7f;

    public static readonly Vector3 BasePosition = new Vector3(0, 5.5f, -8);
    public static readonly Vector3 BaseRotation = new Vector3(25, 0, 0);

    public static readonly LayerMask ignoreTriggerMask = LayerMask.GetMask(new string[] { "trigger", "player", "ragdoll" });
}

public enum GameTypes
{
    Dont_fall,
    Finish_line
}
