using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] private bool isLaser = false;
    private WaitForSeconds ZeroFive = new WaitForSeconds(0.05f);
    private GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
    }

    private void OnTriggerStay(Collider other)
    {
        if (gm.IsGameStarted && other.CompareTag("Player") && other.TryGetComponent(out RespawnControl r))
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
        else if (gm.IsGameStarted && other.TryGetComponent(out RagdollPartCollisionChecker rag))
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
        
        for (float i = 0; i < 1; i+=0.05f)
        {
            if (!gm.IsGameStarted) yield break;
            yield return ZeroFive;
        }
        

        r.Die();
    }
}
