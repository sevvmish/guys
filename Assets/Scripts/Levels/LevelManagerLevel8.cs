using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerLevel8 : MonoBehaviour
{
    private float interval = 7;
    private float timeForPass = 9;
    private float up = 0.75f;

    [SerializeField] private GameObject laserExample;
    [SerializeField] private Transform location;

    [SerializeField] private Transform[] path1;
    [SerializeField] private Transform[] path2;
    [SerializeField] private Transform[] path3;
    [SerializeField] private Transform[] path4;
    [SerializeField] private Transform[] path5;

    [SerializeField] private GameObject[] pointGroupFar;
    [SerializeField] private GameObject[] pointGroupCloser;
    [SerializeField] private GameObject[] pointGroupVeryClose;

    private ObjectPool laserPool;
    private float _timer;
    private GameManager gm;
    private Transform lastPathStart;
    private int iterations;
    private bool isDouble = false;

    private bool isOne, isTwo, isThree;

    // Start is called before the first frame update
    void Start()
    {
        SetGroupBot(pointGroupFar, false);
        SetGroupBot(pointGroupCloser, false);
        SetGroupBot(pointGroupVeryClose, false);

        _timer = 4;
        gm = GameManager.Instance;
        laserExample.SetActive(false);
        laserPool = new ObjectPool(10, laserExample, location);
    }

    private void Update()
    {
        if (!gm.IsGameStarted) return;

        if (!isOne)
        {
            isOne = true;
            SetGroupBot(pointGroupFar, true);
        }

        if (!isTwo && gm.GameSecondsPlayed > 3f )
        {
            isTwo = true;
            
            SetGroupBot(pointGroupFar, false);
            SetGroupBot(pointGroupCloser, true);
        }

        if (_timer > interval)
        {
            _timer = 0;

            
            
            if (iterations < 2)
            {
                interval -= 0.3f;
                timeForPass -= 0.5f;

                startLaser(0, false);
            }
            else if (iterations < 5)
            {
                interval -= 0.15f;
                timeForPass -= 0.4f;

                startLaser(UnityEngine.Random.Range(0.2f, 0.6f), false);
                //startLaser(UnityEngine.Random.Range(0.5f, 1.2f), false);
            }
            else// if(iterations < 7)
            {
                if (!isThree)
                {
                    isThree = true;
                    SetGroupBot(pointGroupCloser, false);
                    SetGroupBot(pointGroupVeryClose, true);
                    interval = 4;
                    isDouble = true;
                }

                interval -= 0.1f;
                timeForPass -= 0.3f;

                print("timeforp: " + timeForPass + ", interval: " + interval);

                startLaser(UnityEngine.Random.Range(0.1f, 0.3f), false);
                //startLaser(UnityEngine.Random.Range(1f, 3f), true);
                //startLaser(UnityEngine.Random.Range(1f, 3f), true);
                //startLaser(UnityEngine.Random.Range(1f, 3f), true);
            }



            iterations++;
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    private void startLaser(float delay, bool isSamePath)
    {
        Transform[] path = default;

        for (int i = 0; i < 100; i++)
        {
            int rnd = UnityEngine.Random.Range(0, 5);

            switch (rnd)
            {
                case 0:
                    path = path1;
                    break;

                case 1:
                    path = path2;
                    break;

                case 2:
                    path = path3;
                    break;

                case 3:
                    path = path4;
                    break;

                case 4:
                    path = path5;
                    break;

                

            }

            if (isSamePath) break;

            if (lastPathStart == null || !lastPathStart.Equals(path[0]))
            {
                break;
            }
        }

        lastPathStart = path[0];

        StartCoroutine(play(path, timeForPass, delay));
    }
    private IEnumerator play(Transform[] t, float timeForWalking, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject g = laserPool.GetObject();
        g.SetActive(true);
        
        if (isDouble)
        {
            g.transform.GetChild(2).GetComponent<BotJumpDirectionTrigger>().IsDoubleJump = true;
        }

        g.transform.position = t[0].position + Vector3.up * up;
        g.transform.LookAt(t[1].position + Vector3.up * up);
        g.transform.DOMove(t[1].position + Vector3.up * up, timeForWalking).SetEase(Ease.Linear);

        yield return new WaitForSeconds(timeForWalking);

        laserPool.ReturnObject(g);
    }

    private void SetGroupBot(GameObject[] gr, bool isActive)
    {
        for (int i = 0; i < gr.Length; i++)
        {
            gr[i].SetActive(isActive);
        }
    }
}
