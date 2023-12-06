using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public static BonusManager Instance { get; private set; }
    private Dictionary<int, BonusClient> points = new Dictionary<int, BonusClient>();

    public int GetCurrentValue(int index) => Globals.MainPlayerData.B[index];
    

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
        
    public void AddPoint(int number, BonusClient pointT)
    {
        if (points.ContainsKey(number))
        {
            Debug.LogError("error in point: " + number + " in " + pointT.transform.position);
        }

        points.Add(number, pointT);
    }
}
