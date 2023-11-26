using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCantDoubleJumpTrig : MonoBehaviour
{
    private HashSet<BotAI> bots = new HashSet<BotAI>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 && other.TryGetComponent(out BotAI bot) && bot.IsCanDoubleJump && !bots.Contains(bot))
        {
            bots.Add(bot);
            bot.IsCanDoubleJump = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9 && other.TryGetComponent(out BotAI bot) && bots.Contains(bot))
        {
            bots.Remove(bot);
            bot.IsCanDoubleJump = true;
        }
    }
}
