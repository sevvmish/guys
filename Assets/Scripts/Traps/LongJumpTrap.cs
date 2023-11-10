using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongJumpTrap : MonoBehaviour
{
    [SerializeField] private Transform from, to;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerControl player))
        {
            //player.ApplyTrapForce((to.position - from.position) * 30);
        }
    }
}
