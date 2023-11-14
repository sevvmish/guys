using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] private int currentIndex;
    private HashSet<GameObject> players = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out BotAI bot) && bot.CurrentIndex > currentIndex)
        {
            bot.ResetIndexToValue(currentIndex);
        }

        if (other.CompareTag("Player") && !players.Contains(other.gameObject) && other.TryGetComponent(out RespawnControl resp) && resp.CurrentRespawnIndex < currentIndex)
        {
            players.Add(other.gameObject);
            resp.SetNewRespawn(new RespawnControl.RespawnData(new Vector3(
                other.transform.position.x, 
                transform.position.y, 
                other.transform.position.z) , transform.eulerAngles), currentIndex);
        }
    }
}
