using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circus1Optimyzer : MonoBehaviour
{
    [SerializeField] private GameObject part1;
    [SerializeField] private GameObject part2;
    [SerializeField] private GameObject part3;
    [SerializeField] private GameObject part4;

    private int previousPos;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.05f);

        int currentPos = RespawnManager.Instance.GetCurrentIndex;
        previousPos = currentPos;
        checkState(currentPos);

    }

    private void Update()
    {
        if (RespawnManager.Instance.GetCurrentIndex != previousPos)
        {
            previousPos = RespawnManager.Instance.GetCurrentIndex;
            print(previousPos);
            checkState(previousPos);
        }
    }

    private void checkState(int pos)
    {        
        if (pos < 4)
        {
            if (!part1.activeSelf) part1.SetActive(true);
            if (!part2.activeSelf) part2.SetActive(true);
            if (part3.activeSelf) part3.SetActive(false);
            if (part4.activeSelf) part4.SetActive(false);
        }
        else if (pos >= 4 && pos < 5)
        {
            if (!part1.activeSelf) part1.SetActive(true);
            if (!part2.activeSelf) part2.SetActive(true);
            if (!part3.activeSelf) part3.SetActive(true);
            if (part4.activeSelf) part4.SetActive(false);
        }
        else if (pos >= 5 && pos < 6)
        {
            if (part1.activeSelf) part1.SetActive(false);
            if (!part2.activeSelf) part2.SetActive(true);
            if (!part3.activeSelf) part3.SetActive(true); 
            if (!part4.activeSelf) part4.SetActive(true);

            print(part4.activeSelf);
        }
        else if (pos >= 6)
        {
            if (part1.activeSelf) part1.SetActive(false);
            if (part2.activeSelf) part2.SetActive(false);
            if (!part3.activeSelf) part3.SetActive(true);
            if (!part4.activeSelf) part4.SetActive(true);
            print(part4.activeSelf);
        }
    }

    
}
