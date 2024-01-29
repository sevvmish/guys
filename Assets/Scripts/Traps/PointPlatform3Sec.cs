using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPlatform3Sec : MonoBehaviour
{
    [SerializeField] private GameObject state1;
    [SerializeField] private GameObject state2;
    [SerializeField] private GameObject state3;
    [SerializeField] private GameObject vfx;
    [SerializeField] private GameObject point;
    [SerializeField] private float recovery = 3f;
    [SerializeField] private float _state1base = 0.4f;
    [SerializeField] private float _state2base = 0.5f;
    [SerializeField] private float _state3base = 0.6f;
    [SerializeField] private BoxCollider[] boxes;
    

    private bool isState1 = true;
    private bool isState2 = false;
    private bool isState3 = false;
    private bool isStateOff = false;
    private float _timer = 0, _state1, _state2, _state3;
    private readonly float delay = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        state1.SetActive(true);
        state2.SetActive(false);
        state3.SetActive(false);
        point.SetActive(true);
        vfx.SetActive(false);
        setBoxes(true);
        _state1 = _state1base; 
        _state2 = _state2base; 
        _state3 = _state3base;
    }

    private void Update()
    {
        if (_timer > 0) _timer -= Time.deltaTime;
      
        if (isStateOff && _timer <= 0)
        {
            isState1 = true;
            isStateOff = false;
            
            vfx.SetActive(false);
            vfx.SetActive(true);

            state1.SetActive(true);
            setBoxes(true);
            point.SetActive(true);

            _state1 = _state1base;
            _state2 = _state2base;
            _state3 = _state3base;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (_timer > 0) return;

        if (other.CompareTag("Player") && isState1)
        {
            if (_state1 > 0)
            {
                _state1 -= Time.deltaTime;
            }
            else
            {
                stage1();
            }
        }
        else if (other.CompareTag("Player") && isState2)
        {
            if (_state2 > 0)
            {
                _state2 -= Time.deltaTime;
            }
            else
            {
                stage2();
            }
        }
        else if (other.CompareTag("Player") && isState3)
        {
            if (_state3 > 0)
            {
                _state3 -= Time.deltaTime;
            }
            else
            {
                stage3();
            }
        }


    }

    private void stage1()
    {
        isState1 = false;
        isState2 = true;
        _timer = delay;

        vfx.SetActive(false);
        vfx.SetActive(true);

        state1.SetActive(false);
        state2.SetActive(true);
    }

    private void stage2()
    {
        isState2 = false;
        isState3 = true;
        _timer = delay;

        vfx.SetActive(false);
        vfx.SetActive(true);

        state2.SetActive(false);
        state3.SetActive(true);
    }

    private void stage3()
    {
        if (recovery == 0)
        {
            point.SetActive(false);
            state3.SetActive(false);
            setBoxes(false);            
            return;
        }


        isState3 = false;
        isStateOff = true;
        _timer = recovery;

        vfx.SetActive(false);
        vfx.SetActive(true);

        state3.SetActive(false);
        setBoxes(false);
        point.SetActive(false);

        
    }

    private void setBoxes(bool isActive)
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].enabled = isActive;
        }
    }
}
