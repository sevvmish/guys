using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCantMove : MonoBehaviour
{
    private HashSet<BotAI> bots = new HashSet<BotAI>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 && other.TryGetComponent(out BotAI bot) && bot.IsCanRun && !bots.Contains(bot))
        {
            bots.Add(bot);
            //StartCoroutine(freeBot(bot));
            bot.IsCanRun = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9 && other.TryGetComponent(out BotAI bot) && bots.Contains(bot))
        {
            bots.Remove(bot);
            bot.IsCanRun = true;
        }
    }

    private IEnumerator freeBot(BotAI bot)
    {
        PlayerControl p = bot.GetComponent<PlayerControl>();

        for (float i = 0; i < 2; i+= 0.1f)
        {
            if (p.IsDead || p.IsRagdollActive) break;

            yield return new WaitForSeconds(0.1f);
        }

        
        if (bots.Contains(bot))
        {
            bots.Remove(bot);
            bot.IsCanRun = true;
        }
    }
}
