using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3InvisibleForce : MonoBehaviour
{
    public float AddForce = 5;
    public Transform from;
    public Transform to;
    public float Delay = 3;

    private Vector3 dir;
    private List<PlayerControl> players = new List<PlayerControl>();
    private WaitForSeconds ZeroOne = new WaitForSeconds(0.1f);

    private void Start()
    {
        dir = (to.position - from.position).normalized;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl pc) && !players.Contains(pc))
        {
            players.Add(pc);
            StartCoroutine(play(pc, pc.transform.position + Vector3.up + pc.transform.forward));
        }
    }

    private IEnumerator play(PlayerControl pc, Vector3 point)
    {
        Rigidbody rb = pc.GetComponent<Rigidbody>();

        for (float i = 0; i < 1; i+=0.1f)
        {
            rb.AddForce(dir * AddForce, ForceMode.Impulse);

            yield return ZeroOne;

            if (pc.IsDead) break;
        }

        yield return new WaitForSeconds(Delay);

        players.Remove(pc);
    }
}
