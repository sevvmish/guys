using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceExplosion : MonoBehaviour, Explosives
{
    [SerializeField] private GameObject VFX;

    private bool isActive;
    private float timer = 3f;

    
    private void OnEnable()
    {
        isActive = true;
        VFX.SetActive(true);
        StartCoroutine(playExplosion());
    }

    private void OnTriggerStay(Collider other)
    {
        if (isActive && other.TryGetComponent(out ConditionControl player) && !player.HasCondition(Conditions.frozen))
        {            
            player.MakeFrozen(timer);
        }
    }

    private IEnumerator playExplosion()
    {        
        yield return new WaitForSeconds(0.3f);
        isActive = false;
        
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }

    public void SetTTL(float seconds)
    {
        //
    }
}
