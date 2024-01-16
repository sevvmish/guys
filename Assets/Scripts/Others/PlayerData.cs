using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{   
    public string L;
    public int M;
    public int S;
    public int Mus;

    public int D;
    public int G;

    public int[] Skins;
    public int CS;

    public float Zoom;

    public bool AdvOff;
    public bool AllSkins;

    public int DR;
    public int LDR;

    public PlayerData()
    {        
        L = ""; //prefered language
        M = 1; //mobile platform? 1 - true;
        S = 1; // sound on? 1 - true;        
        Mus = 1; // music
        Zoom = 0; //camera zoom

        //currencies
        D = 0; //Diamonds collected
        G = 0; //Gold collected

        //skins
        Skins = new int[50];
        int randomSkinSex = UnityEngine.Random.Range(0, 2);

        switch(randomSkinSex)
        {
            case 0:
                int skin = UnityEngine.Random.Range(2, 6);
                Skins[skin] = 1;
                CS = skin;
                break;

            case 1:
                skin = UnityEngine.Random.Range(25, 29);
                Skins[skin] = 1;
                CS = skin;
                break;
        }

        AdvOff = false;
        AllSkins = false;

        LDR = 0; //last number of day when reward was
        DR = 0; //how many daily rewards

        Debug.Log("created PlayerData instance");
    }


}
