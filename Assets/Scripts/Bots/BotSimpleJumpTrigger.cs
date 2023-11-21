using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSimpleJumpTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl pc) && !pc.IsItMainPlayer)
        {            
            pc.SetJump();
        }
    }
}
