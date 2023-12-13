using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFallZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl pc) && !pc.IsDead && !pc.IsRagdollActive)
        {
            pc.SetFreeFall(true);
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl pc) && pc.IsFreeFall)
        {
            pc.SetFreeFall(false);
        }
    }
}
