using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Translations", menuName = "Languages", order = 1)]
public class Translation : ScriptableObject
{
    public string UpArrowLetter;
    public string DownArrowLetter;
    public string LeftArrowLetter;
    public string RightArrowLetter;
    public string JumpLetter;
    public string DoubleJumpHint;
    public string CameraHintPC;
    public string CameraHintMobile;
    public string DontForgetDoubleJump;

    public string MovementHintLeftJ;
    public string MovementHintLetters;
    public string JumpHintRightJ;
    public string JumpHintKeyboard;

    public string CameraScalerInfo;

    public string Saved;
    public string PlusBonus;
    public string SkipLevelOffer;
    public string SkipLevelConfirmation;
    public string PressButtonWhenSkipLevel;

    public string Play;
    public string Options;
    public string Continue;
    public string Reset;
    public string ResetInfoText;

    public Translation() { }
}
