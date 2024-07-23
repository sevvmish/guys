using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSaver : MonoBehaviour
{
    public static ScreenSaver Instance { get; private set; }

    [SerializeField] private RectTransform[] type1;
    [SerializeField] private RectTransform[] type2;

    private readonly float additionalWait = 0.3f;

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
        type1[1].anchoredPosition = new Vector3 (0, 800, 0);

        type1[2].gameObject.SetActive(true);
        type1[2].anchoredPosition = new Vector3(0, -800, 0);

        type2[0].gameObject.SetActive(true);
        type2[0].anchoredPosition = new Vector3(0, 400, 0);

        type2[1].gameObject.SetActive(true);
        type2[1].anchoredPosition = new Vector3(0, -400, 0);
    }

    public void HideScreen()
    {
        type1[0].gameObject.SetActive(true);
        type1[0].anchoredPosition = new Vector3(-3000, 0, 0);
        type1[0].DOAnchorPos3D(new Vector3(0, 0, 0), Globals.SCREEN_SAVER_AWAIT + additionalWait).SetEase(Ease.Linear);

        type1[1].gameObject.SetActive(true);
        type1[1].anchoredPosition = new Vector3(-3000, 800, 0);
        type1[1].DOAnchorPos3D(new Vector3(0, 800, 0), Globals.SCREEN_SAVER_AWAIT + additionalWait).SetEase(Ease.Linear);

        type1[2].gameObject.SetActive(true);
        type1[2].anchoredPosition = new Vector3(-3000, -800, 0);
        type1[2].DOAnchorPos3D(new Vector3(0, -800, 0), Globals.SCREEN_SAVER_AWAIT + additionalWait).SetEase(Ease.Linear);

        type2[0].gameObject.SetActive(true);
        type2[0].anchoredPosition = new Vector3(3000, 400, 0);
        type2[0].DOAnchorPos3D(new Vector3(0, 400, 0), Globals.SCREEN_SAVER_AWAIT + additionalWait).SetEase(Ease.Linear);

        type2[1].gameObject.SetActive(true);
        type2[1].anchoredPosition = new Vector3(3000, -400, 0);
        type2[1].DOAnchorPos3D(new Vector3(0, -400, 0), Globals.SCREEN_SAVER_AWAIT + additionalWait).SetEase(Ease.Linear);
    }

    public void HideScreenFast()
    {
        type1[0].gameObject.SetActive(true);        
        type1[0].DOAnchorPos3D(new Vector3(0, 0, 0), 0).SetEase(Ease.Linear);

        type1[1].gameObject.SetActive(true);        
        type1[1].DOAnchorPos3D(new Vector3(0, 800, 0), 0).SetEase(Ease.Linear);

        type1[2].gameObject.SetActive(true);
        type1[2].DOAnchorPos3D(new Vector3(0, -800, 0), 0).SetEase(Ease.Linear);

        type2[0].gameObject.SetActive(true);
        type2[0].DOAnchorPos3D(new Vector3(0, 400, 0), 0).SetEase(Ease.Linear);

        type2[1].gameObject.SetActive(true);
        type2[1].DOAnchorPos3D(new Vector3(0, -400, 0), 0).SetEase(Ease.Linear);
    }

    public void FastShowScreen()
    {        
        StartCoroutine(deactivateAfter(0));
    }

    public void ShowScreen()
    {
        if (Globals.IsDevelopmentBuild || Globals.IsDontShowIntro)
        {            
            type1[0].gameObject.SetActive(true);
            type1[0].anchoredPosition = new Vector3(-3000, 0, 0);

            type1[1].gameObject.SetActive(true);
            type1[1].anchoredPosition = new Vector3(-3000, 800, 0);

            type1[2].gameObject.SetActive(true);
            type1[2].anchoredPosition = new Vector3(-3000, -800, 0);

            type2[0].gameObject.SetActive(true);
            type2[0].anchoredPosition = new Vector3(3000, 400, 0);

            type2[1].gameObject.SetActive(true);
            type2[1].anchoredPosition = new Vector3(3000, -400, 0);
            StartCoroutine(deactivateAfter(0));
            return;
        }

        type1[0].gameObject.SetActive(true);
        type1[0].anchoredPosition = new Vector3(0, 0, 0);
        type1[0].DOAnchorPos3D(new Vector3(-3000, 0, 0), Globals.SCREEN_SAVER_AWAIT + additionalWait).SetEase(Ease.Linear);

        type1[1].gameObject.SetActive(true);
        type1[1].anchoredPosition = new Vector3(0, 800, 0);
        type1[1].DOAnchorPos3D(new Vector3(-3000, 800, 0), Globals.SCREEN_SAVER_AWAIT + additionalWait).SetEase(Ease.Linear);

        type1[2].gameObject.SetActive(true);
        type1[2].anchoredPosition = new Vector3(0, -800, 0);
        type1[2].DOAnchorPos3D(new Vector3(-3000, -800, 0), Globals.SCREEN_SAVER_AWAIT + additionalWait).SetEase(Ease.Linear);

        type2[0].gameObject.SetActive(true);
        type2[0].anchoredPosition = new Vector3(0, 400, 0);
        type2[0].DOAnchorPos3D(new Vector3(3000, 400, 0), Globals.SCREEN_SAVER_AWAIT + additionalWait).SetEase(Ease.Linear);

        type2[1].gameObject.SetActive(true);
        type2[1].anchoredPosition = new Vector3(0, -400, 0);
        type2[1].DOAnchorPos3D(new Vector3(3000, -400, 0), Globals.SCREEN_SAVER_AWAIT + additionalWait).SetEase(Ease.Linear);

        StartCoroutine(deactivateAfter(Globals.SCREEN_SAVER_AWAIT));
    }

    private IEnumerator deactivateAfter(float secs)
    {
        yield return new WaitForSeconds(secs);
        type1[0].gameObject.SetActive(false);
        type1[1].gameObject.SetActive(false);
        type1[2].gameObject.SetActive(false);
        type2[0].gameObject.SetActive(false);
        type2[1].gameObject.SetActive(false);

        Globals.IsDontShowIntro = false;
    }


}
