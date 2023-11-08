using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPointSystem : MonoBehaviour
{
    public static NavPointSystem Instance { get; private set; }

    private List<BotNavPoint> navPoints = new List<BotNavPoint>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public BotNavPoint GetBotNavPoint(int index)
    {
        if (navPoints.Count > 0)
        {
            for (int i = 0; i < navPoints.Count; i++)
            {
                if (navPoints[i].Index > index)
                {
                    return navPoints[i];
                }
            }            
        }

        return null;
    }

    public void AddPoint(BotNavPoint _point)
    {
        if (!navPoints.Contains(_point))
        {
            navPoints.Add(_point);
        }
    }

    public void RemovePoint(BotNavPoint _point)
    {
        if (navPoints.Contains(_point))
        {
            navPoints.Remove(_point);
        }
    }
}
