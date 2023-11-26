using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCantJumpTrigger : MonoBehaviour
{
    private HashSet<BotAI> bots = new HashSet<BotAI>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 && other.TryGetComponent(out BotAI bot) && bot.IsCanJump && !bots.Contains(bot))
        {
            bots.Add(bot);
            bot.IsCanJump = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9 && other.TryGetComponent(out BotAI bot) && bots.Contains(bot))
        {
            bots.Remove(bot);
            bot.IsCanJump = true;
        }
    }
}
