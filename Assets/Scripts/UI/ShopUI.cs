using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private GameObject back;
    [SerializeField] private RectTransform mainRect;

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
        mainMenu.GetCameraTransform.DOMove(new Vector3(-6, 0, -9), 0.5f).SetEase(Ease.Linear);
        mainMenu.MainPlayerSkin.SetActive(false);        
    }

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;

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
            }
        }
    }

    private void ReturnBack()
    {
        back.SetActive(false);
        mainMenu.BackToMainMenu();
    }

}
