using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneResper : MonoBehaviour
{
    [SerializeField] private GameObject stoneExample;
    [SerializeField] private GameObject breakVFXExample;

    [SerializeField] private Transform from;
    [SerializeField] private Transform to;
    [SerializeField] private float cooldown = 2f;


    private ObjectPool poolStones;
    private ObjectPool poolVFXStone;
    private float _timer;
    private GameManager gm;
    private HashSet<GameObject> stones = new HashSet<GameObject>();

    private List<Rigidbody> stonesAtWork = new List<Rigidbody>();

    private Vector3 lastVec = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        _timer = cooldown;
        stoneExample.SetActive(false);
        breakVFXExample.SetActive(false);

        gm = GameManager.Instance;
        poolStones = new ObjectPool(10, stoneExample, transform);
        poolVFXStone = new ObjectPool(10, breakVFXExample, transform);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!gm.IsGameStarted) return;

        if (_timer > cooldown)
        {
            _timer = 0;
            

            if (stonesAtWork.Count > 0)
            {
                for (int i = 0; i < stonesAtWork.Count; i++)
                {
                    if (stonesAtWork[i].velocity.magnitude < 1 && !stones.Contains(stonesAtWork[i].gameObject))
                    {
                        stones.Add(stonesAtWork[i].gameObject);
                        StartCoroutine(breakAndReturn(stonesAtWork[i].gameObject));
                    }
                }
            }

            createStone();
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    private void createStone()
    {
        Vector3 pos = Vector3.zero;

        for (int i = 0; i < 100; i++)
        {
            pos = Vector3.Lerp(from.position, to.position, UnityEngine.Random.Range(0, 1f));

            if ((lastVec - pos).magnitude > 6.5f)
            {
                break;
            }
        }
        
        lastVec = pos;
        GameObject g = poolStones.GetObject();
        Rigidbody rb = g.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        stonesAtWork.Add(rb);
        
        g.transform.position = pos;
        g.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stone") && !stones.Contains(other.gameObject))
        {
            stones.Add(other.gameObject);
            StartCoroutine(breakAndReturn(other.gameObject));
        }
    }


    private IEnumerator breakAndReturn(GameObject g)
    {
        GameObject vfx = poolVFXStone.GetObject();
        vfx.transform.position = g.transform.position;
        vfx.SetActive(true);

        Rigidbody rb = g.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(0.2f);

        stonesAtWork.Remove(rb);
        poolStones.ReturnObject(g);
        stones.Remove(g);

        yield return new WaitForSeconds(1.3f);
        poolVFXStone.ReturnObject(vfx);
    }
}
