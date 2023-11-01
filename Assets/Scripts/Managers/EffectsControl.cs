using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsControl : MonoBehaviour
{
    [SerializeField] private GameObject shadow;
    
    [SerializeField] private GameObject jumpEffect;
    private AudioSource jumpSound;

    [SerializeField] private ParticleSystem landEffect;
    //private AudioSource landSound;

    //private ObjectPool jumpPool;

    // Start is called before the first frame update
    void Start()
    {
        shadow.SetActive(false);
        landEffect.gameObject.SetActive(true);
        //landSound = landEffect.GetComponent<AudioSource>();
        jumpEffect.SetActive(true);
        jumpSound = jumpEffect.GetComponent<AudioSource>();
        //jumpPool = new ObjectPool(2, jumpEffect, transform);
    }

    public void SetShadow(bool isActive) => shadow.SetActive(isActive);

    public void MakeJumpFX()
    {
        jumpSound.Play();
        //StartCoroutine(playJumpFX());
    }
    private IEnumerator playJumpFX()
    {
        //GameObject temp = jumpPool.GetObject();
        //jumpEffect.SetActive(true);
        yield return new WaitForSeconds(0.37f);
        //jumpEffect.SetActive(false);
        //jumpPool.ReturnObject(temp);
    }

    public void MakeLandEffect()
    {        
        landEffect.Play();
        //landSound.Play();
    }
}
