using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circus1Optimyzer : MonoBehaviour
{
    [SerializeField] private GameObject part1;
    [SerializeField] private GameObject part2;
    [SerializeField] private GameObject part3;
    [SerializeField] private GameObject part4;
    [SerializeField] private GameObject part5;
    [SerializeField] private GameObject part6;
    [SerializeField] private GameObject part7;

    private int previousPos;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        part1.SetActive(false);
        part2.SetActive(false);
        part3.SetActive(false);
        part4.SetActive(false);
        part5.SetActive(false);
        part6.SetActive(false);
        part7.SetActive(false);

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
        }
        else if (pos >= 6 && pos < 9)
        {            
            if (part2.activeSelf) part2.SetActive(false);
            if (!part3.activeSelf) part3.SetActive(true);
            if (!part4.activeSelf) part4.SetActive(true);            
        }
        else if (pos >= 9 && pos < 11)
        {
            if (part2.activeSelf) part2.SetActive(false);
            if (!part3.activeSelf) part3.SetActive(true);
            if (!part4.activeSelf) part4.SetActive(true);
            if (!part5.activeSelf) part5.SetActive(true);
        }
        else if (pos >= 11 && pos < 14)
        {            
            if (part3.activeSelf) part3.SetActive(false);
            if (!part4.activeSelf) part4.SetActive(true);
            if (!part5.activeSelf) part5.SetActive(true);
            if (!part6.activeSelf) part6.SetActive(true);
        }
        else if (pos >= 14 && pos < 16)
        {            
            if (part4.activeSelf) part4.SetActive(false);
            if (!part5.activeSelf) part5.SetActive(true);
            if (!part6.activeSelf) part6.SetActive(true);
            if (!part7.activeSelf) part7.SetActive(true);
        }
        else if (pos >= 16 )
        {            
            if (part5.activeSelf) part5.SetActive(false);
            if (!part6.activeSelf) part6.SetActive(true);
            if (!part7.activeSelf) part7.SetActive(true);
        }
    }

    
}
