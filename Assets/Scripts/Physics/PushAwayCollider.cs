using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAwayCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.collider.gameObject.layer == Globals.LAYER_PLAYER && collision.gameObject.TryGetComponent(out PlayerControl pc))
        {
            pc.ApplyTrapForce(
                (pc.transform.position - transform.position).normalized * Globals.PLAYERS_COLLIDE_FORCE, 
                collision.contacts[0].point, 
                ApplyForceType.Punch_easy, 
                1);                        
        }
    }
}
