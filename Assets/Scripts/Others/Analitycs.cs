using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analitycs : MonoBehaviour
{
    public static Analitycs Instance { get; private set; }

    private void Awake()
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

    public void Send(string data)
    {

    }
}
