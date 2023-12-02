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
    private float koeff;
    private readonly float XLimit = 10;
    private GameManager gm;

    private bool isTouchZoom;
    private Vector3 zoom1Finger;
    private Vector3 zoom2Finger;
    private float zoomDistance;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        joystick = GameManager.Instance.GetJoystick();
        cameraControl = GameManager.Instance.GetCameraControl();
        playerControl = gameObject.GetComponent<PlayerControl>();
        playerTransform = playerControl.transform;
        
        if (!Globals.IsMobile)
        {       
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.Confined;            
            Cursor.visible = false;
        }
        else
        {
            jump = GameObject.Find("JumpButton").GetComponent<PointerBase>();
            mover = GameObject.Find("Screen mover").GetComponent<PointerBase>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.IsGameStarted) return;

        if (Globals.IsMobile)
        {
            forMobile();
        }
        else
        {
            forPC();
        }        
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(t());
        }
    }

    private IEnumerator t()
    {
        Cursor.lockState = CursorLockMode.Locked;
        yield return new WaitForSeconds(0.1f);
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void forMobile()
    {
        /*
        if (Input.touchCount >= 2)
        {         
            isTouchZoom = true;

            zoom1Finger = Input.GetTouch(0).position;
            zoom2Finger = Input.GetTouch(1).position;
            float newZoomDistance = Vector2.Distance(zoom1Finger, zoom2Finger);

            gm.GetTestText().text = "new: " + newZoomDistance.ToString("f2") + "old: " + zoomDistance.ToString("f2");

            zoomDistance = newZoomDistance;
        }
        else
        {            
            isTouchZoom = false;
        }*/

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

        Vector3 mouseDelta = Input.mousePosition - mousePosition;
        //gm.GetTestText().text = Input.mousePosition + " = " + mouseDelta;

        if ((mouseDelta.x > 0 && Input.mousePosition.x < (Screen.width-5)) || (mouseDelta.x < 0 && (Input.mousePosition.x > 5)))
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

            Cursor.lockState = CursorLockMode.Confined;
            playerControl.SetRotationAngle(koeff);
        }
        else if (Input.mousePosition.x >= Screen.width-5)
        {
            Cursor.lockState = CursorLockMode.Locked;
            playerControl.SetRotationAngle(XLimit * 0.5f);
        }
        else if (Input.mousePosition.x <= 5)
        {
            Cursor.lockState = CursorLockMode.Locked;
            playerControl.SetRotationAngle(-XLimit * 0.5f);
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
