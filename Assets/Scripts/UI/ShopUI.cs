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
        //mainMenu.GetCameraTransform.DOMove(new Vector3(-6, 0, -9), 0.5f).SetEase(Ease.Linear);
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

            /*
            if (Globals.IsMobile)
            {
                mainRect.anchoredPosition = new Vector2(0,-70);
            }
            else
            {
                if (!Globals.MainPlayerData.AdvOff)
                {
                    mainRect.anchoredPosition = new Vector2(0, 50);
                }
                else
                {
                    mainRect.anchoredPosition = new Vector2(0, 0);
                }
            }*/
            StartCoroutine(play());
        }
    }

    private void ReturnBack()
    {
        back.SetActive(false);
        mainMenu.BackToMainMenu();
    }

}
