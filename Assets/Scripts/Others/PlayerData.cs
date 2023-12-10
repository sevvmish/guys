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
    public int[] B;

    public float Zoom;

    public PlayerData()
    {        
        L = ""; //prefered language
        M = 1; //mobile platform? 1 - true;
        S = 1; // sound on? 1 - true;        
        Mus = 1; // music
        CM = 0; // current map type
        M1 = -1; //map1 progress
        Zoom = 0; //camera zoom
        B = new int[20]; //bonuses collected
        Debug.Log("created PlayerData instance");
    }


}
