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
    private Vector3 mousePosition;
    
  
    // Start is called before the first frame update
    void Start()
    {
        joystick = GameManager.Instance.GetJoystick();
        cameraControl = GameManager.Instance.GetCameraControl();
        playerControl = gameObject.GetComponent<PlayerControl>();
        playerTransform = playerControl.transform;
        jump = GameObject.Find("JumpButton").GetComponent<PointerBase>();
        mover = GameObject.Find("Screen mover").GetComponent<PointerBase>();
        if (!Globals.IsMobile)
        {       
            joystick.gameObject.SetActive(false);
            jump.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.IsMobile)
        {
            forMobile();

#if UNITY_EDITOR
            //forPC();
#endif
        }
        else
        {
            forPC();
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
                
        Vector2 delta = mover.DeltaPosition.normalized;
     
        if (delta.x > 0 || delta.x < 0)
        {
            playerControl.SetRotationAngle(delta.x * 300 * Time.deltaTime);
        }        
        else if (delta.x == 0)
        {
            playerControl.SetRotationAngle(0);
        }

        if (Mathf.Abs(delta.y) > 0)
        {
            cameraControl.ChangeCameraAngleX(delta.y * -70 * Time.deltaTime);
        }

        cameraControl.ChangeCameraAngleY(playerControl.angleYForMobile);
    }

    private void forPC()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        playerControl.SetHorizontal(horizontal);
        playerControl.SetVertical(vertical);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerControl.SetJump();
        }

        Vector3 mouseDelta = Input.mousePosition - mousePosition;

        //print(Input.mousePosition.x + " = " + Screen.width + " = " + (Screen.width - 1));
        
        if (mouseDelta.x > 0 || mouseDelta.x < 0)
        {
            playerControl.SetRotationAngle(mouseDelta.x * 20 * Time.deltaTime);
        }        
        else if (Input.mousePosition.x >= Screen.width-50)
        {
            playerControl.SetRotationAngle(200 * Time.deltaTime);
        }
        else if (Input.mousePosition.x <= 1)
        {
            playerControl.SetRotationAngle(-200 * Time.deltaTime);
        }
        else
        {
            playerControl.SetRotationAngle(0);
        }

        if (Mathf.Abs(mouseDelta.y) > 0)
        {            
            cameraControl.ChangeCameraAngleX(mouseDelta.y * -7 * Time.deltaTime);
        }

        cameraControl.ChangeCameraAngleY(playerTransform.eulerAngles.y);
        mousePosition = Input.mousePosition;
    }
}
