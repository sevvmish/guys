using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyBarrel : MonoBehaviour
{
    [SerializeField] private GameObject explosive;
    [SerializeField] private GameObject barrel;
    [SerializeField] private GameObject vfx;

    [SerializeField] private bool isAutoBlowable = false;
    [SerializeField] private float secondsToLive = 3;

    public bool IsBlown { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        explosive.SetActive(false);
        barrel.SetActive(true);
        vfx.SetActive(false);      
    }

    private void OnEnable()
    {
        if (isAutoBlowable)
        {
            StartCountdown();
        }
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
        explosive.transform.SetParent(transform.parent);
        yield return new WaitForSeconds(0.1f);

        barrel.SetActive(false);
        yield return new WaitForSeconds(1f);
        vfx.SetActive(false);
        gameObject.SetActive(false);
    }
        
    public void StartCountdown()
    {
        if (!isAutoBlowable || IsBlown) return;
        IsBlown = true;
        StartCoroutine(playTTL());
    }
    private IEnumerator playTTL()
    {
        yield return new WaitForSeconds(secondsToLive);
        StartCoroutine(playBlow());
    }
}
