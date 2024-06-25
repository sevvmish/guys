using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level18Helper : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform from;
    [SerializeField] private Transform to;

    [SerializeField] private GameObject[] easyObjects;
    [SerializeField] private GameObject[] mediumObjects;
    [SerializeField] private GameObject[] hardObjects;

    private float cooldown = 5f;
    private float speedTime = 10f;
    private float _timer = 2f;
    private float gameTimer = 0;

    private int lastRandomEasy = -1;
    private int prelastRandomEasy = -1;

    private int lastRandomMedium = -1;
    private int prelastRandomMedium = -1;

    private int lastRandomHard = -1;
    private int prelastRandomHard = -1;


    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        easyObjects.ToList().ForEach(x => x.SetActive(false));
        mediumObjects.ToList().ForEach(x => x.SetActive(false));
        hardObjects.ToList().ForEach(x => x.SetActive(false));
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.IsGameStarted) return;

        gameTimer += Time.deltaTime;



        if (_timer > cooldown)
        {
            _timer = 0;

            if (gameTimer < 20)
            {
                speedTime = 6.8f;
                cooldown = 5.6f;
                sendObject(getRandomEasy(), speedTime);
            }
            else if (gameTimer >= 20 && gameTimer < 50)
            {
                if (speedTime > 6f) speedTime -= 0.5f;
                if (cooldown > 4.5f) cooldown -= 0.3f;

                //print("speedTimne: " + speedTime + ", cooldown: " + cooldown);

                int rnd = UnityEngine.Random.Range(0, 5);

                switch(rnd)
                {                    
                    case 4:
                        sendObject(getRandomEasy(), speedTime);
                        break;

                    default:
                        sendObject(getRandomMedium(), speedTime);
                        break;
                }
            }
            else if (gameTimer >= 50)
            {
                speedTime = 6.9f;
                cooldown = 6.9f;

                sendObject(getRandomHard(), speedTime);

                int rnd = UnityEngine.Random.Range(0, 3);

                switch (rnd)
                {
                    case 0:
                        sendObjectWithDelay(getRandomMedium(), speedTime, 3f);
                        break;

                    default:
                        sendObjectWithDelay(getRandomEasy(), speedTime, 3f);
                        break;
                }
            }
            
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    private GameObject getRandomEasy()
    {
        int rnd = 0;

        for (int i = 0; i < 100; i++)
        {
            rnd = UnityEngine.Random.Range(0, easyObjects.Length);

            if (rnd != lastRandomEasy && rnd != prelastRandomEasy)
            {
                prelastRandomEasy = lastRandomEasy;
                lastRandomEasy = rnd;
                return easyObjects[rnd];
            }
        }

        return easyObjects[0];
    }

    private GameObject getRandomMedium()
    {
        int rnd = 0;

        for (int i = 0; i < 100; i++)
        {
            rnd = UnityEngine.Random.Range(0, mediumObjects.Length);

            if (rnd != lastRandomMedium && rnd != prelastRandomMedium)
            {
                prelastRandomMedium = lastRandomMedium;
                lastRandomMedium = rnd;
                return mediumObjects[rnd];
            }
        }

        return mediumObjects[0];
    }

    private GameObject getRandomHard()
    {
        int rnd = 0;

        for (int i = 0; i < 100; i++)
        {
            rnd = UnityEngine.Random.Range(0, hardObjects.Length);

            if (rnd != lastRandomHard && rnd != prelastRandomHard)
            {
                prelastRandomHard = lastRandomHard;
                lastRandomHard = rnd;
                return hardObjects[rnd];
            }
        }

        return hardObjects[0];
    }

    private void sendObject(GameObject example, float speed)
    {
        GameObject g = Instantiate(example, transform);
        g.transform.position = start.position;
        g.transform.eulerAngles = Vector3.zero;
        g.SetActive(true);

        g.transform.DOMove(from.position, 2).SetUpdate(UpdateType.Fixed).SetEase(Ease.Linear).OnComplete(() => { g.transform.DOMove(to.position, speed).SetUpdate(UpdateType.Fixed).SetEase(Ease.Linear).OnComplete(() => { g.SetActive(false); }); });
        
    }

    private void sendObjectWithDelay(GameObject example, float speed, float delay)
    {
        StartCoroutine(playSend(example, speed, delay));
    }
    private IEnumerator playSend(GameObject example, float speed, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject g = Instantiate(example, transform);
        g.transform.position = start.position;
        g.transform.eulerAngles = Vector3.zero;
        g.SetActive(true);

        g.transform.DOMove(from.position, 2).SetUpdate(UpdateType.Fixed).SetEase(Ease.Linear).OnComplete(() => { g.transform.DOMove(to.position, speed).SetUpdate(UpdateType.Fixed).SetEase(Ease.Linear).OnComplete(() => { g.SetActive(false); }); });
    }

    public void PullMeOut(Transform me, Transform myParent, GameObject g)
    {
        StartCoroutine(play(me, myParent, g));
    }
    private IEnumerator play(Transform me, Transform myParent, GameObject g)
    {
        yield return new WaitForSeconds(Time.deltaTime);
        g.SetActive(true);
        me.SetParent(myParent);
        g.SetActive(false);
    }
}
