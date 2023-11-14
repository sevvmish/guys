using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out RespawnControl r))
        {
            r.Die();
        }
        else if (other.TryGetComponent(out RagdollPartCollisionChecker rag))
        {
            rag.LinkToPlayerControl.gameObject.GetComponent<RespawnControl>().Die();
        }
    }
}
