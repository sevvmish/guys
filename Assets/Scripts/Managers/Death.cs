using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] private bool isLaser = false;


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out RespawnControl r))
        {
            if (isLaser)
            {
                other.GetComponent<ConditionControl>().LaserDeath();
                StartCoroutine(play(r));
            }
            else
            {
                r.Die();
            }
        }
        else if (other.TryGetComponent(out RagdollPartCollisionChecker rag))
        {            
            if (isLaser)
            {
                other.GetComponent<ConditionControl>().LaserDeath();
                StartCoroutine(play(rag.LinkToPlayerControl.gameObject.GetComponent<RespawnControl>()));
            }
            else
            {
                rag.LinkToPlayerControl.gameObject.GetComponent<RespawnControl>().Die();
            }
        }
    }

    private IEnumerator play(RespawnControl r)
    {
        yield return new WaitForSeconds(1);

        r.Die();
    }
}
