using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusClient : MonoBehaviour
{
    [SerializeField] private int currentIndex = 0;
    [SerializeField] private GameObject endVFX;

    private bool isTaken;

    // Start is called before the first frame update
    void Start()
    {
        endVFX.SetActive(false);

        if (BonusManager.Instance != null)
        {            
            if (BonusManager.Instance.GetCurrentValue(currentIndex) > 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                BonusManager.Instance.AddPoint(currentIndex, this);
            }
        }              
    }

    
    private void OnTriggerEnter(Collider other)
    {        
        if (!isTaken && other.CompareTag("Player"))
        {
            isTaken = true;
            Globals.MainPlayerData.B[currentIndex] = 1;
            SaveLoadManager.Save();
            GameManager.Instance.GetUI().ShowInformer($"+1 {Globals.Language.PlusBonus}...");
            StartCoroutine(playVFX());
        }
    }

    private IEnumerator playVFX()
    {        
        endVFX.SetActive(true);
        transform.GetChild(0).DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(0.6f);                
        gameObject.SetActive(false);
    }
}
