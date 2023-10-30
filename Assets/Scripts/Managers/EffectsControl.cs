using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsControl : MonoBehaviour
{
    [SerializeField] private GameObject shadow;
    [SerializeField] private GameObject jumpEffect;

    [SerializeField] private ParticleSystem landEffect;

    private ObjectPool jumpPool;

    // Start is called before the first frame update
    void Start()
    {
        shadow.SetActive(false);

        jumpEffect.SetActive(false);
        jumpPool = new ObjectPool(2, jumpEffect, transform);
    }

    public void SetShadow(bool isActive) => shadow.SetActive(isActive);

    public void MakeJumpFX()
    {
        StartCoroutine(playJumpFX());
    }
    private IEnumerator playJumpFX()
    {
        GameObject temp = jumpPool.GetObject();
        temp.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        jumpPool.ReturnObject(temp);
    }

    public void MakeLandEffect()
    {        
        landEffect.Play();
    }
}
