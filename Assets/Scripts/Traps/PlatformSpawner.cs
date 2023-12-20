using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private PlatformSystem[] examples;
    [SerializeField] private Transform[] from;
    [SerializeField] private Transform[] to;
    [SerializeField] private float speedTime = 11;
    [SerializeField] private float howOften = 2f;
    [SerializeField] private float disperse = 0.3f;
    [SerializeField] private int spawnReserve = 10;

    private float _timer = 0;
    private List<int> lastRND = new List<int>();
    private float nextTime = 0;
    private ObjectPool[] pools;
    private Dictionary<PlatformSystem, ObjectPool> source = new Dictionary<PlatformSystem, ObjectPool>();

    // Start is called before the first frame update
    void Start()
    {
        pools = new ObjectPool[examples.Length];

        for (int i = 0; i < examples.Length; i++)
        {
            pools[i] = new ObjectPool(spawnReserve * examples.Length, examples[i].gameObject, transform);
        }

        //nextTime = howOften + UnityEngine.Random.Range(-disperse, disperse);
        _timer = nextTime + 1;
    }

    private void Update()
    {
        if (_timer > nextTime)
        {
            _timer = 0;
            nextTime = howOften + UnityEngine.Random.Range(-disperse, disperse);
            addPlatform();
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    private void addPlatform()
    {
        int rnd = UnityEngine.Random.Range(0, examples.Length);

        int fromToRND = UnityEngine.Random.Range(0, from.Length);

        if (lastRND.Count > (2))
        {
            lastRND.Remove(lastRND[0]);
        }

        for (int i = 0; i < 100; i++)
        {
            if (lastRND.Count > 0 && lastRND.Contains(fromToRND))
            {
                fromToRND = UnityEngine.Random.Range(0, from.Length);
            }
            else
            {
                lastRND.Add(fromToRND);
                break;
            }
        }
        

        GameObject g = pools[rnd].GetObject();
        g.transform.position = from[fromToRND].position;
        g.SetActive(true);
        PlatformSystem pl = g.GetComponent<PlatformSystem>();
        pl.SetPlay(from[fromToRND].position, to[fromToRND].position, speedTime, this);
        source.Add(pl, pools[rnd]);
    }

    public void PlatformEnded(PlatformSystem pl)
    {
        source[pl].ReturnObject(pl.gameObject);
        source.Remove(pl);
    }

}
