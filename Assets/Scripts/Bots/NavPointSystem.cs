using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    public BotNavPoint GetBotNavPoint(int index, Vector3 pos)
    {
        if (navPoints.Count > 0)
        {
            int minIndex = 100000;
            int number = 0;
            SortedDictionary<float, BotNavPoint> points = new SortedDictionary<float, BotNavPoint> ();
            
            for (int i = 0; i < navPoints.Count; i++)
            {
                float currDistance = (pos - navPoints[i].transform.position).magnitude;
                if (navPoints[i].Index > index && navPoints[i].IsActive && navPoints[i].Index <= minIndex)
                {
                    number = i;
                    minIndex = navPoints[i].Index;
                    if (!points.ContainsKey(currDistance)) points.Add(currDistance, navPoints[i]);
                }                
            }

            if (points.Count > 0)
            {
                return points.First().Value;
            }
            else
            {
                return null;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            for (int i = 0; i < navPoints.Count; i++)
            {
                print(i + ": " + navPoints[i].Index);
            }
        }
    }
}
