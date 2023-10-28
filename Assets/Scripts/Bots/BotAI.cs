using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BotAI : MonoBehaviour
{
    private PlayerControl playerControl;
    private Transform playerTransform;
    private NavPointSystem nps;
    private GameManager gm;

    private float _timer;
    private float _timerCheckForward;
    private bool isChecked;

    private BotNavPoint currentPoint;
    private Action currentAction;

    private void Start()
    {
        nps = NavPointSystem.Instance;
        playerControl = GetComponent<PlayerControl>();
        gm = GameManager.Instance;
        playerTransform = playerControl.transform;
    }

    private void Update()
    {
        if (!gm.IsGameStarted) return;

        

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }

        decisionMaking();

        _timer = 0.2f;
    }

    private void FixedUpdate()
    {
        currentAction?.Invoke();
    }

    private void decisionMaking()
    {
        currentPoint = nps.GetBotNavPoint();
        if (currentPoint != null)
        {
            if ((currentPoint.transform.position - playerTransform.position).magnitude > 1)
            {
                followPoint(currentPoint);
            }
            else
            {
                currentAction = null;
            }
            
        }
        else
        {
            currentAction = null;
        }
    }

    private void runToPoint()
    {        
        if (_timerCheckForward > 0.05f)
        {
            _timerCheckForward = 0;
            
            isChecked = Physics.CheckBox(playerTransform.position + Vector3.up * 0.5f + playerTransform.forward, new Vector3(0.3f, 0.3f, 0.3f), playerTransform.rotation);
            
            if (isChecked)
            {
                playerControl.SetJump();
            }
            else
            {
                isChecked = Physics.CheckBox(playerTransform.position + Vector3.down * 1f + playerTransform.forward, new Vector3(0.5f, 1f, 0.5f), playerTransform.rotation);
                if (!isChecked)
                {
                    playerControl.SetJump();
                }
            }

            
        }
        else
        {            
            _timerCheckForward += Time.deltaTime;
        }

        playerControl.SetForward();
    }

    private void followPoint(BotNavPoint _point)
    {
        playerTransform.LookAt(new Vector3(_point.transform.position.x, playerTransform.position.y, _point.transform.position.z));
        currentAction = runToPoint;
    }


}
