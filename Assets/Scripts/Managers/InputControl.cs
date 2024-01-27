using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerControl))]
public class InputControl : MonoBehaviour
{
    private Joystick joystick;
    private PlayerControl playerControl;
    private CameraControl cameraControl;
    private Transform playerTransform;
    private PointerBase jump;
    private PointerBase mover;
    private PointerBase ability;
    private Vector3 mousePosition;
    private float koeff;
    private readonly float XLimit = 10;
    private GameManager gm;
    private AbilityManager abilityManager;
    private bool isLockedUsed;

    private bool isTouchZoom;
    private Vector3 zoom1Finger;
    private Vector3 zoom2Finger;
    private float zoomDistance;

    private float level4Koeff = 1;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        
        joystick = GameManager.Instance.GetJoystick();
        cameraControl = GameManager.Instance.GetCameraControl();
        playerControl = gameObject.GetComponent<PlayerControl>();
        playerTransform = playerControl.transform;
        abilityManager = playerControl.GetComponent<AbilityManager>();
        ability = GameObject.Find("AbilityButton").GetComponent<PointerBase>();


        if (!Globals.IsMobile)
        {
            Cursor.lockState = CursorLockMode.Locked;       
            Cursor.visible = false;
        }
        else
        {
            jump = GameObject.Find("JumpButton").GetComponent<PointerBase>();
            mover = GameObject.Find("Screen mover").GetComponent<PointerBase>();            
        }

        
        if (gm.GetLevelManager().GetCurrentLevelType() == LevelTypes.level4)
        {
            level4Koeff = 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.IsGameStarted || Globals.IsOptions) return;

        if (Globals.IsMobile)
        {
            forMobile();

            if (ability.IsPressed)
            {
                abilityManager.ActivateAbility();
            }
        }
        else
        {
            forPC();

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
            {
                abilityManager.ActivateAbility();
            }
        }

        
    }



    private void forMobile()
    {        

        playerControl.SetHorizontal(joystick.Horizontal);
        playerControl.SetVertical(joystick.Vertical);

        //TODEL        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            playerControl.SetHorizontal(horizontal);
            playerControl.SetVertical(vertical);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerControl.SetJump();
        }
        //==========================

        if (jump.IsPressed)
        {
            playerControl.SetJump();
        }

        Vector2 delta2 = mover.DeltaPosition;
        Vector2 delta = delta2.normalized;


        if (delta2.x > 0 || delta2.x < 0)
        {
            
            int sign = delta2.x > 0 ? 1 : -1;
            playerControl.SetRotationAngle(200 * sign * Time.deltaTime * level4Koeff);

        }
        else if (delta2.x == 0)
        {
            playerControl.SetRotationAngle(0);
        }

        if (Mathf.Abs(delta.y) > 0)
        {
            cameraControl.ChangeCameraAngleX(delta.y * -70 * Time.deltaTime);
        }

    }

    private void forPC()
    {        
        if (Input.mouseScrollDelta.magnitude > 0)
        {            
            cameraControl.ChangeZoom(Input.mouseScrollDelta.y);
        }

        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            playerControl.SetHorizontal(horizontal);
            playerControl.SetVertical(vertical);
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
        {
            playerControl.SetJump();
        }

        //Vector3 mouseDelta = Input.mousePosition - mousePosition;
        Vector3 mouseDelta = new Vector3(
            Input.GetAxis("Mouse X") * Globals.MOUSE_X_SENS, 
            Input.GetAxis("Mouse Y") * Globals.MOUSE_Y_SENS, 0);

        //gm.GetTestText().text = Input.mousePosition + " = " + mouseDelta;

                
        if ((mouseDelta.x > 0) || (mouseDelta.x < 0))
        {
            float koeff = mouseDelta.x * 20 * Time.deltaTime;

            if (koeff > XLimit)
            {
                koeff = XLimit;
            }
            else if (koeff < -XLimit)
            {
                koeff = -XLimit;
            }

            
            playerControl.SetRotationAngle(koeff);
        }        
        else
        {
            playerControl.SetRotationAngle(0);
        }

        
        if (Mathf.Abs(mouseDelta.y) > 0)
        {            
            cameraControl.ChangeCameraAngleX(mouseDelta.y * -7 * Time.deltaTime);
        }

        //cameraControl.ChangeCameraAngleY(playerTransform.eulerAngles.y);
        mousePosition = Input.mousePosition;
    }

}
