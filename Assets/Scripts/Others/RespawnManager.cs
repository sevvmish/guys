using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance { get; private set; }

    private Dictionary<int, RespawnPoint> points = new Dictionary<int, RespawnPoint>();

    public int GetCurrentIndex { 
        get 
        {
            switch(GameManager.Instance.GetLevelManager().GetCurrentLevelType())
            {
                case LevelTypes.circus1:
                    return Globals.MainPlayerData.M1;
            }

            return 0;
        } 
    }

    // Start is called before the first frame update
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

    private void Start()
    {
        
        GameManager.Instance.GetMainPlayerTransform().position = points[Globals.CurrentRespawnPointOnMap].transform.position;
        GameManager.Instance.GetMainPlayerTransform().eulerAngles = new Vector3(0, points[Globals.CurrentRespawnPointOnMap].transform.eulerAngles.y, 0);
    }

    public void AddPoint(int number, RespawnPoint pointT)
    {
        if (points.ContainsKey(number))
        {
            Debug.LogError("error in point: " + number + " in " + pointT.transform.position);
        }

        points.Add(number, pointT);
    }
}
