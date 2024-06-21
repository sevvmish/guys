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
    private HashSet<Rigidbody> players = new HashSet<Rigidbody>();

    // Start is called before the first frame update
    void Start()
    {
        dir = (to.position - from.position).normalized;
        vfx.SetActive(false);

        if (drum == null) drum = transform;
    }

    private void OnCollisionEnter(Collision collision)
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
        vfx.SetActive(true);
        _audio.Play();
                

        //yield return new WaitForSeconds(Time.fixedDeltaTime);

        player.velocity = Vector3.zero;
        player.ResetInertiaTensor();

        player.AddForce(dir * force, ForceMode.Impulse);
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        
        for (int i = 0; i < 3; i++)
        {
            player.AddForce(dir * 10, ForceMode.Impulse); 
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        

        yield return new WaitForSeconds(0.3f);

        players.Remove(player);
        drum.localScale = Vector3.one;
        vfx.SetActive(false);
    }

    /*
    private IEnumerator cleanList(Rigidbody player)
    {
        vfx.SetActive(true);
        _audio.Play();

        player.AddForce(dir * 10, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f);

        players.Remove(player);
        drum.localScale = Vector3.one;
        vfx.SetActive(false);
    }*/
}
