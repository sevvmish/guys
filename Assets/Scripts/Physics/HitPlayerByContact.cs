using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayerByContact : MonoBehaviour
{
    public float ImpulseForce = 25;
    public Transform from;
    public Transform to;
    public float Delay = 1;

    private Vector3 dir;
    private List<PlayerControl> players = new List<PlayerControl>();

    private void Start()
    {
        dir = (to.position - from.position).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl pc) && !players.Contains(pc) && !pc.IsRagdollActive)
        {
            players.Add(pc);
            StartCoroutine(play(pc, pc.transform.position + Vector3.up + pc.transform.forward));
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.TryGetComponent(out PlayerControl pc) && !players.Contains(pc) && !pc.IsRagdollActive)
        {
            players.Add(pc);
            StartCoroutine(play(pc, pc.transform.position + Vector3.up + pc.transform.forward));
        }
    }


    private IEnumerator play(PlayerControl pc, Vector3 point)
    {
        pc.ApplyTrapForce(dir * ImpulseForce, point, ApplyForceType.Punch_medium, 2);
       
        yield return new WaitForSeconds(Delay);
        players.Remove(pc);
    }
}
