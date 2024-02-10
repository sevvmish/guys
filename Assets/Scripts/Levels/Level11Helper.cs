using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level11Helper : MonoBehaviour
{
    [SerializeField] private TextMeshPro Description;

    private bool isReady;

    
    // Update is called once per frame
    void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;

            Description.text = Globals.Language.DontForgetDoubleJump;
        }
    }
}
