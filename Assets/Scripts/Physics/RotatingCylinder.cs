using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCylinder : MonoBehaviour
{
    [SerializeField] private Transform center;
    [SerializeField] private float koeff = 1f;

    private void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject.layer == 9 || collision.gameObject.layer == 3) && collision.gameObject.TryGetComponent(out Rigidbody player))
        {            
            //player.AddForce(Vector3.Cross((center.position - player.transform.position).normalized, Vector3.up) * 50, ForceMode.Force);
            player.MovePosition(player.transform.position + Vector3.Cross((center.position - player.transform.position).normalized, Vector3.up) / 10f * koeff);
        }
    }
}
