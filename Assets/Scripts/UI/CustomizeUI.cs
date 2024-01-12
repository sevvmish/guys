using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeUI : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private Transform location;

    private bool isReady;
    private GameObject[] PlayerSkins;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSkins = new GameObject[50];
        
        for (int i = 2; i <= 4; i++)
        {
            PlayerSkins[i] = SkinControl.GetSkinGameobject((Skins)i);
            PlayerSkins[i].transform.parent = location;
            PlayerSkins[i].transform.position = Globals.UIPlayerPosition + new Vector3(2.9f, 0.1f, 0);
            PlayerSkins[i].transform.eulerAngles = Globals.UIPlayerRotation;
            PlayerSkins[i].transform.localScale = Vector3.one * 0.9f;
            PlayerSkins[i].SetActive(false);
        }
    }

    public void SetOn()
    {
        mainMenu.GetCameraTransform.DOMove(new Vector3(1.3f, 0, -9), 0.3f).SetEase(Ease.Linear);
    }

    private void Update()
    {
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;

        }
    }

}
