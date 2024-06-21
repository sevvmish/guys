using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffect : MonoBehaviour
{
    private HashSet<PlayerControl> players = new HashSet<PlayerControl>();
    [SerializeField] private GameObject effect;
    private ObjectPool poolEffects;

    // Start is called before the first frame update
    void Start()
    {
        poolEffects = new ObjectPool(10, effect, transform.parent);
    }
        
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerControl p) && !players.Contains(p))
        {
            players.Add(p);
            StartCoroutine(playEffect(p));
        }
    }
    

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerControl p) && !players.Contains(p))
        {
            players.Add(p);
            StartCoroutine(playEffect(p));
        }
    }
    */

    private IEnumerator playEffect(PlayerControl p)
    {
        GameObject e = poolEffects.GetObject();
        e.transform.position = p.transform.position;
        e.transform.eulerAngles = Vector3.zero;
        e.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        poolEffects.ReturnObject(e);
        players.Remove(p);
    }
}
