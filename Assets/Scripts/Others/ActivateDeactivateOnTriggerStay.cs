using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeactivateOnTriggerStay : MonoBehaviour
{
    [SerializeField] private GameObject[] toActivate;
    [SerializeField] private GameObject[] toDeActivate;


    private void OnTriggerStay(Collider other)
    {
        if ((other.TryGetComponent(out PlayerControl pc) && pc.IsItMainPlayer) || (other.TryGetComponent(out RagdollPartCollisionChecker rag) && rag.LinkToPlayerControl.IsItMainPlayer))
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
}
