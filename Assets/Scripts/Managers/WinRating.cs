using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WinRating
{
    public List<GameSessionResult> Results = new List<GameSessionResult>();

    public void AddResult(GameSessionResult result)
    {
        Results.Add(result);
    }

    public float GetWinRating()
    {
        return 1;
    }
}

[Serializable]
public struct GameSessionResult
{
    public LevelTypes LT;
    public GameTypes GT;
    public int Pl;

    public GameSessionResult(LevelTypes levelType, GameTypes gameType, int place)
    {
        LT = levelType;
        GT = gameType;
        Pl = place;
    }
}
