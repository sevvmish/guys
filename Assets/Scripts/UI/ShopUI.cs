using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private GameObject back;
    [SerializeField] private RectTransform mainRect;
    [SerializeField] private RectTransform location;

    private bool isReady;

    // Start is called before the first frame update
    void Start()
    {
        back.SetActive(false);
        mainMenu.OnBackToMainMenu += ReturnBack;
                
    }

    public void SetOn()
    {        
        back.SetActive(true);
        
        mainMenu.MainPlayerSkin.SetActive(false);

        location.anchoredPosition = new Vector2(5000, 0);
    }

    private IEnumerator play()
    {
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < 5000; i++)
        {
            location.anchoredPosition = new Vector2(i, 0);
        }
    }


    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;

            StartCoroutine(play());
        }
    }

    private void ReturnBack()
    {
        back.SetActive(false);
        mainMenu.BackToMainMenu();
    }

}
