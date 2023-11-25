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

    [SerializeField] private GameObject vfx;

    private Vector3 dir;
    //private HashSet<Rigidbody> players = new HashSet<Rigidbody>();

    // Start is called before the first frame update
    void Start()
    {
        dir = (to.position - from.position).normalized;
        vfx.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.layer == 9 || collision.gameObject.layer == 3) && collision.gameObject.TryGetComponent(out Rigidbody player)/* && !players.Contains(player)*/)
        {            
            //players.Add(player);
            StartCoroutine(cleanList(player));
            drum.localScale = Vector3.one;
            drum.DOShakeScale(0.3f, 0.6f, 30).SetEase(Ease.OutQuad);
            player.velocity = Vector3.zero;
            player.AddForce(dir * force, ForceMode.Impulse);
            
            if (collision.gameObject.TryGetComponent(out PlayerControl pc))
            {
                pc.StopJumpPermission(0.5f);
            }
        }
    }


    private IEnumerator cleanList(Rigidbody player)
    {
        vfx.SetActive(true);
        _audio.Play();


        for (int j = 0; j < 3; j++)
        {
            player.AddForce(dir * force/10f, ForceMode.Force);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        
        yield return new WaitForSeconds(0.2f);
        drum.localScale = Vector3.one;
        vfx.SetActive(false);

        //if (players.Contains(player))
        //{
        //    players.Remove(player);
        //}
    }
}
