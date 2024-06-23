using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrumJumper : MonoBehaviour
{
    [SerializeField] private Transform drum;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private float force = 60;
    [SerializeField] private Transform from;
    [SerializeField] private Transform to;
    [SerializeField] private bool isTesting;

    [SerializeField] private GameObject vfx;

    private Vector3 dir;
    private HashSet<Rigidbody> players = new HashSet<Rigidbody>();

    // Start is called before the first frame update
    void Start()
    {
        dir = (to.position - from.position).normalized;
        vfx.SetActive(false);

        if (drum == null) drum = transform;
    }

    private void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject.layer == 9 || collision.gameObject.layer == 3) && collision.gameObject.TryGetComponent(out Rigidbody player) && !players.Contains(player))
        {        
            players.Add(player);
            drum.localScale = Vector3.one;
            drum.DOShakeScale(0.3f, 0.6f, 30).SetEase(Ease.OutQuad);

            StartCoroutine(play(player));           
        }
    }


    private IEnumerator play(Rigidbody player)
    {
        if (isTesting)
        {
            player.velocity = Vector3.zero;
            player.AddForce(dir * force*2, ForceMode.Impulse);

            yield return new WaitForSeconds(0.5f);
            players.Remove(player);
            drum.localScale = Vector3.one;
            vfx.SetActive(false);
        }
        else
        {
            vfx.SetActive(true);
            _audio.Play();

            if (player.drag != 2) player.drag = 2;
            //player.velocity = Vector3.zero;
            //player.ResetInertiaTensor();

            player.AddForce(dir * force, ForceMode.Impulse);
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            for (int i = 0; i < 3; i++)
            {
                if (player.drag != 2) player.drag = 2;
                player.AddForce(dir * 10, ForceMode.Impulse);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }


            yield return new WaitForSeconds(0.3f);

            players.Remove(player);
            drum.localScale = Vector3.one;
            vfx.SetActive(false);
        }

        
    }

}
