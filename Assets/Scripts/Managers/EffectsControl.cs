using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsControl : MonoBehaviour
{
    [SerializeField] private GameObject shadow;

    // Start is called before the first frame update
    void Start()
    {
        shadow.SetActive(false);
    }

    public void SetShadow(bool isActive) => shadow.SetActive(isActive);
}
