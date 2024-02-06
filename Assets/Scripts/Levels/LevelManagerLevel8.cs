using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerLevel8 : MonoBehaviour
{
    [SerializeField] private GameObject laserExample;
    [SerializeField] private Transform location;

    private ObjectPool laserPool;

    // Start is called before the first frame update
    void Start()
    {
        laserExample.SetActive(false);
        laserPool = new ObjectPool(5, laserExample, location);

    }

    
}
