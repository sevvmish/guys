using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5Helper : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float intensityOut = 1;
    [SerializeField] private float intensityIn = 0.3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl pc) && pc.IsItMainPlayer)
        {
            _light.DOIntensity(intensityIn, 1f).SetEase(Ease.Linear);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl pc) && pc.IsItMainPlayer)
        {
            _light.DOIntensity(intensityOut, 1f).SetEase(Ease.Linear);
        }
    }
}
