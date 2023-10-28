using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerControl))]
public class InputControl : MonoBehaviour
{
    private Joystick joystick;
    private PlayerControl playerControl;
    private CameraControl cameraControl;
    private PointerBase jump;
    private PointerBase mover;
    private Vector3 mousePosition;


    // Start is called before the first frame update
    void Start()
    {
        joystick = GameManager.Instance.GetJoystick();
        cameraControl = GameManager.Instance.GetCameraControl();
        playerControl = gameObject.GetComponent<PlayerControl>();
        jump = GameObject.Find("JumpButton").GetComponent<PointerBase>();
        mover = GameObject.Find("Screen mover").GetComponent<PointerBase>();
        if (!Globals.IsMobile)
        {       
            joystick.gameObject.SetActive(false);
            jump.gameObject.SetActive(false);
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
        if (jump.IsPressed)
        {
            playerControl.SetJump();
        }
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
        
        if (mouseDelta.x > 0 || mouseDelta.x < 0)
        {
            playerControl.SetRotationAngle(mouseDelta.x * 12 * Time.deltaTime);
        }        
        else if (Input.mousePosition.x >= Screen.width)
        {
            playerControl.SetRotationAngle(200 * Time.deltaTime);
        }
        else if (Input.mousePosition.x <= 0)
        {
            playerControl.SetRotationAngle(-200 * Time.deltaTime);
        }
        else
        {
            playerControl.SetRotationAngle(0);
        }

        if (Mathf.Abs(mouseDelta.y) > 0)
        {            
            cameraControl.ChangeCameraAngleX(mouseDelta.y * -5 * Time.deltaTime);
        }
        
             
        mousePosition = Input.mousePosition;
    }
}
