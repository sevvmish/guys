using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPlatformShort : MonoBehaviour
{
    [SerializeField] private Material material1;
    [SerializeField] private Material material2;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private GameObject vfx;
   
    private bool isStarted, isOne, isTwo, isThree;
    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer.material = material1;
        vfx.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            _timer += Time.deltaTime;

            if (_timer > 0.1f && !isOne)
            {
                isOne = true;
                meshRenderer.material = material2;                
                vfx.SetActive(true);
            }

            if (_timer > 0.5f && !isTwo)
            {
                isTwo = true;
                _collider.enabled = false;
                meshRenderer.enabled = false;
            }

            if (_timer > 1f && !isThree)
            {
                isThree = true;
                vfx.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
   

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !isStarted)
        {
            isStarted = true;
        }
    }
}
