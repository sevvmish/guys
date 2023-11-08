using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsControl : MonoBehaviour
{
    [SerializeField] private GameObject shadow;
    [SerializeField] private GameObject respawnFX;

    [SerializeField] private GameObject jumpEffect;
    private AudioSource jumpSound;

    [SerializeField] private ParticleSystem landEffect;
   

    // Start is called before the first frame update
    void Start()
    {
        shadow.SetActive(false);
        respawnFX.SetActive(false);
        landEffect.gameObject.SetActive(true);
        jumpEffect.SetActive(true);
        jumpSound = jumpEffect.GetComponent<AudioSource>();
    }

    public void SetShadow(bool isActive) => shadow.SetActive(isActive);

    public void MakeJumpFX()
    {
        jumpSound.Play();
    }

    public void PlayRespawnEffect() => StartCoroutine(playEffect(1.5f, respawnFX));

    public void MakeLandEffect()
    {        
        landEffect.Play();
    }

    private IEnumerator playEffect(float duration, GameObject fx)
    {
        fx.SetActive(false);
        fx.SetActive(true);
        yield return new WaitForSeconds(duration);
        fx.SetActive(false);
    }
}
