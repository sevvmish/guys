using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainPlayerTrigger : MonoBehaviour
{
    public UnityEvent e;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerControl pc) && pc.IsItMainPlayer)
        {
            e?.Invoke();
        }

    }
}
