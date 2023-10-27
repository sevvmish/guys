using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerControl))]
public class InputControl : MonoBehaviour
{
    private Joystick joystick;
    private PlayerControl playerControl;
    private PointerBase jump;


    // Start is called before the first frame update
    void Start()
    {
        joystick = GameManager.Instance.GetJoystick();
        playerControl = gameObject.GetComponent<PlayerControl>();
        jump = GameObject.Find("JumpButton").GetComponent<PointerBase>();

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
            playerControl.SetHorizontal(joystick.Horizontal);
            playerControl.SetVertical(joystick.Vertical);
            if (jump.IsPressed)
            {
                playerControl.SetJump();
            }
        }
        else
        {
            //=====================================
            playerControl.SetHorizontal(Input.GetAxis("Horizontal"));
            playerControl.SetVertical(Input.GetAxis("Vertical"));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerControl.SetJump();
            }
        }

        if (Input.GetKey(KeyCode.K))
        {
            playerControl.SetForward();
        }
    }
}
