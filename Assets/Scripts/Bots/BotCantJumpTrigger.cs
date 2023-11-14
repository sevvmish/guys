using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCantJumpTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 && other.TryGetComponent(out BotAI bot) && bot.IsCanJump)
        {
            bot.IsCanJump = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9 && other.TryGetComponent(out BotAI bot) && !bot.IsCanJump)
        {
            bot.IsCanJump = true;
        }
    }
}
