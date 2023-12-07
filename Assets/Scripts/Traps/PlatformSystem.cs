using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSystem : MonoBehaviour
{    
    private float speed = 10;
    private Vector3 from;
    private Vector3 to;
    private PlatformSpawner spawner;
    //private List<Rigidbody> players = new List<Rigidbody>();
    private Dictionary<Transform, Transform> players = new Dictionary<Transform, Transform>();

    private bool isOff;

    public void SetPlay(Vector3 from, Vector3 to, float speed, PlatformSpawner spawner)
    {
        //boxer.enabled = true;
        players.Clear();
        this.from = from;
        this.to = to;
        this.speed = speed;
        this.spawner = spawner;
        isOff = false;
        transform.position = from;
        transform.LookAt(to);
        transform.DOMove(to, speed).SetUpdate(UpdateType.Fixed).SetEase(Ease.Linear).OnComplete(Ended);
    }

    private void Ended()
    {
        isOff = true;
        //boxer.enabled = false;

        foreach (Transform pl in players.Keys)
        {
            pl.SetParent(players[pl]);
        }

        spawner.PlatformEnded(this);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isOff) return;

        if (!players.ContainsKey(collision.transform) && (collision.gameObject.layer == Globals.LAYER_PLAYER ))
        {
            if (collision.gameObject.TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = Vector3.zero;
            }

            players.Add(collision.transform, collision.transform.parent);
            collision.transform.SetParent(transform);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (isOff) return;

        if (!players.ContainsKey(collision.transform) && (collision.gameObject.layer == Globals.LAYER_PLAYER))
        {
            if (collision.gameObject.TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = Vector3.zero;
            }

            players.Add(collision.transform, collision.transform.parent);
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {        
        if (players.ContainsKey(collision.transform))
        {
            collision.transform.SetParent(players[collision.transform]);
            players.Remove(collision.transform);
        }
    }
}
