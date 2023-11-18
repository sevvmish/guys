using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSaver : MonoBehaviour
{
    public static ScreenSaver Instance { get; private set; }

    [SerializeField] private RectTransform[] type1;
    [SerializeField] private RectTransform[] type2;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        type1[0].gameObject.SetActive(true);
        type1[0].anchoredPosition = new Vector3(0, 0, 0);

        type1[1].gameObject.SetActive(true);
        type1[1].anchoredPosition = new Vector3 (0, 440, 0);

        type1[2].gameObject.SetActive(true);
        type1[2].anchoredPosition = new Vector3(0, -440, 0);

        type2[0].gameObject.SetActive(true);
        type2[0].anchoredPosition = new Vector3(0, 220, 0);

        type2[1].gameObject.SetActive(true);
        type2[1].anchoredPosition = new Vector3(0, -220, 0);
    }

    public void HideScreen()
    {
        type1[0].gameObject.SetActive(true);
        type1[0].anchoredPosition = new Vector3(-2200, 0, 0);
        type1[0].DOAnchorPos3D(new Vector3(0, 0, 0), Globals.SCREEN_SAVER_AWAIT).SetEase(Ease.Linear);

        type1[1].gameObject.SetActive(true);
        type1[1].anchoredPosition = new Vector3(-2200, 440, 0);
        type1[1].DOAnchorPos3D(new Vector3(0, 440, 0), Globals.SCREEN_SAVER_AWAIT).SetEase(Ease.Linear);

        type1[2].gameObject.SetActive(true);
        type1[2].anchoredPosition = new Vector3(-2200, -440, 0);
        type1[2].DOAnchorPos3D(new Vector3(0, -440, 0), Globals.SCREEN_SAVER_AWAIT).SetEase(Ease.Linear);

        type2[0].gameObject.SetActive(true);
        type2[0].anchoredPosition = new Vector3(2200, 220, 0);
        type2[0].DOAnchorPos3D(new Vector3(0, 220, 0), Globals.SCREEN_SAVER_AWAIT).SetEase(Ease.Linear);

        type2[1].gameObject.SetActive(true);
        type2[1].anchoredPosition = new Vector3(2200, -220, 0);
        type2[1].DOAnchorPos3D(new Vector3(0, -220, 0), Globals.SCREEN_SAVER_AWAIT).SetEase(Ease.Linear);
    }

    public void ShowScreen()
    {
        if (Globals.IsDevelopmentBuild)
        {
            type1[0].gameObject.SetActive(true);
            type1[0].anchoredPosition = new Vector3(-2200, 0, 0);

            type1[1].gameObject.SetActive(true);
            type1[1].anchoredPosition = new Vector3(-2200, 440, 0);

            type1[2].gameObject.SetActive(true);
            type1[2].anchoredPosition = new Vector3(-2200, -440, 0);

            type2[0].gameObject.SetActive(true);
            type2[0].anchoredPosition = new Vector3(2200, 220, 0);

            type2[1].gameObject.SetActive(true);
            type2[1].anchoredPosition = new Vector3(2200, -220, 0);

            return;
        }

        type1[0].gameObject.SetActive(true);
        type1[0].anchoredPosition = new Vector3(0, 0, 0);
        type1[0].DOAnchorPos3D(new Vector3(-2200, 0, 0), Globals.SCREEN_SAVER_AWAIT).SetEase(Ease.Linear);

        type1[1].gameObject.SetActive(true);
        type1[1].anchoredPosition = new Vector3(0, 440, 0);
        type1[1].DOAnchorPos3D(new Vector3(-2200, 440, 0), Globals.SCREEN_SAVER_AWAIT).SetEase(Ease.Linear);

        type1[2].gameObject.SetActive(true);
        type1[2].anchoredPosition = new Vector3(0, -440, 0);
        type1[2].DOAnchorPos3D(new Vector3(-2200, -440, 0), Globals.SCREEN_SAVER_AWAIT).SetEase(Ease.Linear);

        type2[0].gameObject.SetActive(true);
        type2[0].anchoredPosition = new Vector3(0, 220, 0);
        type2[0].DOAnchorPos3D(new Vector3(2200, 220, 0), Globals.SCREEN_SAVER_AWAIT).SetEase(Ease.Linear);

        type2[1].gameObject.SetActive(true);
        type2[1].anchoredPosition = new Vector3(0, -220, 0);
        type2[1].DOAnchorPos3D(new Vector3(2200, -220, 0), Globals.SCREEN_SAVER_AWAIT).SetEase(Ease.Linear);
    }


}
