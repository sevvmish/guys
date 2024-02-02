using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    [SerializeField] private float Delay = 2f;
    [SerializeField] private float StartDelay = 0;
    [SerializeField] private Transform from;
    [SerializeField] private Transform to;
    [SerializeField] private float timeForMove;


    private WaitForSeconds ZeroOne = new WaitForSeconds(0.1f);
    private WaitForSeconds DelayWait;
    private List<PlayerControl> players = new List<PlayerControl>();


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.TryGetComponent(out PlayerControl pc) && !players.Contains(pc) && !pc.IsRagdollActive)
        {
            players.Add(pc);
            StartCoroutine(play(pc));
        }
    }

    private IEnumerator play(PlayerControl pc)
    {
        Rigidbody rb = pc.GetComponent<Rigidbody>();        
        pc.GetComponent<ConditionControl>().MakeWindy(Delay);

        for (float i = 0; i < Delay; i+= 0.1f)
        {
            rb.DORotate(new Vector3(pc.transform.eulerAngles.x, pc.transform.eulerAngles.y + UnityEngine.Random.Range(-90, 90), pc.transform.eulerAngles.z), 0.1f).SetEase(Ease.Linear);
            rb.AddRelativeForce(Vector3.forward * 15, ForceMode.Impulse);

            if (pc.IsDead || pc.IsRagdollActive) break;
            yield return ZeroOne;
        }

        yield return new WaitForSeconds(Delay);
        players.Remove(pc);
    }
}
