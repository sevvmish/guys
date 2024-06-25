using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject vfx;
    [SerializeField] private float forceKoeff = 8;

    private bool isEffect;

    // Start is called before the first frame update
    void Start()
    {        
        vfx.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == Globals.LAYER_PLAYER && collision.gameObject.TryGetComponent(out Rigidbody another))
        {
            if (another.TryGetComponent(out ConditionControl cc) && !cc.HasCondition(Conditions.frozen))
            {
                /*
                if (collision.gameObject.TryGetComponent(out PlayerControl p))
                {
                    p.ApplyTrapForce(collision.impulse, collision.GetContact(0).point, ApplyForceType.Punch_medium, 1);
                }
                */
                another.velocity = Vector3.zero;
                another.AddForce((new Vector3(another.transform.position.x, 0, another.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized * Globals.PLAYERS_COLLIDE_FORCE * forceKoeff, ForceMode.Impulse);
            }

            if (!isEffect)
            {
                StartCoroutine(playEffect());
            }
        }
    }

    private IEnumerator playEffect()
    {
        Vector3 scale = model.transform.localScale;
        isEffect = true;
        vfx.SetActive(true);
        model.transform.DOShakeScale(0.3f, 1, 20).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.7f);

        vfx.SetActive(false);
        isEffect = false;
        model.transform.localScale = scale;
    }
}
