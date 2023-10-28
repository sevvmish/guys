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
    [SerializeField] private Transform mainPlayer;
    [SerializeField] private CameraControl cameraControl;

    public Joystick GetJoystick() => joystick;
    public Camera GetCamera() => _camera;
    public Transform GetCameraBody() => cameraBody;
    public CameraControl GetCameraControl() => cameraControl;
    public Vector3 BotPoints;

    //GAME START
    public float GameSecondsPlayed { get; private set; }
    public bool IsGameStarted { get; private set; }

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

        cameraControl.SetData(mainPlayer, cameraBody);
        Globals.IsMobile = true;//GP_Device.IsMobile();
        IsGameStarted = true;
    }

    private void Update()
    {
        if (IsGameStarted)
        {
            GameSecondsPlayed += Time.deltaTime;
        }
    }

}
