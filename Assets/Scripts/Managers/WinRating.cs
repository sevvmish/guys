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
    public int GameType;
    public int LevelType;
    public int Place;

    public GameSessionResult(int gameType, int levelType, int place)
    {
        GameType = gameType;
        LevelType = levelType;
        Place = place;
    }
}
