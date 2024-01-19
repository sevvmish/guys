using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeactivateOnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] toActivate;
    [SerializeField] private GameObject[] toDeActivate;

    
    private void OnTriggerEnter(Collider other)
    {
        if (toActivate.Length > 0)
        {
            for (int i = 0; i < toActivate.Length; i++)
            {
                toActivate[i].SetActive(true);
            }
        }

        if (toDeActivate.Length > 0)
        {
            for (int i = 0; i < toDeActivate.Length; i++)
            {
                toDeActivate[i].SetActive(false);
            }
        }
    }
}
