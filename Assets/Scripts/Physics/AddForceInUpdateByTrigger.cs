using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AddForceInUpdateByTrigger : MonoBehaviour
{
    public float Force = 5;
    public Transform from;
    public Transform to;

    private Vector3 dir;

    private void Start()
    {
        dir = (to.position - from.position).normalized;
    }


    private void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.layer == 9 || other.gameObject.layer == 3) && other.TryGetComponent(out Rigidbody player) && player.velocity.magnitude < 20)
        {
            player.AddForce(dir * Force, ForceMode.Force);
            //player.MovePosition(player.transform.position + dir * Force);
        }
    }

    
}
