using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;

    private bool isReady;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;

        }
    }

}
