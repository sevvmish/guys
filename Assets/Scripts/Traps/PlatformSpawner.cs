using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private PlatformSystem example;
    [SerializeField] private Transform from;
    [SerializeField] private Transform to;


    // Start is called before the first frame update
    void Start()
    {
        GameObject g = Instantiate(example.gameObject, transform);
        g.SetActive(true);
        g.GetComponent<PlatformSystem>().SetPlay(from, to);
        
    }

}
