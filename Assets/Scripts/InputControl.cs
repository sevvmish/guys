using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.IsMobile)
        {
            playerControl.Horizontal = joystick.Horizontal;
            playerControl.Vertical = joystick.Vertical;
            if (jump.IsPressed)
            {
                playerControl.IsJump = true;
            }

            //=====================================
            playerControl.Horizontal = Input.GetAxis("Horizontal");
            playerControl.Vertical = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                playerControl.IsJump = true;
            }
        }
        else
        {

        }
    }
}
