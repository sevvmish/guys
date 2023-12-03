using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSignTrigger : MonoBehaviour
{
    private bool isPlaying;
    private AudioSource _audio;
    private float deltaX = 0.4f;
    private float deltaZ = 0.4f;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isPlaying && other.gameObject.layer == 9 || other.gameObject.layer == 3)
        {
            StartCoroutine(play());
        }
    }

    private IEnumerator play()
    {
        isPlaying = true;
        _audio.Play();
        transform.DOPunchPosition(new Vector3(UnityEngine.Random.Range(-deltaX, deltaX), 1.5f, UnityEngine.Random.Range(-deltaZ, deltaZ)), 0.5f, 10, 1f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(0.75f);
        isPlaying = false;
    }
}
