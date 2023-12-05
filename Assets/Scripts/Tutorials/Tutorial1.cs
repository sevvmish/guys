using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl pc) && pc.IsItMainPlayer)
        {
            if (Globals.MainPlayerData.Hint1 == 0)
            {
                Globals.MainPlayerData.Hint1 = 1;
                StartCoroutine(doubleJumpHint());
            }
        }
    }

    private IEnumerator doubleJumpHint()
    {
        GameManager.Instance.GetUI().SetDoubleJumpHint(true);
        yield return new WaitForSeconds(10);
        GameManager.Instance.GetUI().SetDoubleJumpHint(false);
    }
}
