using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingOnCylinder : MonoBehaviour
{
    public float Force = 5;
    public Transform from;
    public Transform to;

    private Vector3 dir;

    private void Start()
    {
        dir = (to.position - from.position).normalized;
    }

  
    private void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject.layer == 9 || collision.gameObject.layer == 3)
            && collision.gameObject.TryGetComponent(out Rigidbody player))
        {
            player.AddForce(Vector3.Cross(-collision.contacts[0].normal, dir) * Force, ForceMode.Force);
            //player.AddForce(Vector3.Cross(-collision.contacts[0].normal, dir) * 20, ForceMode.Impulse);
        }
    }
}
