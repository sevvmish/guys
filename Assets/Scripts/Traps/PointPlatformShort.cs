using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPlatformShort : MonoBehaviour
{
    [SerializeField] private GameObject state1;
    [SerializeField] private GameObject state2;
    [SerializeField] private GameObject state3;
    [SerializeField] private GameObject vfx;
    [SerializeField] private GameObject point;    
    private float _state1base = 0.2f;
    private float _state2base = 0.2f;
    private float _state3base = 0.2f;

    private bool isStarted;
    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        state1.SetActive(true);
        state2.SetActive(false);
        state3.SetActive(false);
        point.SetActive(true);
        vfx.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            _timer += Time.deltaTime;

            if (_timer > _state1base && state1.activeSelf)
            {
                state1.SetActive(false);
                state2.SetActive(true);
                vfx.SetActive(false);
                vfx.SetActive(true);
            }

            if (_timer > (_state1base + _state2base) && state2.activeSelf)
            {                
                state2.SetActive(false);
                state3.SetActive(true);
                vfx.SetActive(false);
                vfx.SetActive(true);
            }

            if (_timer > (_state1base + _state2base + _state3base) && state3.activeSelf)
            {
                state3.SetActive(false);                
                vfx.SetActive(false);
                vfx.SetActive(true);
            }

            if (_timer > (_state1base + _state2base + _state3base + 0.2f) && !state3.activeSelf && !state1.activeSelf && !state1.activeSelf)
            {                
                point.SetActive(false);
                this.gameObject.SetActive(false);
            }
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isStarted)
        {
            isStarted = true;
        }
    }
}
