using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsControl : MonoBehaviour
{

    [SerializeField] private GameObject shadow;

    [SerializeField] private GameObject respawnFX;

    [SerializeField] private GameObject frozen;
    private bool isFrozenBusy;

    [SerializeField] private GameObject punchEasy;
    [SerializeField] private GameObject punchMedium;
    [SerializeField] private GameObject punchLarge;

    [SerializeField] private GameObject paintBlue;
    [SerializeField] private GameObject paintGreen;
    [SerializeField] private GameObject paintPink;

    [SerializeField] private GameObject woohooSound;
    [SerializeField] private GameObject woohooSound2;
    [SerializeField] private GameObject yiihaaSound;

    [SerializeField] private GameObject woohooGirlSound;
    [SerializeField] private GameObject woohooGirlSound2;

    [SerializeField] private GameObject smallPunchSound;

    [SerializeField] private GameObject fastEffect;


    [SerializeField] private GameObject jumpEffect;
    private AudioSource jumpSound;

    [SerializeField] private ParticleSystem landEffect;

    private PlayerControl pc;

    // Start is called before the first frame update
    void Start()
    {
        pc = transform.parent.GetComponent<PlayerControl>();

        shadow.SetActive(false);
        respawnFX.SetActive(false);
        landEffect.gameObject.SetActive(true);
        jumpEffect.SetActive(true);
        jumpSound = jumpEffect.GetComponent<AudioSource>();

        punchEasy.SetActive(false);
        punchMedium.SetActive(false);
        punchLarge.SetActive(false);

        paintBlue.SetActive(false);
        paintGreen.SetActive(false);
        paintPink.SetActive(false);

        woohooSound.SetActive(false);
        woohooSound2.SetActive(false);
        yiihaaSound.SetActive(false);

        woohooGirlSound.SetActive(false);
        woohooGirlSound2.SetActive(false);

        smallPunchSound.SetActive(false);

        frozen.SetActive(false);

        fastEffect.SetActive(false);
    }

    public void SetShadow(PlayerControl player)
    {
        shadow.AddComponent<ShadowPoint>();
        shadow.GetComponent<ShadowPoint>().SetData(player);
    }

    public void ShowShadow(bool isActive)
    {
        shadow.SetActive(isActive);
    }

    public void MakeJumpFX()
    {
        jumpSound.Play();
    }

    public void MakeSmallPunchSound()
    {
        StartCoroutine(playEffect(1, smallPunchSound));
    }

    public void MakeWoohooSound()
    {
        StartCoroutine(playEffect(1, woohooSound));
    }

    public void MakeWoohooSound2()
    {
        StartCoroutine(playEffect(1, woohooSound2));
    }

    public void MakeWoohooGirlSound()
    {
        StartCoroutine(playEffect(1, woohooGirlSound));
    }

    public void MakeWoohooGirlSound2()
    {
        StartCoroutine(playEffect(1, woohooGirlSound2));
    }

    public void MakeFastEffect(float duration)
    {
        StartCoroutine(playEffectBreakable(duration, fastEffect));
    }

    public void MakeFunnySound()
    {
        MakeFunnySound(100);
    }
    public void MakeFunnySound(int chanceFrom100)
    {
        //if (woohooSound.activeSelf || yiihaaSound.activeSelf || woohooSound2.activeSelf || woohooGirlSound.activeSelf ||) return;

        if (chanceFrom100 > 100) chanceFrom100 = 100;

        int rnd = UnityEngine.Random.Range(0, 100);

        if (rnd > chanceFrom100) return;

        if ((int)pc.CurrentSkin >= 50)
        {
            
            rnd = UnityEngine.Random.Range(0, 2);
            switch (rnd)
            {
                case 0:
                    MakeWoohooGirlSound();
                    break;

                case 1:
                    MakeWoohooGirlSound2();
                    break;
            }
        }
        else
        {
            rnd = UnityEngine.Random.Range(0, 3);
            switch (rnd)
            {
                case 0:
                    MakeWoohooSound();
                    break;

                case 1:
                    MakeYiihaaSound();
                    break;

                case 2:
                    MakeWoohooSound2();
                    break;
            }
        }

        
    }

    public void MakeYiihaaSound()
    {
        StartCoroutine(playEffect(1, yiihaaSound));
    }

    public void PlayPunchEffect(ApplyForceType _type, Vector3 pointOfPunch)
    {
        StartCoroutine(playPunch(_type, pointOfPunch));
    }
    private IEnumerator playPunch(ApplyForceType _type, Vector3 pointOfPunch)
    {
        switch (_type)
        {
            case ApplyForceType.Punch_easy:
                if (punchEasy.activeSelf) punchEasy.SetActive(false);                
                punchEasy.transform.position = pointOfPunch + Vector3.up * 0.3f;
                punchEasy.SetActive(true);
                break;

            case ApplyForceType.Punch_medium:
                if (punchMedium.activeSelf) punchMedium.SetActive(false);                
                punchMedium.transform.position = pointOfPunch + Vector3.up * 0.3f;
                punchMedium.SetActive(true);
                break;

            case ApplyForceType.Punch_large:
                if (punchLarge.activeSelf) punchLarge.SetActive(false);                
                punchLarge.transform.position = pointOfPunch + Vector3.up * 0.3f;
                punchLarge.SetActive(true);
                break;
        }

        yield return new WaitForSeconds(0.6f);

        switch (_type)
        {
            case ApplyForceType.Punch_easy:                
                punchEasy.SetActive(false);
                break;

            case ApplyForceType.Punch_medium:                
                punchMedium.SetActive(false);
                break;

            case ApplyForceType.Punch_large:                
                punchLarge.SetActive(false);
                break;
        }
    }

    public void PlayRespawnEffect() => StartCoroutine(playEffect(1.5f, respawnFX));

    public void MakeLandEffect()
    {        
        landEffect.Play();
    }

    public void MakePainted(Color color, float timer)
    {
        GameObject g = default;
        if (color == Color.green)
        {
            g = paintGreen;
        }
        else if (color == Color.blue)
        {
            g = paintBlue;
        }
        else if (color == Color.magenta)
        {
            g = paintPink;
        }

        StartCoroutine(playEffectBreakable(timer, g));
    }
    

    private IEnumerator playEffect(float duration, GameObject fx)
    {
        fx.SetActive(false);
        fx.SetActive(true);
        yield return new WaitForSeconds(duration);
        fx.SetActive(false);
    }

    private IEnumerator playEffectBreakable(float duration, GameObject fx)
    {
        fx.SetActive(false);
        fx.SetActive(true);
        for (float i = 0; i < duration; i += 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
            if (pc.IsDead && pc.IsRagdollActive) break;
        }
        fx.SetActive(false);
    }

    public void MakeFrozen(float seconds)
    {       
        StartCoroutine(playFrozen(seconds));
    }
    private IEnumerator playFrozen(float duration)
    {
        for (float i = 0; i < duration - 0.3f; i += 0.1f)
        {
            if (!isFrozenBusy) break;
            yield return new WaitForSeconds(0.1f);
        }

        if (isFrozenBusy) yield break;

        isFrozenBusy = true;


        frozen.SetActive(false);
        frozen.SetActive(true);

        frozen.transform.GetChild(1).gameObject.SetActive(true);
        frozen.transform.GetChild(1).gameObject.SetActive(false);

        frozen.transform.GetChild(0).gameObject.SetActive(true);

        for (float i = 0; i < duration; i += 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
            if (pc.IsDead && pc.IsRagdollActive) break;
        }

        frozen.transform.GetChild(1).gameObject.SetActive(true);
        frozen.transform.GetChild(0).gameObject.SetActive(false);

        

        yield return new WaitForSeconds(1f);
        frozen.transform.GetChild(1).gameObject.SetActive(false);
        isFrozenBusy = false;
        frozen.SetActive(false);
    }
}
