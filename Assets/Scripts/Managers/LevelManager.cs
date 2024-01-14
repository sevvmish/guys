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

        if (!gm.IsGameStarted && countDown.IsCountDownOff)
        {
            gm.StartTheGame();
            AmbientMusic.Instance.PlayScenario1();
            /*
            int rnd = UnityEngine.Random.Range(0, 2);

            switch(rnd)
            {
                case 0:
                    AmbientMusic.Instance.PlayAmbient(AmbientMelodies.loop_melody1);
                    break;

                case 1:
                    AmbientMusic.Instance.PlayAmbient(AmbientMelodies.loop_melody2);
                    break;
            }*/
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

        countDown.StartCountDown();

        cameraBody.DOLocalMove(Globals.BasePosition, 1f).SetEase(Ease.Linear);
        cameraBody.DOLocalRotate(Globals.BaseRotation, 1f).SetEase(Ease.Linear);
        cameraBody.parent.DOMove(gm.GetMainPlayerTransform().position, 1f).SetEase(Ease.Linear);
    }

    public static LevelData GetLevelData(LevelTypes level)
    {
        LevelData result = new LevelData();

        switch(level)
        {
            case LevelTypes.level1:
                return new LevelData(Globals.Language.Level1Name, "", LevelTypes.level1, Globals.Language.Aim_Finish);

            case LevelTypes.level2:
                return new LevelData(Globals.Language.Level2Name, "", LevelTypes.level2, Globals.Language.Aim_Finish);

            case LevelTypes.level3:
                return new LevelData(Globals.Language.Level3Name, "", LevelTypes.level3, Globals.Language.Aim_Finish);
        }

        return result;
    }
}

public enum LevelTypes
{
    none,
    level1,
    level2,
    level3    
}

public struct LevelData
{
    public string LevelName;
    public string LevelDescription;
    public LevelTypes LevelType;
    public string LevelAim;

    public LevelData(string name, string descr, LevelTypes lvl, string aim)
    {
        LevelName = name;
        LevelDescription = descr;
        LevelType = lvl;
        LevelAim = aim;
    }
}
