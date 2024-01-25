using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TranslateGO : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;

    private bool isReady;

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;
            text.text = Globals.Language.GoGoGo;
        }
    }

}
