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
            //if (cooldown>=5) cooldown -= 1f;

            if (gameTimer < 20)
            {
                speedTime = 10f;
                cooldown = 6f;
                sendObject(easyObjects[UnityEngine.Random.Range(0, easyObjects.Length)], speedTime);
            }
            else if (gameTimer >= 20 && gameTimer < 40)
            {
                speedTime = 8f;
                cooldown = 5f;
                sendObject(mediumObjects[UnityEngine.Random.Range(0, mediumObjects.Length)], speedTime);
            }
            else if (gameTimer >= 40 && gameTimer < 60)
            {
                speedTime = 7f;
                cooldown = 4f;
                sendObject(hardObjects[UnityEngine.Random.Range(0, hardObjects.Length)], speedTime);
                sendObjectWithDelay(easyObjects[UnityEngine.Random.Range(0, easyObjects.Length)], speedTime, 2f);
            }
        }
        else
        {
            _timer += Time.deltaTime;
        }
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
