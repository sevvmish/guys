using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullDownByTrigger : MonoBehaviour
{
    [SerializeField] private float forceAmount = 10;

    private void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.layer == 9) && other.TryGetComponent(out Rigidbody player))
        {
            player.AddForce(Vector3.down * forceAmount, ForceMode.Impulse);
        }
    }
}
