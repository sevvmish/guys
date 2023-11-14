using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCantDoubleJumpTrig : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 && other.TryGetComponent(out BotAI bot) && bot.IsCanDoubleJump)
        {
            bot.IsCanDoubleJump = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9 && other.TryGetComponent(out BotAI bot) && !bot.IsCanDoubleJump)
        {
            bot.IsCanDoubleJump = true;
        }
    }
}
