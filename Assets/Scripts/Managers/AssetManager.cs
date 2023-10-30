using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    [SerializeField] private GameObject testPlayer;

    public GameObject GetPlayerSkin(PLayerSkin _skin)
    {
        switch ( _skin )
        {
            case PLayerSkin.test:
                return testPlayer;
        }

        return null;
    }
}

public enum PLayerSkin
{
    test
}
