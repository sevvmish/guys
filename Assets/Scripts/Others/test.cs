using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private GameObject te;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 20; i+=2)
        {
            for (int ii = 0; ii < 20; ii+=2)
            {
                GameObject g = Instantiate(te);
                g.transform.position = new Vector3(ii-10, i - 10, 0);
                g.SetActive(true);
            }
            
        }
    }

}
