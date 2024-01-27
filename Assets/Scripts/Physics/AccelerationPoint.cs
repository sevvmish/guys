using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AccelerationPoint : MonoBehaviour
{
    public float AddForce = 200f;    
    public float DecreaseKoeff = 3f;
    public float DownForce = 50f;
    public float DownPeriod = 0.9f;
    public float Timer = 2f;
    public Transform from;
    public Transform to;

    private Vector3 dir;
    private HashSet<PlayerControl> players = new HashSet<PlayerControl>();

    private void Start()
    {
        dir = (to.position - from.position).normalized;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerControl pc) && !players.Contains(pc) && !pc.IsRagdollActive)
        {
            players.Add(pc);
            StartCoroutine(play(pc));
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out PlayerControl pc) && !players.Contains(pc) && !pc.IsRagdollActive)
        {
            players.Add(pc);
            StartCoroutine(play(pc));
        }
    }
    private IEnumerator play(PlayerControl pc)
    {
        Rigidbody rb = pc.GetComponent<Rigidbody>();
        pc.transform.GetChild(0).GetComponent<EffectsControl>().MakeFunnySound();
        //int x = 0;

        rb.AddForce(dir * AddForce / DecreaseKoeff, ForceMode.Impulse);

        for (float i = 0; i < Timer; i+=0.05f)
        {
            rb.AddForce(dir * AddForce, ForceMode.Force);
            //print("!!!!!!!: " + x);
            //x++;
            if (pc.IsDead) break;
            yield return new WaitForSeconds(0.05f);

            if (i > Timer * DownPeriod)
            {
                rb.AddForce(Vector3.down * DownForce, ForceMode.Force);
            }
        }

        rb.velocity /= 2;

        if (players.Contains(pc))
        {
            players.Remove(pc);
        }
    }
        
}
