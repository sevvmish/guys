using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AbilityProvider : MonoBehaviour
{
    public AbilityTypes CurrentAbility;

    [SerializeField] private float Cooldown = 10;
    [SerializeField] private GameObject currentVisual;
    [SerializeField] private GameObject acceleration;
    [SerializeField] private GameObject rocketBack;
    [SerializeField] private GameObject VFX;

    
    private BoxCollider mainCollider;

    private float _timer;
    private bool isActive;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        mainCollider = GetComponent<BoxCollider>();

        acceleration.SetActive(false);
        rocketBack.SetActive(false);
        VFX.SetActive(false);

        switch (CurrentAbility)
        {
            case AbilityTypes.Acceleration:
                acceleration.transform.SetParent(currentVisual.transform);
                acceleration.transform.localPosition = Vector3.zero;
                acceleration.SetActive(true);
                break;

            case AbilityTypes.RocketBack:
                rocketBack.transform.SetParent(currentVisual.transform);
                rocketBack.transform.localPosition = Vector3.zero;
                rocketBack.SetActive(true);
                break;
        }

        Activate();
    }

    public void Activate()
    {
        isActive = true;
        _timer = 0;
        currentVisual.SetActive(true);
        mainCollider.enabled = true;
    }

    public void Deactivate()
    {
        isActive = false;
        _timer = 0;
        currentVisual.SetActive(false);
        mainCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.IsGameStarted) return;

        if (!isActive)
        {
            if (_timer > Cooldown)
            {
                Activate();
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }
    }
}
