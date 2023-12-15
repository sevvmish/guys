using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Circus1Optimyzer : MonoBehaviour
{
    [Header("mobile movement")]
    [SerializeField] private GameObject MovementForMobile;
    [SerializeField] private TextMeshPro MovementLeftJTexter;

    [Header("mobile jump")]
    [SerializeField] private GameObject JumpForMobile;
    [SerializeField] private TextMeshPro JumpRightJTexter;

    [Header("PC jump")]
    [SerializeField] private GameObject JumpForPC;
    [SerializeField] private TextMeshPro JumpPCTexter;

    [Header("double jump")]
    [SerializeField] private GameObject DoubleJump;
    [SerializeField] private TextMeshPro DoubleJumpText;

    [Header("Camera helper PC")]
    [SerializeField] private GameObject CameraInfoPC;
    [SerializeField] private TextMeshPro CameraInfoPCTexter;

    [Header("Camera helper mobile")]
    [SerializeField] private GameObject CameraInfoMobile;
    [SerializeField] private TextMeshPro CameraInfoMobileTexter;

    [Header("Dont forget double jump")]
    [SerializeField] private GameObject DontForgetDJ;
    [SerializeField] private TextMeshPro DontForgetDJTexter;

    [Header("PC movement")]
    [SerializeField] private GameObject MovementForPC;
    [SerializeField] private TextMeshPro MovementKeyboard;
    [SerializeField] private TextMeshPro MovementLeftW;
    [SerializeField] private TextMeshPro MovementLeftA;
    [SerializeField] private TextMeshPro MovementLeftS;
    [SerializeField] private TextMeshPro MovementLeftD;

    [SerializeField] private GameObject part1;
    [SerializeField] private GameObject part2;
    [SerializeField] private GameObject part3;
    [SerializeField] private GameObject part4;
    [SerializeField] private GameObject part5;
    [SerializeField] private GameObject part6;
    [SerializeField] private GameObject part7;
    [SerializeField] private GameObject part8;
    [SerializeField] private GameObject part9;

    private int previousPos;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        MovementForMobile.SetActive(false);
        MovementForPC.SetActive(false);
        JumpForMobile.SetActive(false);
        JumpForPC.SetActive(false);
        DoubleJump.SetActive(false);
        CameraInfoPC.SetActive(false);
        CameraInfoMobile.SetActive(false);
        DontForgetDJ.SetActive(false);

        part1.SetActive(false);
        part2.SetActive(false);
        part3.SetActive(false);
        part4.SetActive(false);
        part5.SetActive(false);
        part6.SetActive(false);
        part7.SetActive(false);
        part8.SetActive(false);
        part9.SetActive(false);

        yield return new WaitForSeconds(0.05f);

        int currentPos = RespawnManager.Instance.GetCurrentIndex;
        previousPos = currentPos;
        checkState(currentPos);

    }

    private void Update()
    {
        if (RespawnManager.Instance.GetCurrentIndex != previousPos)
        {
            previousPos = RespawnManager.Instance.GetCurrentIndex;
            print(previousPos);
            checkState(previousPos);
        }
    }

    private void checkState(int pos)
    {        
        if (pos < 4)
        {
            if (Globals.IsMobile)
            {
                MovementForMobile.SetActive(true);
                MovementLeftJTexter.text = Globals.Language.MovementHintLeftJ;

                JumpForMobile.SetActive(true);
                JumpRightJTexter.text = Globals.Language.JumpHintRightJ;

                CameraInfoMobile.SetActive(true);
                CameraInfoMobileTexter.text = Globals.Language.CameraHintMobile;
            }
            else
            {
                MovementForPC.SetActive(true);
                MovementLeftW.text = Globals.Language.UpArrowLetter;
                MovementLeftS.text = Globals.Language.DownArrowLetter;
                MovementLeftA.text = Globals.Language.LeftArrowLetter;
                MovementLeftD.text = Globals.Language.RightArrowLetter;
                MovementKeyboard.text = Globals.Language.MovementHintLetters;

                JumpForPC.SetActive(true);
                JumpPCTexter.text = Globals.Language.JumpHintKeyboard;

                CameraInfoPC.SetActive(true);
                CameraInfoPCTexter.text = Globals.Language.CameraHintPC;
            }

            DoubleJump.SetActive(true);
            DoubleJumpText.text = Globals.Language.DoubleJumpHint;

            DontForgetDJ.SetActive(true);
            DontForgetDJTexter.text = Globals.Language.DontForgetDoubleJump;


            if (!part1.activeSelf) part1.SetActive(true);
            if (!part2.activeSelf) part2.SetActive(true);
            if (part3.activeSelf) part3.SetActive(false);
            if (part4.activeSelf) part4.SetActive(false);
        }
        else if (pos >= 4 && pos < 5)
        {
            MovementForMobile.SetActive(false);
            MovementForPC.SetActive(false);
            JumpForMobile.SetActive(false);
            JumpForPC.SetActive(false);
            DoubleJump.SetActive(false);

            if (!part1.activeSelf) part1.SetActive(true);
            if (!part2.activeSelf) part2.SetActive(true);
            if (!part3.activeSelf) part3.SetActive(true);
            if (part4.activeSelf) part4.SetActive(false);
        }
        else if (pos >= 5 && pos < 6)
        {
            if (part1.activeSelf) part1.SetActive(false);
            if (!part2.activeSelf) part2.SetActive(true);
            if (!part3.activeSelf) part3.SetActive(true); 
            if (!part4.activeSelf) part4.SetActive(true);
        }
        else if (pos >= 6 && pos < 9)
        {            
            if (part2.activeSelf) part2.SetActive(false);
            if (!part3.activeSelf) part3.SetActive(true);
            if (!part4.activeSelf) part4.SetActive(true);            
        }
        else if (pos >= 9 && pos < 11)
        {
            if (part2.activeSelf) part2.SetActive(false);
            if (!part3.activeSelf) part3.SetActive(true);
            if (!part4.activeSelf) part4.SetActive(true);
            if (!part5.activeSelf) part5.SetActive(true);
        }
        else if (pos >= 11 && pos < 14)
        {            
            if (part3.activeSelf) part3.SetActive(false);
            if (!part4.activeSelf) part4.SetActive(true);
            if (!part5.activeSelf) part5.SetActive(true);
            if (!part6.activeSelf) part6.SetActive(true);
        }
        else if (pos >= 14 && pos < 16)
        {            
            if (part4.activeSelf) part4.SetActive(false);
            if (!part5.activeSelf) part5.SetActive(true);
            if (!part6.activeSelf) part6.SetActive(true);
            
        }
        else if (pos >= 16 && pos < 20)
        {            
            if (part5.activeSelf) part5.SetActive(false);
            if (!part6.activeSelf) part6.SetActive(true);
            if (!part7.activeSelf) part7.SetActive(true);
        }
        else if (pos >= 20 && pos < 22)
        {
            if (part5.activeSelf) part5.SetActive(false);
            if (part6.activeSelf) part6.SetActive(false);
            if (!part7.activeSelf) part7.SetActive(true);
            if (!part8.activeSelf) part8.SetActive(true);
        }
        else if (pos >= 22 && pos < 23)
        {            
            if (part6.activeSelf) part6.SetActive(false);
            if (part7.activeSelf) part7.SetActive(false);
            if (!part8.activeSelf) part8.SetActive(true);
            if (!part9.activeSelf) part9.SetActive(true);
        }
        else if (pos >= 23 && pos<24)
        {   
            if (part7.activeSelf) part7.SetActive(false);
            if (!part8.activeSelf) part8.SetActive(true);
            if (!part9.activeSelf) part9.SetActive(true);
        }
        else if (pos >= 24)
        {            
            if (!part9.activeSelf) part9.SetActive(true);
        }
    }

    
}
