using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollPartCollisionChecker : MonoBehaviour
{
    public bool IsRagdollHasContact;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.gameObject.layer != 3 && !collision.gameObject.CompareTag("Player"))
        {
            IsRagdollHasContact = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision != null && collision.gameObject.layer != 3 && !collision.gameObject.CompareTag("Player"))
        {
            IsRagdollHasContact = false;
        }
    }
}
