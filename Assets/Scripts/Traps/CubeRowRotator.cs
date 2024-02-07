using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRowRotator : MonoBehaviour
{
    [SerializeField] private Rigidbody[] cubesTransform;
    [SerializeField] private float[] delays;
    [SerializeField] private float[] cooldowns;
    [SerializeField] private GameObject[] jumpers;

    [SerializeField] private float startDelay = 0;
    [SerializeField] private float rotateSpeed = 1;
    [SerializeField] private float rotationAmount = 90;  

    List<CubesForRotation> cubes = new List<CubesForRotation>();
    private float _startTimer;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        for (int i = 0; i < cubesTransform.Length; i++)
        {
            cubes.Add(new CubesForRotation(delays[i], cooldowns[i], cubesTransform[i], jumpers[i]));
        }

        cubes[1]._timer = 12;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.IsGameStarted) return;

        if (_startTimer < startDelay)
        {
            _startTimer += Time.deltaTime;
            return;
        }

        for (int i = 0; i < cubes.Count; i++)
        {            
            if (!cubes[i].isOK)
            {
                if (cubes[i]._timer >= cubes[i]._delay)
                {
                    cubes[i]._timer = cubes[i]._cooldown;
                    cubes[i].isOK = true;                    
                }
                else
                {
                    cubes[i]._timer += Time.deltaTime;
                    continue;
                }
            }

            if (cubes[i]._timerJumper > 0)
            {
                cubes[i]._timerJumper-= Time.deltaTime;
            }
            else
            {
                if (cubes[i]._jumper.activeSelf) cubes[i]._jumper.SetActive(false);
            }

            if (cubes[i]._timer >= cubes[i]._cooldown)
            {
                cubes[i]._timer = 0;
                cubes[i]._jumper.SetActive(true);
                cubes[i]._timerJumper = rotateSpeed * 0.75f;

                int rnd = UnityEngine.Random.Range(0, 2);

                float direction = rnd == 0 ? rotationAmount : -rotationAmount;

                cubes[i]._rigidbody.DORotate(cubes[i]._rigidbody.transform.eulerAngles + new Vector3(0, 0, direction), rotateSpeed).SetEase(Ease.Linear).SetUpdate(UpdateType.Fixed);
            }
            else
            {
                cubes[i]._timer += Time.deltaTime;
            }
        }
    }

    public class CubesForRotation
    {
        public float _timer;
        public float _timerJumper;
        public float _delay;
        public float _cooldown;
        public Rigidbody _rigidbody;
        public bool isOK;
        public GameObject _jumper;

        public CubesForRotation(float delay, float cooldown, Rigidbody t, GameObject jumper)
        {
            _timer = 0;
            _timerJumper = 0;
            _delay = delay;
            _cooldown = cooldown;
            _rigidbody = t;
            isOK = false;
            _jumper = jumper;
            _jumper.SetActive(false);
        }
    }
}
