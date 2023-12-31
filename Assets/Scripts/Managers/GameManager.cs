using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YG;
using UnityEngine.SceneManagement;
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

    public Joystick GetJoystick() => joystick;
    public Camera GetCamera() => _camera;
    public Transform GetCameraBody() => cameraBody;
    public CameraControl GetCameraControl() => cameraControl;
    public Transform GetPlayersLocation() => playersLocation;
    public Transform GetMainPlayerTransform() => mainPlayer;
    public Transform GetVFX() => vfx;
    public UIManager GetUI() => mainUI;
    public LevelManager GetLevelManager() => levelManager;
                   

    //GAME START
    public float GameSecondsPlayed { get; private set; }
    public bool IsGameStarted { get; private set; }

    private Transform mainPlayer;
    private PlayerControl mainPlayerControl;
    private List<PlayerControl> bots = new List<PlayerControl>();
    private List<PlayerControl> finishPlaces = new List<PlayerControl>();

    private float cameraShakeCooldown;

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


        //TODEL
        Globals.MainPlayerData = new PlayerData();
        Globals.MainPlayerData.Zoom = 0;
        Globals.IsMobile = true;//false;
        Globals.Language = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();


        mainPlayer = addPlayer(true, Vector3.zero, Vector3.zero).transform;
        cameraControl.SetData(mainPlayer, cameraBody, _camera.transform);
        mainPlayer.GetComponent<PlayerControl>().SetPlayerToMain();
        mainPlayer.gameObject.name = "Main Player";

        ArrangePlayers(7);

        if (levelManager == null)
        {
            StartTheGame();
            addPlayer(false, Vector3.zero, Vector3.zero);
        }

        

    }

    public void ArrangePlayers(int botsAmount)
    {
        List<GameObject> players = new List<GameObject>();
        players.Add(mainPlayer.gameObject);

        GameObject g = default;

        for (int i = 0; i < botsAmount; i++)
        {
            g = addPlayer(false, new Vector3(0, 0, 0), Vector3.zero);
            g.transform.localPosition = Vector3.zero;
            players.Add(g);
        }


        Vector3 startPoint = Vector3.zero;

        if (levelManager != null)
        {
            startPoint = levelManager.GetStartPoint.position;
        }

            
        float delta = 2.4f;

        if (players.Count <= 8)
        {
            int amount = players.Count;
            for (int i = 0; i < amount; i++)
            {
                int index = UnityEngine.Random.Range(0, players.Count);                
                players[index].transform.position = new Vector3(startPoint.x - 8.4f + i * delta, startPoint.y, startPoint.z);                
                players.Remove(players[index]);
            }
        }
        else if(players.Count <= 16)
        {
            float addZ = 0;
            float addX = 1.2f;
            int amount = players.Count;
            for (int i = 0; i < amount; i++)
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
                players[index].transform.position = new Vector3(startPoint.x - 8.4f + i * delta + addX, startPoint.y, startPoint.z + addZ);
                players.Remove(players[index]);
            }
        }
    }

    public void StartTheGame()
    {
        if (IsGameStarted) return;

        mainUI.StartTheGame();
        options.TurnAllOn();
        IsGameStarted = true;
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
        }

        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
            Globals.MainPlayerData = new PlayerData();
            SaveLoadManager.Save();

            SceneManager.LoadScene("circus1");
        }
        */
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
        if (!finishPlaces.Contains(player))
        {
            finishPlaces.Add(player);
        }

        if (player.Equals(mainPlayerControl))
        {

        }
        else
        {

        }
    }

    private GameObject addPlayer(bool isMain, Vector3 pos, Vector3 rot)
    {
        //main template
        GameObject g = Instantiate(SkinControl.GetSkinGameobject(Skins.main_player_template), playersLocation);
        g.transform.position = pos;
        g.transform.eulerAngles = rot;
        

        //vfx
        GameObject vfx = Instantiate(Resources.Load<GameObject>("player vfx"), g.transform);
        vfx.transform.localPosition = Vector3.zero;
        vfx.transform.localEulerAngles = Vector3.zero;
        g.GetComponent<PlayerControl>().SetEffectControl(vfx.GetComponent<EffectsControl>());

        //player
        GameObject skin = Instantiate(SkinControl.GetSkinGameobject(Skins.civilian_male_1), g.transform);
        skin.transform.localPosition = Vector3.zero;
        skin.transform.localEulerAngles = Vector3.zero;
        SkinControl skinControl = skin.GetComponent<SkinControl>();
        g.GetComponent<PlayerControl>().SetSkinData(skinControl.ragdollColliders, skinControl._animator, Skins.pomni);

        if (isMain)
        {
            g.AddComponent<InputControl>();
            g.AddComponent<AudioListener>();
            mainPlayerControl = g.GetComponent<PlayerControl>();
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

