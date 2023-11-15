using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("game intro")]
    [SerializeField] private Transform[] previewTransform;
    [SerializeField] private float[] previewTime;
    [SerializeField] private GameCountDown countDown;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            countDown.StartCountDown();
        }
    }
}
