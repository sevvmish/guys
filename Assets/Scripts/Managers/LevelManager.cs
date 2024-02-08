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
                return new LevelData(Globals.Language.Level1Name, "", GameTypes.Tutorial, LevelTypes.tutorial, Globals.Language.Aim_Tutorial, "tutorial", 1, null, 0,0);

            case LevelTypes.level1:
                return new LevelData(Globals.Language.Level1Name, "", GameTypes.Finish_line, LevelTypes.level1, Globals.Language.Aim_Finish, "level1", 1, Resources.Load<Sprite>("Sprites/map1"), 0, 0);

            case LevelTypes.level2:
                return new LevelData(Globals.Language.Level2Name, "", GameTypes.Finish_line, LevelTypes.level2, Globals.Language.Aim_Finish, "level2", 2, Resources.Load<Sprite>("Sprites/map2"), 2, 5);

            case LevelTypes.level3:
                return new LevelData(Globals.Language.Level3NewLVLName, "", GameTypes.Dont_fall, LevelTypes.level3, Globals.Language.Aim_DontFall, "level3", 3, Resources.Load<Sprite>("Sprites/map3"), 2, 5);

            case LevelTypes.level4:
                return new LevelData(Globals.Language.Level9Name, "", GameTypes.Finish_line, LevelTypes.level4, Globals.Language.Aim_Finish, "level4", 1, Resources.Load<Sprite>("Sprites/map4"), 3, 5);

            case LevelTypes.level5:
                return new LevelData(Globals.Language.Level5Name, "", GameTypes.Finish_line, LevelTypes.level5, Globals.Language.Aim_Finish, "level5", 2, Resources.Load<Sprite>("Sprites/map5"), 4, 5);

            case LevelTypes.level6:
                return new LevelData(Globals.Language.Level6Name, "", GameTypes.Dont_fall, LevelTypes.level6, Globals.Language.Aim_DontFall, "level6", 2, Resources.Load<Sprite>("Sprites/map6"), 5, 5);

            case LevelTypes.level7:
                return new LevelData(Globals.Language.Level7Name, "", GameTypes.Finish_line, LevelTypes.level7, Globals.Language.Aim_Finish, "level7", 4, Resources.Load<Sprite>("Sprites/map7"), 6, 5);

            case LevelTypes.level8:
                return new LevelData(Globals.Language.Level8Name, "", GameTypes.Dont_fall, LevelTypes.level8, Globals.Language.Aim_Laser, "level8", 3, Resources.Load<Sprite>("Sprites/map8"), 6, 5);

            case LevelTypes.level9:
                return new LevelData(Globals.Language.Level3Name, "", GameTypes.Finish_line, LevelTypes.level9, Globals.Language.Aim_Finish, "level9", 3, Resources.Load<Sprite>("Sprites/map9"), 7, 5);

            case LevelTypes.level10:
                return new LevelData(Globals.Language.Level10Name, "", GameTypes.Finish_line, LevelTypes.level10, Globals.Language.Aim_Finish, "level10", 2, Resources.Load<Sprite>("Sprites/map10"), 8, 5);
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
    level4,
    level5,
    level6,
    level7,
    level8,
    level9,
    level10
}


public struct LevelData
{
    public string LevelName;
    public string LevelDescription;
    public GameTypes GameType;
    public LevelTypes LevelType;
    public string LevelAim;
    public string LevelInInspector;
    public int Difficulty;
    public Sprite ImageSprite;
    public int LevelRestriction;
    public int UblockGemPrice;

    public LevelData(string name, string descr, GameTypes gameType, LevelTypes levelType, string aim, string levelInInspector, int difficulty, Sprite sprite, int levelRestr, int price)
    {
        LevelName = name;
        LevelDescription = descr;
        GameType = gameType;
        LevelType = levelType;
        LevelAim = aim;
        LevelInInspector = levelInInspector;
        Difficulty = difficulty;
        ImageSprite = sprite;
        LevelRestriction = levelRestr;
        UblockGemPrice = price;
    }
}
