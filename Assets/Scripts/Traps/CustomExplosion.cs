using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomExplosion : MonoBehaviour, Explosives
{
    [SerializeField] private GameObject VFX;

    private bool isActive;
    private float force = 7;
    private HashSet<Rigidbody> players = new HashSet<Rigidbody>();


    private void OnEnable()
    {
        players.Clear();
        isActive = true;
        VFX.SetActive(true);
        StartCoroutine(playExplosion());
    }

    
    private void OnTriggerStay(Collider other)
    {
        if (isActive && other.TryGetComponent(out Rigidbody player) && !players.Contains(player))
        {
            players.Add(player);
            Vector3 mainVec = (player.transform.position - transform.position + Vector3.up).normalized * force/* + new Vector3(0,6,0)*/;
            player.AddForce(mainVec, ForceMode.Impulse);
            if (player.TryGetComponent(out PlayerControl pc))
            {
                pc.ApplyTrapForce(mainVec, transform.position, ApplyForceType.Punch_easy, 1);
            }
        }
    }

    private IEnumerator playExplosion()
    {

        yield return new WaitForSeconds(0.3f);
        isActive = false;

        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    public void SetTTL(float seconds)
    {
        //
    }
}
