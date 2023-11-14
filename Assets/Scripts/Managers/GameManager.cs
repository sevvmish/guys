using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamePush;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Controls")]
    [SerializeField] private Joystick joystick;

    [Header("Others")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform cameraBody;    
    [SerializeField] private CameraControl cameraControl;
    [SerializeField] private AssetManager assetManager;
    [SerializeField] private Transform playersLocation;

    public Joystick GetJoystick() => joystick;
    public Camera GetCamera() => _camera;
    public Transform GetCameraBody() => cameraBody;
    public CameraControl GetCameraControl() => cameraControl;
    public AssetManager GetAssetManager() => assetManager;
    public Transform GetPlayerLocation() => playersLocation;
    public Vector3 BotPoints;

    //GAME START
    public float GameSecondsPlayed { get; private set; }
    public bool IsGameStarted { get; private set; }

    private Transform mainPlayer;

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

        Globals.IsMobile = GP_Device.IsMobile();
        IsGameStarted = true;

        mainPlayer = addPlayer(true, Vector3.zero, Vector3.zero).transform;
        cameraControl.SetData(mainPlayer, cameraBody, _camera.transform);
        mainPlayer.GetComponent<PlayerControl>().SetPlayerToMain();
        mainPlayer.gameObject.name = "Main Player";

        addPlayer(false, new Vector3(0, 0, 1), Vector3.zero);
        addPlayer(false, new Vector3(2.5f, 0, 1), Vector3.zero);
        addPlayer(false, new Vector3(-2.5f, 0, 1), Vector3.zero);
        addPlayer(false, new Vector3(-0.5f, 0, -1), Vector3.zero);
        addPlayer(false, new Vector3(1.5f, 0, 1), Vector3.zero);
    }

  

    private void Update()
    {
        if (IsGameStarted)
        {
            GameSecondsPlayed += Time.deltaTime;
        }
    }

    private GameObject addPlayer(bool isMain, Vector3 pos, Vector3 rot)
    {
        GameObject g = Instantiate(assetManager.GetPlayerSkin(PLayerSkin.test), playersLocation);
        g.transform.position = pos;
        g.transform.eulerAngles = rot;
        g.SetActive(true);

        if (isMain)
        {
            g.AddComponent<InputControl>();
            g.AddComponent<AudioListener>();
        }
        else
        {
            g.AddComponent<BotAI>();
        }

        return g;
    }

}
