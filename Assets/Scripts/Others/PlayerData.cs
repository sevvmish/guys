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
    public int CM;
    public int M1;
    public int MP;

    public int Hint1;

    public float Zoom;

    public PlayerData()
    {        
        L = ""; //prefered language
        M = 1; //mobile platform? 1 - true;
        S = 1; // sound on? 1 - true;        
        Mus = 1; // music
        CM = 0; // current map type
        M1 = 0; //map1 progress
        Hint1 = 0; //tutrial 1 with double jump
        Zoom = 0; //camera zoom
        Debug.Log("created PlayerData instance");
    }


}
