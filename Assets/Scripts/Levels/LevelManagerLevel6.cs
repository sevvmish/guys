using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerLevel6 : MonoBehaviour
{
    [SerializeField] private GameObject platformExample;
    [SerializeField] private float prepareTimer = 2.5f;
    [SerializeField] private float stayDownTimer = 1.5f;
    [SerializeField] private float waitTimer = 4;

    [SerializeField] private GameObject[] alarms;

    private PlatformLevel6[] platforms;
    private Vector2 platformAmount = new Vector2 (6, 6);
    private float platformWidth = 4;

    private float _timer, _timerAct;
    private bool isActStarted, isFirstReorder;
    private float _cooldown;
    private int iterationAmount;

    private HashSet<int> actedPlatforms = new HashSet<int> ();

    int[] one = new int[] { 0, 5, 6, 11 };//{ 1, 7, 12, 25, 32, 4, 10, 17, 28, 33 };
    int[] two = new int[] { 30, 35 };//{ 12, 18, 32, 27, 22, 11 };
    int[] three = new int[] { 0, 6, 32 };//{ 18, 25, 31, 17, 10, 9, 8 };
    int[] four = new int[] { 17, 23, 28 };//{ 15, 16, 20, 27, 34, 25 };
    int[] five = new int[] { 2, 1, 18 };
    int[] six = new int[] { 0,1,2,3,4,5,6,11,17,23,29,35,30,31,32,33,34,12,18,24 };
    int[] seven = new int[] { 14,15,20,21 };

    private GameManager gm;

    // Start is called before the first frame update
    void Awake()
    {
        platformExample.SetActive(false);

        platforms = new PlatformLevel6[36];
        int index = 0;

        float limit = platformWidth / 2 + 2 * platformWidth;
        
        for (float x = -(limit); x <= (limit); x+= platformWidth)
        {
            for (float z = -(limit); z <= (limit); z += platformWidth)
            {
                GameObject g = Instantiate(platformExample, transform);
                g.SetActive(true);
                g.name = index.ToString();
                g.transform.position = new Vector3 (x, 0, z);
                platforms[index] = g.GetComponent<PlatformLevel6>();
                index++;
            }
        }

        setAllAlarms(false);
    }

    private void Start()
    {
        gm = GameManager.Instance;        
    }

    private void Update()
    {
        if (!gm.IsGameStarted) return;

        if (!isFirstReorder)
        {
            isFirstReorder = true;
            StartCoroutine(playChangeAfterSec(new float[] { 0.15f, 1.4f, 1.4f, 0.6f }, new int[] { 6, 7, 6, 0 }));
        }

        if (isActStarted)
        {
            if (_timerAct > (prepareTimer + stayDownTimer))
            {
                isActStarted = false;
                _timerAct = 0;
                setAllAlarms(false);
                //changeBotPoints();

                StartCoroutine(playChangeAfterSec(new float[] { 0.15f, 1.4f, 1.4f, 0.6f }, new int[] { 6, 7, 6, 0 }));
            }
            else
            {
                _timerAct += Time.deltaTime;
            }
        }
        else
        {
            if (_timer > waitTimer)
            {
                isActStarted = true;
                _timer = 0;
                iterationAmount++;
                setAllAlarms(true);
                //print(iterationAmount + " !!!!!!!!!!!!!!!!!!!!!!!!");
                switch (iterationAmount)
                {
                    case 1:
                        startRandomPlatformActing(12);
                        break;

                    case 2:
                        startRandomPlatformActing(18);
                        break;

                    case 3:
                        startRandomPlatformActing(24);
                        break;

                    case 4:
                        startRandomPlatformActing(28);
                        break;

                    case 5:
                        startRandomPlatformActing(30);
                        break;

                    case 6:
                        startRandomPlatformActing(33);
                        break;
                }
                
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }


        if (Input.GetKeyDown(KeyCode.B)) 
        {
            playPlatform(platforms[14], prepareTimer, stayDownTimer);
            playPlatform(platforms[18], prepareTimer, stayDownTimer);
            playPlatform(platforms[22], prepareTimer, stayDownTimer);
            playPlatform(platforms[12], prepareTimer, stayDownTimer);
            playPlatform(platforms[5], prepareTimer, stayDownTimer);
            playPlatform(platforms[28], prepareTimer, stayDownTimer);
        }
    }

    private void startRandomPlatformActing(int Amount)
    {        
        setAllPlatformPoints(true);
        actedPlatforms.Clear();

        for (int i = 0; i < Amount; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                int rnd = UnityEngine.Random.Range(0, platforms.Length);
                if (!actedPlatforms.Contains(rnd))
                {
                    actedPlatforms.Add(rnd);
                    playPlatform(platforms[rnd], prepareTimer, stayDownTimer);
                    break;
                }
            }            
        }
    }

    private void playPlatform(PlatformLevel6 platform, float prepareTime, float downTime)
    {
        StartCoroutine(play(platform, prepareTime, downTime));
    }
    private IEnumerator play(PlatformLevel6 platform, float prepareTime, float downTime)
    {
        platform.PrepareTurn(prepareTime);
        yield return new WaitForSeconds(prepareTime + 0.1f);

        platform.MakeTurn();
        yield return new WaitForSeconds(downTime);

        platform.Return();
    }

    private void changeBotPoints()
    {
        print("start changing bor points");
        StartCoroutine(playChange());        
    }
    private IEnumerator playChange()
    {
        yield return new WaitForSeconds(0.3f);

        int oneN = UnityEngine.Random.Range(1, 6);

        if (iterationAmount == 0 || iterationAmount > 2) oneN = 6;

        int twoN = 0;

        for (int i = 0; i < 100; i++)
        {
            twoN = UnityEngine.Random.Range(1, 6);
            if (oneN != twoN) break;
        }

        
        setPlatformsON(oneN);
        yield return new WaitForSeconds(1.5f);

        
        setPlatformsON(twoN);
        yield return new WaitForSeconds(1.5f);


    }

    private IEnumerator playChangeAfterSec(float sec, int number)
    {
        yield return new WaitForSeconds(sec);
        
        if (number == 0)
        {
            setAllPlatformPoints(true);
        }
        else
        {
            setPlatformsON(number);
        }        
        
    }

    private IEnumerator playChangeAfterSec(float[] seconds, int[] numbers  )
    {
        for (int i = 0; i < seconds.Length; i++)
        {
            yield return new WaitForSeconds(seconds[i]);
            int number = numbers[i];

            if (number == 0)
            {
                setAllPlatformPoints(true);
            }
            else
            {
                setPlatformsON(number);
            }
        }
                
    }

    private void setPlatformsON(int number)
    {
        setAllPlatformPoints(false);
        //print("number is: " + number);

        int[] arr = new int[0];

        switch(number)
        {
            case 1:
                arr = one;
                break;

            case 2:
                arr = two;
                break;

            case 3:
                arr = three;
                break;

            case 4:
                arr = four;
                break;

            case 5:
                arr = five;
                break;

            case 6:
                arr = six;
                break;

            case 7:
                arr = seven;
                break;
        }

        //print("activating points...:");
        for (int i = 0; i < arr.Length; i++)
        {
            //print("point #:" + arr[i]);
            platforms[arr[i]].SetPoint(true);
        }
    }

    private void setAllPlatformPoints(bool isActive)
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i].SetPoint(isActive);
        }
    }

    private void setAllAlarms(bool isActive)
    {
        for (int i = 0; i < alarms.Length; i++)
        {
            alarms[i].SetActive(isActive);
        }
    }
}
