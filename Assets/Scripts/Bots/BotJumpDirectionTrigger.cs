using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotJumpDirectionTrigger : MonoBehaviour
{
    public bool IsDoubleJump;
    public Transform dir;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out BotAI bot))
        {
            if (IsDoubleJump)
            {
                bot.MakeDoubleJump(dir.position);
            }
            else
            {
                bot.MakeJump(dir.position);
            }
            
        }
    }
}
