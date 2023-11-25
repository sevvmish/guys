using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDropper : MonoBehaviour
{
    [SerializeField] private GameObject shadow;

    [SerializeField] private GameObject[] bomb;
    [SerializeField] private float distance = 10;
    [SerializeField] private float flyTime = 4;
    [SerializeField] private float interval = 4;

    [SerializeField] private float deltaX = 2;
    [SerializeField] private float deltaZ = 2;

    [SerializeField] private int howManyPerTime = 1;

    private ObjectPool[] pools;
    private ObjectPool poolShadow;
    private float _timer;
    private float _random;
    private Vector3 lastPos, pos;

    // Start is called before the first frame update
    void Start()
    {
        pools = new ObjectPool[bomb.Length];
        
        _random = UnityEngine.Random.Range(-2f, 2f);
        int amount = ((int)(flyTime / interval) + 2) * howManyPerTime;

        poolShadow = new ObjectPool(amount * bomb.Length, shadow, GameManager.Instance.GetVFX());

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new ObjectPool(amount, bomb[i], GameManager.Instance.GetVFX());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > (interval + _random))
        {   
            _timer = 0;
            _random = UnityEngine.Random.Range(-2f, 2f);

            for (int i = 0; i < howManyPerTime; i++)
            {
                addBomb();
            }
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    private void addBomb()
    {        
        int rnd = UnityEngine.Random.Range(0, bomb.Length);

        ObjectPool p = pools[rnd];
        GameObject g = p.GetObject();
        GameObject shad = poolShadow.GetObject();
        shad.SetActive(true);

        AnyBarrel b = g.GetComponent<AnyBarrel>();

        for (int i = 0; i < 10; i++)
        {
            pos = new Vector3(
            transform.position.x + UnityEngine.Random.Range(-deltaX, deltaX),
            transform.position.y,
            transform.position.z + UnityEngine.Random.Range(-deltaZ, deltaZ));

            if ((pos - lastPos).magnitude > 6) break;
        }
        
        lastPos = pos;
                
        g.transform.position = pos;
        lastPos = pos;

        shad.transform.position = pos + Vector3.up * 0.051f - Vector3.up * distance;

        b.SetParashute(distance, flyTime, 0.1f);
        g.SetActive(true);
        StartCoroutine(playParashute(g, p, shad));
    }

    private IEnumerator playParashute(GameObject g, ObjectPool p, GameObject sh)
    {
        yield return new WaitForSeconds(flyTime);

        while (g.activeSelf)
        {
            yield return new WaitForSeconds(0.1f);
        }

        poolShadow.ReturnObject(sh);
        p.ReturnObject(g);
    }
}
