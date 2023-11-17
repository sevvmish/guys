using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientMusic : MonoBehaviour
{
    public static AmbientMusic Instance { get; private set; }

    private AudioSource _audio;

    [SerializeField] private AudioClip levelIntro;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _audio = GetComponent<AudioSource>();
    }


    public void PlayAmbient(AmbientMelodies _type)
    {
        _audio.pitch = 1;
        _audio.volume = 0.6f;

        switch (_type)
        {
            case AmbientMelodies.level_intro:
                _audio.Stop();
                _audio.clip = levelIntro;
                _audio.Play();
                break;


        }
    }
}

public enum AmbientMelodies
{
    level_intro
}
