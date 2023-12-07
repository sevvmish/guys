using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
        Globals.MainPlayerData.M1 = 18;
        Globals.MainPlayerData.Zoom = 0;


        mainPlayer = addPlayer(true, Vector3.zero, Vector3.zero).transform;
        cameraControl.SetData(mainPlayer, cameraBody, _camera.transform);
        mainPlayer.GetComponent<PlayerControl>().SetPlayerToMain();
        mainPlayer.gameObject.name = "Main Player";

        //ArrangePlayers(15);      

        if (levelManager == null)
        {
            IsGameStarted = true;
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
        IsGameStarted = true;
    }
  

    private void Update()
    {
        if (IsGameStarted)
        {
            GameSecondsPlayed += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Globals.MainPlayerData = new PlayerData();
            SaveLoadManager.Save();

            //Globals.CurrentRespawnPointOnMap = Globals.MainPlayerData.M1;

            SceneManager.LoadScene("circus1");
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
        GameObject skin = Instantiate(SkinControl.GetSkinGameobject(Skins.pomni), g.transform);
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

