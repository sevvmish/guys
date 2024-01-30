using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLevel6 : MonoBehaviour
{
    [SerializeField] private Transform visualT;
    [SerializeField] private Material materialIdle;
    [SerializeField] private Material materialAct;
    [SerializeField] private MeshRenderer mr;
    [SerializeField] private Collider _collider;
    [SerializeField] private GameObject pointForBot;
    [SerializeField] private GameObject VFX;

    private bool isTurnedOnPoint;
    private bool isActivated;
    private AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();

        VFX.SetActive(false);
        isTurnedOnPoint = true;
        mr.material = materialIdle;
        _collider.enabled = true;
        pointForBot.SetActive(true);
        isActivated = false;
    }

    public void PrepareTurn(float _time)
    {        
        pointForBot.SetActive(false);
        isActivated = true;
        mr.material = materialAct;
        transform.DOPunchPosition(new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), 0.3f, UnityEngine.Random.Range(-0.1f, 0.1f)), _time, 30).SetEase(Ease.OutQuad);
    }

    public void MakeTurn()
    {
        _audio.Play();
        VFX.SetActive(true);
        isActivated = true;
        visualT.DORotate(new Vector3(0,0,-90), 0.3f).SetEase(Ease.OutSine);
        _collider.enabled = false;
    }

    public void Return()
    {
        _audio.Play();
        VFX.SetActive(false);
        visualT.DORotate(new Vector3(0, 0, 0), 0.3f).SetEase(Ease.OutSine);
        mr.material = materialIdle;
        _collider.enabled = true;
        isActivated = false;
        pointForBot.SetActive(isTurnedOnPoint);
    }

    public void SetPoint(bool isActive)
    {
        if (!isActivated)
        {
            isTurnedOnPoint = isActive;
            pointForBot.SetActive(isTurnedOnPoint);
        }
        else
        {
            print("error, non change while acting");
        }
        
    }
}
