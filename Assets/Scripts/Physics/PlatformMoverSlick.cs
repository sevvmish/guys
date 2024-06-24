using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoverSlick : MonoBehaviour
{    
    private Dictionary<Transform, Transform> players = new Dictionary<Transform, Transform>();
    private Level18Helper level18Helper;

    private void OnEnable()
    {
        players.Clear();
        level18Helper = GameObject.Find("LevelHelper").GetComponent<Level18Helper>();
    }

    

    private void OnDisable()
    {        
        if (players.Count > 0)
        {
            foreach (var player in players.Keys) 
            {
                level18Helper.PullMeOut(player, players[player], this.gameObject);
            }
        }

    }


    private void OnCollisionStay(Collision collision)
    {        
        if (!players.ContainsKey(collision.transform) && (collision.gameObject.layer == Globals.LAYER_PLAYER))
        {            
            players.Add(collision.transform, collision.transform.parent);
            collision.transform.SetParent(transform);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {        
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
