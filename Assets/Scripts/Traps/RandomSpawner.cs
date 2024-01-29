using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] locations;
    [SerializeField] private Transform aim;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        int rnd = UnityEngine.Random.Range(0, locations.Length);
        aim.position = locations[rnd].position;
        aim.eulerAngles = locations[rnd].eulerAngles;
    }

    
}
