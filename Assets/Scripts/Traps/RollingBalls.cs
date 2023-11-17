using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBalls : MonoBehaviour
{
    [SerializeField] private GameObject ball;

    [Header("Line A")]
    [SerializeField] private Transform[] posA;
    [SerializeField] private float[] timeA;
    [SerializeField] private float delayA;
    [SerializeField] private float continuetyA;
    private float _timer1;


    [Header("Line B")]
    [SerializeField] private Transform[] posB;
    [SerializeField] private float[] timeB;
    [SerializeField] private float delayB;
    [SerializeField] private float continuetyB;
    private float _timer2;


    [Header("Line C")]
    [SerializeField] private Transform[] posC;
    [SerializeField] private float[] timeC;
    [SerializeField] private float delayC;
    [SerializeField] private float continuetyC;
    private float _timer3;

    private ObjectPool balls;

    // Start is called before the first frame update
    void Start()
    {
        balls = new ObjectPool(30, ball, transform);
        _timer1 = delayA;
        _timer2 = delayB;
        _timer3 = delayC;
    }

    private void Update()
    {
        if (_timer1 > continuetyA)
        {
            _timer1 = 0;
            StartCoroutine(playLine(posA, timeA, delayA));
        }
        else
        {
            _timer1 += Time.deltaTime;
        }

        if (_timer2 > continuetyB)
        {
            _timer2 = 0;
            StartCoroutine(playLine(posB, timeB, delayB));
        }
        else
        {
            _timer2 += Time.deltaTime;
        }

        if (_timer3 > continuetyA)
        {
            _timer3 = 0;
            StartCoroutine(playLine(posC, timeC, delayC));
        }
        else
        {
            _timer3 += Time.deltaTime;
        }
    }

    private IEnumerator playLine(Transform[] pos, float[] times, float delay)
    {
        //yield return new WaitForSeconds(delay);
        Rigidbody g = balls.GetObject().GetComponent<Rigidbody>();        
        g.transform.position = pos[0].position;
        g.gameObject.SetActive(true);

        for (int i = 1; i < pos.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);
            g.transform.LookAt(pos[i].position);
            g.DOMove(pos[i].position, times[i]).SetEase(Ease.Linear);
            yield return new WaitForSeconds(times[i]);
        }

        MeshRenderer ballRendered = g.GetComponent<MeshRenderer>();
        ballRendered.enabled = false;
        g.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        g.gameObject.SetActive(false);
        ballRendered.enabled = true;
        g.transform.GetChild(1).gameObject.SetActive(false);

        balls.ReturnObject(g.gameObject);
    }
}
