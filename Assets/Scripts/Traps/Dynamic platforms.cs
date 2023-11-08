using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Dynamicplatforms : MonoBehaviour
{
    [SerializeField] private GameObject examplePlatform;
    [SerializeField] private float speedTime = 8;
    [SerializeField] private float timer1 = 4;

    [SerializeField] private Transform line1From;
    [SerializeField] private Transform line1To;
    [SerializeField] private int index1 = 2;
    

    [SerializeField] private Transform line2From;
    [SerializeField] private Transform line2To;
    [SerializeField] private int index2 = 3;

    [SerializeField] private Transform line3From;
    [SerializeField] private Transform line3To;
    [SerializeField] private int index3 = 4;

    [SerializeField] private Transform line4From;
    [SerializeField] private Transform line4To;
    [SerializeField] private int index4 = 5;

    private ObjectPool examples;

    // Start is called before the first frame update
    void Start()
    {
        examplePlatform.SetActive(false);
        examples = new ObjectPool(20, examplePlatform, transform);

        StartCoroutine(playLine1(0, line1From, line1To, index1));
        StartCoroutine(playLine1(timer1, line1From, line1To, index1));

        StartCoroutine(playLine1(0, line2From, line2To, index2));
        StartCoroutine(playLine1(timer1, line2From, line2To, index2));

        StartCoroutine(playLine1(0, line3From, line3To, index3));
        StartCoroutine(playLine1(timer1, line3From, line3To, index3));

        StartCoroutine(playLine1(0, line4From, line4To, index4));
        StartCoroutine(playLine1(timer1, line4From, line4To, index4));
    }

    private IEnumerator playLine1(float delay, Transform from, Transform to, int index)
    {
        yield return new WaitForSeconds(delay);
        GameObject g = default;
        List<PlayerControl> players = new List<PlayerControl>();

        while (true)
        {
            g = examples.GetObject();
            g.transform.position = from.position;
            g.SetActive(true);
            g.transform.GetChild(0).gameObject.GetComponent<BotNavPoint>().SetNewIndex(index);
            g.transform.DOMove(to.position, speedTime).SetEase(Ease.Linear);
            yield return new WaitForSeconds(speedTime);
            players.Clear();
            for (int i = 0; i < g.transform.childCount; i++)
            {
                if (g.transform.GetChild(i).TryGetComponent(out PlayerControl p))
                {
                    players.Add(p);
                }
            }
            examples.ReturnObject(g);

            for (int i = 0; i < players.Count; i++)
            {
                players[i].FreePlatformStatusForPlayer();
            }
        }
    }
}
