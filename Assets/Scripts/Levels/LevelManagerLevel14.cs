using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManagerLevel14 : MonoBehaviour
{
    [SerializeField] private Transform location;
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
    private int lastRandomMedium = -1;
    private int lastRandomHard = -1;


    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        easyObjects.ToList().ForEach(x=>x.SetActive(false));
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
                speedTime = 9.5f;
                cooldown = 5f;
                sendObject(getRandomEasy(), speedTime);
            }
            else if (gameTimer >= 20 && gameTimer < 40)
            {
                speedTime = 6.5f;
                if (cooldown > 3f) cooldown -= 0.5f;
                sendObject(getRandomMedium(), speedTime);
            }
            else if (gameTimer >= 40 && gameTimer < 60)
            {
                speedTime = 8f;
                cooldown = 5f;
                sendObject(getRandomMedium(), speedTime);
                sendObjectWithDelay(getRandomEasy(), speedTime, 2.5f);
            }
            else if (gameTimer >= 60 && gameTimer < 75)
            {
                speedTime = 7f;
                cooldown = 4f;
                sendObject(getRandomHard(), speedTime);
            }
            else if (gameTimer >= 75)
            {
                speedTime = 8.5f;
                cooldown = 5.5f;
                sendObject(getRandomHard(), speedTime);

                int rnd = UnityEngine.Random.Range(0, 2);

                if (rnd == 0)
                {
                    sendObjectWithDelay(getRandomEasy(), speedTime, 2.5f);
                }
                else
                {
                    sendObjectWithDelay(getRandomMedium(), speedTime, 2.5f);
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

            if (rnd != lastRandomEasy)
            {
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

            if (rnd != lastRandomMedium)
            {
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

            if (rnd != lastRandomHard)
            {
                lastRandomHard = rnd;
                return hardObjects[rnd];
            }
        }

        return hardObjects[0];
    }

    private void sendObject(GameObject example, float speed)
    {
        GameObject g = Instantiate(example, location);
        g.transform.position = from.position;
        g.transform.eulerAngles = Vector3.zero;
        g.SetActive(true);

        g.transform.DOMove(to.position, speed).SetUpdate(UpdateType.Fixed).SetEase(Ease.Linear).OnComplete(() => { g.SetActive(false); });
    }

    private void sendObjectWithDelay(GameObject example, float speed, float delay)
    {
        StartCoroutine(playSend(example, speed, delay));
    }
    private IEnumerator playSend(GameObject example, float speed, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject g = Instantiate(example, location);
        g.transform.position = from.position;
        g.transform.eulerAngles = Vector3.zero;
        g.SetActive(true);

        g.transform.DOMove(to.position, speed).SetUpdate(UpdateType.Fixed).SetEase(Ease.Linear).OnComplete(() => { g.SetActive(false); });
    }
}
