using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using YG;
using DG.Tweening;

[DefaultExecutionOrder(-2)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Controls")]
    [SerializeField] private Joystick joystick;

    [Header("Others")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform cameraBody;    
    [SerializeField] private CameraControl cameraControl;
    [SerializeField] private Transform playersLocation;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Transform vfx;
    [SerializeField] private UIManager mainUI;
    [SerializeField] private OptionsMenu options;
    [SerializeField] private PhysicMaterial sliderMaterial;
    public PlayerControl MainPlayerControl { get; private set; }

    public Joystick GetJoystick() => joystick;
    public Camera GetCamera() => _camera;
    public Transform GetCameraBody() => cameraBody;
    public CameraControl GetCameraControl() => cameraControl;
    public Transform GetPlayersLocation() => playersLocation;
    public Transform GetMainPlayerTransform() => mainPlayer;
    public Transform GetVFX() => vfx;
    public UIManager GetUI() => mainUI;
    public LevelManager GetLevelManager() => levelManager;
    public PhysicMaterial GetSlidingPhysicsMaterial() => sliderMaterial;
    public GameTypes GameType { get; private set; }
    public int PlayersAmount { get; private set; }
    public bool IsMainPlayerWin { get; private set; }
    public bool IsPause { get; private set; }


    //GAME START
    public float GameSecondsPlayed { get; private set; }
    public float GameSecondsLeft { get; private set; }
    public bool IsGameStarted { get; private set; }

    private Transform mainPlayer;
    
    private List<PlayerControl> bots = new List<PlayerControl>();
    private List<PlayerControl> finishPlaces = new List<PlayerControl>();

    private float cameraShakeCooldown;
    private float _forTimer;
    private bool isTimerActive;

    //TODEL
    [SerializeField] private TextMeshProUGUI testText;
    public TextMeshProUGUI GetTestText() => testText;


    // Start is called before the first frame update
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

        if (Globals.MainPlayerData != null) YandexGame.StickyAdActivity(!Globals.MainPlayerData.AdvOff);

        Globals.IsMobile = Globals.IsMobileChecker();

        
        if (Globals.IsMobile)
        {
            QualitySettings.antiAliasing = 2;

            if (Globals.IsLowFPS)
            {
                QualitySettings.shadows = ShadowQuality.Disable;
            }
            else
            {
                QualitySettings.shadows = ShadowQuality.HardOnly;
                QualitySettings.shadowResolution = ShadowResolution.Medium;
            }
        }
        else
        {
            QualitySettings.antiAliasing = 4;
            QualitySettings.shadows = ShadowQuality.All;
            QualitySettings.shadowResolution = ShadowResolution.Medium;
        }
        



        //TODEL
        //Globals.MainPlayerData = new PlayerData();
        //Globals.MainPlayerData.Zoom = 0;
        //Globals.IsInitiated = true;
        //Globals.IsMobile = false;
        //Globals.IsSoundOn = true;
        //Globals.IsMusicOn = true;
        //Globals.Language = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();
        if (Globals.MainPlayerData == null)
        {
            Globals.MainPlayerData = new PlayerData();
            Globals.MainPlayerData.Zoom = 0;
            Globals.IsInitiated = true;
            Globals.IsMobile = false;
            Globals.IsSoundOn = true;
            Globals.IsMusicOn = true;
            Globals.Language = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();
        }


        //additional systems
        GameType = LevelManager.GetLevelData(levelManager.GetCurrentLevelType()).GameType;

        if (GameType == GameTypes.Dont_fall)
        {
            Globals.IsBotAntiStuckON = false;
            print("antistuck OFF");
        }

        

        if (GameType == GameTypes.Dont_fall || GameType == GameTypes.Challenge)
        {
            switch(levelManager.GetCurrentLevelType())
            {
                case LevelTypes.level3:
                    GameSecondsLeft = 45;
                    break;

                case LevelTypes.level6:
                    GameSecondsLeft = 30;
                    break;

                case LevelTypes.level8:
                    GameSecondsLeft = 60;
                    break;

                case LevelTypes.level11:
                    GameSecondsLeft = 150;
                    break;

                case LevelTypes.level12:

                    if (Globals.IsMobile)
                    {
                        GameSecondsLeft = 180;
                    }
                    else
                    {
                        GameSecondsLeft = 120;
                    }                                        
                    break;

                case LevelTypes.level14:
                    GameSecondsLeft = 90;
                    break;

                default:
                    GameSecondsLeft = 30;
                    break;
            }

            isTimerActive = true;
            
            mainUI.ShowTimerData(GameSecondsLeft);
        }

        mainPlayer = AddPlayer(true, Vector3.zero, Vector3.zero, (Skins)Globals.MainPlayerData.CS).transform;
        cameraControl.SetData(mainPlayer, cameraBody, _camera.transform);
        mainPlayer.GetComponent<PlayerControl>().SetPlayerToMain();
        mainPlayer.gameObject.name = "Main Player";

        int playerAmount = 0;
        Globals.LastPlayedLevel = levelManager.GetCurrentLevelType();


        if (levelManager.GetCurrentLevelType() != LevelTypes.tutorial)
        {
            //Analytics
            string dataForA = "lvl" + (int)levelManager.GetCurrentLevelType() + "s";
            YandexMetrica.Send(dataForA);

            if (Globals.IsMobile)
            {
                if (!Globals.IsFirstGameStarted)
                {
                    Globals.IsFirstGameStarted = true;
                    playerAmount = 5;
                }
                else
                {
                    if (Globals.IsLowFPS)
                    {
                        playerAmount = 5;
                    }
                    else
                    {
                        if (Globals.MainPlayerData.FPS <= 1)
                        {
                            playerAmount = 15;
                        }
                        else if (Globals.MainPlayerData.FPS < 40)
                        {
                            playerAmount = 5;
                        }
                        else if (Globals.MainPlayerData.FPS < 56)
                        {
                            playerAmount = 7;
                        }
                        else
                        {
                            playerAmount = 15;

                            if (levelManager.GetCurrentLevelType() == LevelTypes.level1 && Globals.MainPlayerData.WR.Length == 0)
                            {
                                playerAmount = 7;
                            }
                            else
                            {
                                playerAmount = 11;
                            }
                        }
                    }
                }
                                

                if (levelManager.GetCurrentLevelType() == LevelTypes.level10)
                {
                    if (Globals.IsLowFPS)
                    {
                        playerAmount = 5;
                    }
                    else
                    {
                        playerAmount = 11;
                    }                    
                }
            }
            else
            {
                playerAmount = 15;

                if (levelManager.GetCurrentLevelType() == LevelTypes.level1)
                {
                    playerAmount = 11;
                }
            }

            if (levelManager.GetCurrentLevelType() == LevelTypes.level3)
            {                
                playerAmount = 11;
            }

            if (levelManager.GetCurrentLevelType() == LevelTypes.level8 || levelManager.GetCurrentLevelType() == LevelTypes.level14)
            {
                if (Globals.IsLowFPS)
                {
                    playerAmount = 9;
                }
                else
                {
                    playerAmount = 15;
                }
                
            }

            PlayersAmount = playerAmount + 1;
            

            if (GameType == GameTypes.Challenge)
            {
                mainPlayer.position = levelManager.GetStartPoint.position;
            }
            else
            {
                ArrangePlayers(playerAmount);                
            }                        
        }
        else
        {
            //Analytics
            string dataForA = "tuts";
            YandexMetrica.Send(dataForA);
        }

        if (levelManager == null)
        {
            StartTheGame();
            AddPlayer(false, Vector3.zero, Vector3.zero, Skins.civilian_male_1);
        }

    }

    public void ArrangePlayers(int botsAmount)
    {
        List<GameObject> players = new List<GameObject>();
        players.Add(mainPlayer.gameObject);

        GameObject g = default;
        List<int> botsSkins = new List<int>();

        for (int i = 0; i < botsAmount; i++)
        {
            int sex = UnityEngine.Random.Range(0, 2);
            Skins skins = Skins.civilian_male_1;

            for (int j = 0; j < 5; j++)
            {
                switch (sex)
                {
                    case 0:
                        skins = (Skins)UnityEngine.Random.Range(Globals.MaleSkins.x, Globals.MaleSkins.y + 1);
                        break;

                    case 1:
                        skins = (Skins)UnityEngine.Random.Range(Globals.FemaleSkins.x, Globals.FemaleSkins.y + 1);
                        break;
                }

                if (!botsSkins.Contains((int)skins)) break;
            }

            botsSkins.Add((int)skins);


            g = AddPlayer(false, new Vector3(0, 0, 0), Vector3.zero, skins);
            g.transform.localPosition = Vector3.zero;
            players.Add(g);
        }


        Vector3 startPoint = Vector3.zero;

        if (levelManager != null)
        {
            startPoint = levelManager.GetStartPoint.position;
        }

            
        float delta = 2.4f;
        float deltaPlus = 0f;

        if (players.Count <= 8)
        {
            int amount = players.Count;
            float addX = 1.2f;
            for (int i = 0; i < amount; i++)
            {
                float addZ = 0;

                if (GameType == GameTypes.Dont_fall)
                {
                    deltaPlus = 1.6f;
                    if (i < 4)
                    {
                        addZ = 2f;
                    }
                    else
                    {
                        addZ = -2f;
                    }
                    

                    if (i == 0 || i == 4)
                    {
                        addX = 0;
                    }
                    else
                    {
                        addX++;
                    }

                    int index = UnityEngine.Random.Range(0, players.Count);
                    players[index].transform.position = new Vector3(startPoint.x - 6 + addX * (delta + deltaPlus), startPoint.y, startPoint.z + addZ);
                    players.Remove(players[index]);
                }
                else
                {
                    delta = 1.8f;
                    int index = UnityEngine.Random.Range(0, players.Count);
                    players[index].transform.position = new Vector3(startPoint.x - players.Count + i * (delta + deltaPlus), startPoint.y, startPoint.z);
                    players.Remove(players[index]);
                }

                
            }
        }
        else if (players.Count <= 12)
        {
            float addZ = 0;
            float addX = 1.2f;
            int amount = players.Count;
            for (int i = 0; i < amount; i++)
            {
                if (GameType == GameTypes.Dont_fall)
                {
                    deltaPlus = 1.6f;

                    if (i < 4)
                    {
                        addZ = 4f;
                    }
                    else if (i < 8)
                    {
                        addZ = 0;
                    }
                    else
                    {
                        addZ = -4f;
                    }

                    if (i == 0 || i == 4 || i == 8)
                    {
                        addX = 0;
                    }
                    else
                    {
                        addX++;
                    }

                    int index = UnityEngine.Random.Range(0, players.Count);
                    players[index].transform.position = new Vector3(startPoint.x - 6f + addX * (delta + deltaPlus), startPoint.y, startPoint.z + addZ);
                    players.Remove(players[index]);
                }
                else
                {
                    if (i < 6)
                    {
                        addZ = 1.3f;
                    }
                    else
                    {
                        addZ = -1.3f;
                    }

                    if (i == 6)
                    {
                        addX -= 16f;
                    }

                    int index = UnityEngine.Random.Range(0, players.Count);
                    players[index].transform.position = new Vector3(startPoint.x - 6f + i * (delta + deltaPlus) + addX, startPoint.y, startPoint.z + addZ);
                    players.Remove(players[index]);
                }


            }
        }
        else if(players.Count <= 16)
        {
            float addZ = 0;
            float addX = 1.2f;
            int amount = players.Count;
            for (int i = 0; i < amount; i++)
            {
                if (GameType == GameTypes.Dont_fall)
                {
                    deltaPlus = 1.6f;

                    if (i < 6)
                    {
                        addZ = 4f;
                    }
                    else if (i < 12)
                    {
                        addZ = 0;
                    }
                    else
                    {
                        addZ = -4f;
                    }

                    if (i==0 || i == 6 || i == 12)
                    {
                        addX = 0;
                    }
                    else
                    {
                        addX++;
                    }

                    int index = UnityEngine.Random.Range(0, players.Count);
                    players[index].transform.position = new Vector3(startPoint.x - 10f + addX * (delta + deltaPlus), startPoint.y, startPoint.z + addZ);
                    players.Remove(players[index]);
                }
                else
                {
                    if (i < 8)
                    {
                        addZ = 1.3f;
                    }
                    else
                    {
                        addZ = -1.3f;
                    }

                    if (i == 8)
                    {
                        addX -= 18f;
                    }

                    int index = UnityEngine.Random.Range(0, players.Count);
                    players[index].transform.position = new Vector3(startPoint.x - 10f + i * (delta + deltaPlus) + addX, startPoint.y, startPoint.z + addZ);
                    players.Remove(players[index]);
                }

                
            }
        }
    }

    public void StartTheGame()
    {
        if (IsGameStarted) return;

        mainUI.StartTheGame();
        options.TurnAllOn();
        IsGameStarted = true;

        if (isTimerActive)
        {
            mainUI.ShowTimerData(GameSecondsLeft);
        }
    }

    public void EndTheGame(bool isWin)
    {        
        
        mainUI.EndGame(isWin);
        options.TurnAllOff();
        IsGameStarted = false;
    }

    public void SetPause(bool isPause)
    {
        /*
        if (isPause && IsGameStarted && !IsPause)
        {
            IsPause = true;
            Time.timeScale = 0;
        }
        else if (!isPause && IsPause)
        {
            IsPause = false;
            IsGameStarted = true;
            Time.timeScale = 1;
        }*/
    }

    public void ShakeScreen(float _time, float strength, int vibra)
    {
        if (cameraShakeCooldown > 0) return;

        _time = _time < 0.1f ? 0.1f : _time;
        strength = strength < 1f ? 1f : strength;
        vibra = vibra < 10 ? 10 : vibra;


        _camera.transform.DOShakePosition(_time, strength, vibra);
        cameraShakeCooldown = _time + 0.1f;
    }
  

    private void Update()
    {
        if (cameraShakeCooldown > 0) cameraShakeCooldown -= Time.deltaTime;
        
        if (IsGameStarted)
        {
            GameSecondsPlayed += Time.deltaTime;
            

            if (isTimerActive)
            {
                if (GameSecondsLeft <= 0 && !MainPlayerControl.IsDead)
                {
                    AddPlayerFinished(MainPlayerControl);
                    return;
                }

                GameSecondsLeft -= Time.deltaTime;

                if (_forTimer > 1f)
                {
                    _forTimer = 0;
                    mainUI.ShowTimerData(GameSecondsLeft);
                }
                else
                {
                    _forTimer += Time.deltaTime;
                }
            }

            
        }
        
               

        /*
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddPlayerFinished(MainPlayerControl);
        }*/
        
    }

    public void AssessReward(out int xp, out int gold)
    {
        xp = 0;
        gold = 0;

        LevelData ld = LevelManager.GetLevelData(levelManager.GetCurrentLevelType());

        switch(ld.GameType)
        {
            case GameTypes.Tutorial:
                xp = 70;
                gold = 20;
                break;

            case GameTypes.Finish_line:
                int place = GetFinishPlace(MainPlayerControl);

                if (IsMainPlayerWin)
                {
                    if (place == 1)
                    {
                        xp = 70;
                        gold = 30;
                    }
                    else if (place <= 3)
                    {
                        xp = 45;
                        gold = 20;
                    }
                    else
                    {
                        xp = 30;
                        gold = 10;
                    }
                }
                else
                {
                    xp = 20;
                    gold = 5;
                }
                

                break;

            case GameTypes.Dont_fall:
                if (IsMainPlayerWin)
                {
                    xp = 60;
                    gold = 20;
                }
                else
                {
                    xp = 30;
                    gold = 10;
                }
                break;

            case GameTypes.Challenge:
                if (IsMainPlayerWin)
                {
                    xp = 70;
                    gold = 30;
                }
                else
                {
                    xp = 30;
                    gold = 10;
                }
                break;
        }
    }

    public int GetFinishPlace(PlayerControl player)
    {
        if (finishPlaces.Contains(player))
        {
            int result = finishPlaces.IndexOf(player) + 1;
            return result;
        }

        return 0;
    }

    public void AddPlayerFinished(PlayerControl player)
    {
        LevelData data = LevelManager.GetLevelData(levelManager.GetCurrentLevelType());

        switch(data.GameType)
        {
            case GameTypes.Finish_line:
                if (!finishPlaces.Contains(player))
                {
                    finishPlaces.Add(player);
                }

                if (player == MainPlayerControl)
                {
                    int place = GetFinishPlace(player);
                    if (place == PlayersAmount)
                    {
                        IsMainPlayerWin = false;
                    }
                    else
                    {
                        IsMainPlayerWin = true;
                    }

                    EndTheGame(IsMainPlayerWin);
                }
                else
                {
                    int place = GetFinishPlace(player);
                    if (place == (PlayersAmount-1))
                    {
                        EndTheGame(false);
                    }
                }
                break;

            case GameTypes.Tutorial:
                if (!finishPlaces.Contains(player))
                {
                    finishPlaces.Add(player);
                }

                if (player == MainPlayerControl)
                {
                    EndTheGame(true);
                }
                break;

            case GameTypes.Dont_fall:
                if (player.IsItMainPlayer)
                {
                    if (player.IsDead && GameSecondsLeft > 0)
                    {
                        IsMainPlayerWin = false;
                    }
                    else if (!player.IsDead && GameSecondsLeft <= 0)
                    {
                        IsMainPlayerWin = true;
                    }
                }

                EndTheGame(IsMainPlayerWin);

                break;

            case GameTypes.Challenge:
                if (GameSecondsLeft <= 0)
                {
                    IsMainPlayerWin = false;
                }
                else
                {
                    IsMainPlayerWin = true;
                }

                EndTheGame(IsMainPlayerWin);

                break;
        }

              
    }

    public GameObject AddPlayer(bool isMain, Vector3 pos, Vector3 rot, Skins skinType)
    {
        //main template
        GameObject g = SkinControl.GetSkinGameobject(Skins.main_player_template);
        g.transform.parent = playersLocation;
        g.transform.position = pos;
        g.transform.eulerAngles = rot;
        

        //vfx
        GameObject vfx = Instantiate(Resources.Load<GameObject>("player vfx"), g.transform);
        vfx.transform.localPosition = Vector3.zero;
        vfx.transform.localEulerAngles = Vector3.zero;
        g.GetComponent<PlayerControl>().SetEffectControl(vfx.GetComponent<EffectsControl>());

        //player
        GameObject skin = SkinControl.GetSkinGameobject(skinType);
        skin.transform.parent = g.transform;
        skin.transform.localPosition = Vector3.zero;
        skin.transform.localEulerAngles = Vector3.zero;
        SkinControl skinControl = skin.GetComponent<SkinControl>();
        g.GetComponent<PlayerControl>().SetSkinData(skinControl.ragdollColliders, skinControl._animator, skinType);
        
        if (levelManager.GetCurrentLevelType() == LevelTypes.level5)
        {
            g.GetComponent<PlayerControl>().SetSlide(true);
        }
        

        if (isMain)
        {
            g.AddComponent<InputControl>();
            g.AddComponent<AudioListener>();
            MainPlayerControl = g.GetComponent<PlayerControl>();
        }
        else
        {
            g.AddComponent<BotAI>();
            bots.Add(g.GetComponent<PlayerControl>());
        }

        g.SetActive(true);
        

        return g;
    }

    
}

public interface Explosives
{    
    void SetTTL(float seconds);
}

