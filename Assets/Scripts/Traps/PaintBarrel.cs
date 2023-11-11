using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBarrel : MonoBehaviour
{
    [SerializeField] private GameObject paint;
    [SerializeField] private GameObject barrel;
    [SerializeField] private GameObject vfx;
    [SerializeField] private float patchDuration;

    public bool IsBlown { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        paint.SetActive(false);
        barrel.SetActive(true);
        vfx.SetActive(false);
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
        paint.SetActive(true);
        paint.GetComponent<PaintPatch>().Setdata(Vector3.zero, 0.5f, 3, patchDuration);
        yield return new WaitForSeconds(0.1f);
    
        barrel.SetActive(false);
        yield return new WaitForSeconds(1f);
        vfx.SetActive(false);
    }
}
