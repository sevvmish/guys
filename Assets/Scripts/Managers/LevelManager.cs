using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("game intro")]
    [SerializeField] private Transform[] previewTransform;
    [SerializeField] private float[] previewTime;
    [SerializeField] private GameCountDown countDown;

    [Header("level options")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private LevelTypes levelType;

    private Transform cameraBody;
    private GameManager gm;
    private bool isLevelStarted;

    public Transform GetStartPoint => startPoint;
    public LevelTypes GetCurrentLevelType() { return levelType; }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        cameraBody = GameManager.Instance.GetCameraBody();
        StartCoroutine(playPreview());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            countDown.StartCountDown();
        }

    }

    private IEnumerator playPreview()
    {
        ScreenSaver.Instance.ShowScreen();

        if (Globals.IsDevelopmentBuild)
        {
            gm.StartTheGame();
            countDown.IsCountDownOff = true;
            AmbientMusic.Instance.PlayScenario1();
            yield break;
        }
        
        cameraBody.localPosition = Vector3.zero;
        cameraBody.localRotation = Quaternion.identity;
        cameraBody.position = previewTransform[0].position;
        cameraBody.eulerAngles = previewTransform[0].eulerAngles;
        yield return new WaitForSeconds(Globals.SCREEN_SAVER_AWAIT);
        AmbientMusic.Instance.PlayAmbient(AmbientMelodies.level_intro);
        yield return new WaitForSeconds(previewTime[0]);

        for (int i = 1; i < previewTransform.Length; i++)
        {
            cameraBody.DOMove(previewTransform[i].position, previewTime[i]).SetEase(Ease.Linear);
            cameraBody.DORotate(previewTransform[i].eulerAngles, previewTime[i]).SetEase(Ease.Linear);
            yield return new WaitForSeconds(previewTime[i]);
        }

        cameraBody.DOLocalMove(Globals.BasePosition, 1f).SetEase(Ease.Linear);
        cameraBody.DOLocalRotate(Globals.BaseRotation, 1f).SetEase(Ease.Linear);
        cameraBody.parent.DOMove(gm.GetMainPlayerTransform().position, 1f).SetEase(Ease.Linear);

        if (levelType == LevelTypes.tutorial)
        {
            countDown.IsCountDownOff = true;
        }
        else
        {
            countDown.StartCountDown();
        }

        yield return new WaitForSeconds(1f);

        for (float i = 0; i < 10; i+=0.1f)
        {
            if (!gm.IsGameStarted && countDown.IsCountDownOff)
            {                
                gm.StartTheGame();
                AmbientMusic.Instance.PlayScenario1();
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public static LevelData GetLevelData(LevelTypes level)
    {
        LevelData result = new LevelData();

        switch(level)
        {
            case LevelTypes.tutorial:
                return new LevelData(Globals.Language.Level1Name, "", GameTypes.Tutorial, Globals.Language.Aim_Tutorial);

            case LevelTypes.level1:
                return new LevelData(Globals.Language.Level1Name, "", GameTypes.Finish_line, Globals.Language.Aim_Finish);

            case LevelTypes.level2:
                return new LevelData(Globals.Language.Level2Name, "", GameTypes.Finish_line, Globals.Language.Aim_Finish);

            case LevelTypes.level3:
                return new LevelData(Globals.Language.Level3Name, "", GameTypes.Finish_line, Globals.Language.Aim_Finish);

            case LevelTypes.level4:
                return new LevelData(Globals.Language.Level4Name, "", GameTypes.Finish_line, Globals.Language.Aim_Finish);
        }

        return result;
    }
}

public enum LevelTypes
{
    tutorial,
    level1,
    level2,
    level3,
    level4
}


public struct LevelData
{
    public string LevelName;
    public string LevelDescription;
    public GameTypes LevelType;
    public string LevelAim;

    public LevelData(string name, string descr, GameTypes lvl, string aim)
    {
        LevelName = name;
        LevelDescription = descr;
        LevelType = lvl;
        LevelAim = aim;
    }
}
