using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyBarrel : MonoBehaviour
{
    [Header("Dropping")]
    [SerializeField] private GameObject parashute;
    public bool IsParashute = false;
    public float Distance = 5f;
    public float FlyTime = 5f;
    private Vector3 initPos;

    [Header("Main")]
    [SerializeField] private GameObject explosive;
    [SerializeField] private GameObject barrel;
    [SerializeField] private GameObject vfx;

    [SerializeField] private bool isAutoBlowable = false;
    [SerializeField] private float secondsToLive = 3;

    public bool IsBlown { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        parashute.transform.localScale = Vector3.one;
        explosive.SetActive(false);
        barrel.SetActive(true);
        vfx.SetActive(false);      
    }

    private void OnEnable()
    {                     
        if (IsParashute)
        {
            StartCoroutine(playParashute());
        }

        IsBlown = false;
        explosive.SetActive(false);
        barrel.SetActive(true);
        vfx.SetActive(false);

        explosive.transform.localPosition = Vector3.zero;
        explosive.transform.localEulerAngles = Vector3.zero;
    }

    private void OnDisable()
    {        
        barrel.SetActive(true);
        vfx.SetActive(false);
        StopAllCoroutines();
        IsBlown = false;
    }

    public void SetParashute(float dist, float time, float blowAfter)
    {
        parashute.transform.localScale = Vector3.one;
        explosive.SetActive(false);
        barrel.SetActive(true);
        vfx.SetActive(false);

        Distance = dist;
        FlyTime = time;

        IsParashute=true;
        secondsToLive = blowAfter;
    }
    private IEnumerator playParashute()
    {        
        parashute.SetActive(true);
        initPos = transform.position;

        bool isPaint = explosive.TryGetComponent(out PaintPatch paint);

        for (float i = 0; i < Distance; i+=0.05f)
        {
            parashute.SetActive(true);

            transform.position = new Vector3(initPos.x, initPos.y - i, initPos.z);
            parashute.transform.Rotate(Vector3.up, 7);
            yield return new WaitForSeconds(FlyTime / (Distance / 0.05f));

            if (IsBlown) 
            {
                parashute.transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.OutQuad);
                if (isPaint)
                {                    
                    transform.position = initPos - Vector3.up * Distance;
                    explosive.transform.position = initPos - Vector3.up * Distance;
                }
                yield break;
            }
        }
                
        

        parashute.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutQuad);
        isAutoBlowable = true;
        StartCountdown();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !IsBlown)
        {
            IsBlown = true;
            StartCoroutine(playBlow());
        }
    }

    private IEnumerator playBlow()
    {
        vfx.SetActive(true);
        explosive.SetActive(true);
        explosive.transform.SetParent(GameManager.Instance.GetVFX());
        yield return new WaitForSeconds(0.1f);

        barrel.SetActive(false);
        yield return new WaitForSeconds(1f);
        vfx.SetActive(false);
        explosive.transform.SetParent(transform);
        
        while (explosive.activeSelf)
        {
            yield return new WaitForSeconds(0.1f);
        }

        gameObject.SetActive(false);
    }

        
    public void StartCountdown()
    {
        if (!isAutoBlowable || IsBlown) return;
        
        StartCoroutine(playTTL());
    }
    private IEnumerator playTTL()
    {
        yield return new WaitForSeconds(secondsToLive);
        if (!IsBlown)
        {
            IsBlown = true;
            StartCoroutine(playBlow());
        }
        
    }
}
