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
    public LevelTypes LevelType;
    public GameTypes GameType;
    public int Place;

    public GameSessionResult(LevelTypes levelType, GameTypes gameType, int place)
    {
        LevelType = levelType;
        GameType = gameType;
        Place = place;
    }
}
