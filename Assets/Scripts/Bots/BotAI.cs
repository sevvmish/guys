using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAI : MonoBehaviour
{
    public int CurrentIndex { get; private set; } = 0;

    public bool IsCanRun = true;
    public bool IsCanJump = true;
    public bool IsCanDoubleJump = true;

    private PlayerControl playerControl;
    private Transform playerTransform;
    private NavPointSystem nps;
    private GameManager gm;

    private Vector3 lastPosition;
    private float _timerCheckForLastPosition;
    private bool isIndexDowned;

    private float _timer;
    private float _timerForChecking;
    
    private BotNavPoint currentPoint;
    private Action currentAction;
    private bool isChecked = false;

    private bool isWalkChanged;
    
    private HashSet<GameObject> usedNavPoints = new HashSet<GameObject>();

    private readonly float oneJumpDistance = 2.4f;
    private readonly float twoJumpDistance = 5f; // really 4.5
    private readonly float oneJumpAltitude = 2.4f;
    private readonly float twoJumpAltitude = 3.1f;

    private void Start()
    {
        nps = NavPointSystem.Instance;
        playerControl = GetComponent<PlayerControl>();
        gm = GameManager.Instance;
        playerTransform = playerControl.transform;

        //delay before start
        float delay = UnityEngine.Random.Range(0.1f, 0.75f);
        _timer = delay;
        _timerForChecking = delay;

        lastPosition = playerTransform.position;
    }

    private void Update()
    {
        if (isStopAction() || !gm.IsGameStarted || playerControl.IsFinished) return;

        //redirect after non walking
        if (playerControl.IsCanWalk && !isWalkChanged && currentAction == runToPoint && currentPoint != null)
        {
            followPoint(currentPoint);
        }
        isWalkChanged = playerControl.IsCanWalk;
        //============================

        if (_timerForChecking > 0)
        {
            _timerForChecking -= Time.deltaTime;
        }

        if (_timerCheckForLastPosition > 1f)
        {
            _timerCheckForLastPosition = 0;

            if ((lastPosition - playerTransform.position).magnitude < 0.1f && playerControl.IsGrounded && playerControl.IsCanWalk && playerControl.IsCanAct)
            {
                //print("problem!!!");
                if (CurrentIndex > 0 && !isIndexDowned)
                {
                    //CurrentIndex--;
                    isIndexDowned = true;
                    GetComponent<RespawnControl>().Die();
                    //decisionMaking();
                }
            }
            else
            {
                isIndexDowned = false;
            }

            lastPosition = playerTransform.position;
        }
        else
        {
            _timerCheckForLastPosition += Time.deltaTime;
        }

        

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }

        if (currentAction == null) decisionMaking();

        _timer = 0.2f;
    }

    private void FixedUpdate()
    {
        if (isStopAction() || !gm.IsGameStarted) return;

        if (_timerForChecking > 0)
        {
            return;
        }

        currentAction?.Invoke();
    }

    private bool isStopAction()
    {
        if (!gm.IsGameStarted) return true;
        if (playerControl.IsDead || !playerControl.IsCanAct) return true;

        return false;
    }

    private void decisionMaking()
    {
        currentPoint = nps.GetBotNavPoint(CurrentIndex, playerControl.transform.position);
        
        if (currentPoint != null)
        {
            
            if ((currentPoint.transform.position - playerTransform.position).magnitude > 1)
            {
                //print(currentPoint.Index + " = " + currentPoint.transform.position);
                followPoint(currentPoint);
            }
            else
            {

                playerControl.SetForward(false);
                currentAction = null;             
            }
            
        }
        else
        {
            
            playerControl.SetForward(false);
            currentAction = null;
        }
    }

    private void runToPoint()
    {
        if (!playerControl.IsGrounded || playerControl.IsJumping)
        {
            playerControl.SetForward(true);
            return;
        }

        isChecked = Physics.CheckBox(playerTransform.position + playerTransform.forward, new Vector3(0.2f, 1.2f, 0.2f), playerTransform.rotation, ~Globals.ignoreTriggerMask);
            
        if (!isChecked && IsCanJump)
        {            
            currentAction = jumpForwardNoGround;
            return;
        }

        isChecked = Physics.CheckBox(playerTransform.position + Vector3.up * 0.7f + playerTransform.forward, new Vector3(0.2f, 0.2f, 0.2f), playerTransform.rotation, ~Globals.ignoreTriggerMask);

        if (isChecked)
        {
            isChecked = Physics.CheckBox(playerTransform.position + Vector3.up * 0.7f + playerTransform.forward + Vector3.left, new Vector3(0.2f, 0.2f, 0.2f), playerTransform.rotation, ~Globals.ignoreTriggerMask);
            if (!isChecked)
            {
                StartCoroutine(littleTurn(Vector3.left));
                return;
            }
            else
            {
                isChecked = Physics.CheckBox(playerTransform.position + Vector3.up * 0.7f + playerTransform.forward + Vector3.right, new Vector3(0.2f, 0.2f, 0.2f), playerTransform.rotation, ~Globals.ignoreTriggerMask);
                if (!isChecked)
                {
                    StartCoroutine(littleTurn(Vector3.right));
                    return;
                }
                else
                {
                    if (isChecked && IsCanJump)
                    {
                        currentAction = jumpForwardHighGround;
                        return;
                    }
                }
            }
        }

        

        playerControl.SetForward(true);
        _timerForChecking = 0.05f;
    }

    private void followPoint(BotNavPoint _point)
    {        
        if (playerControl.IsCanWalk) playerTransform.LookAt(new Vector3(_point.transform.position.x, playerTransform.position.y, _point.transform.position.z));
        if (currentAction == null) currentAction = runToPoint;
    }

    private IEnumerator littleTurn(Vector3 way)
    {
        playerControl.SetForward(true);
        if (playerControl.IsCanWalk) playerTransform.LookAt(playerTransform.position + way);
        _timerForChecking = 0.35f;
        yield return new WaitForSeconds(0.35f);
        if (playerControl.IsCanWalk) playerTransform.LookAt(new Vector3(currentPoint.transform.position.x, playerTransform.position.y, currentPoint.transform.position.z));
    }

    private void jumpForwardHighGround()
    {
        if (!playerControl.IsJumping && playerControl.IsGrounded)
        {
            isChecked = Physics.CheckBox(playerTransform.position + playerTransform.up * (oneJumpAltitude + 0.2f) + playerTransform.forward * 1.5f, new Vector3(0.1f, 0.1f, 0.1f), playerTransform.rotation, ~Globals.ignoreTriggerMask);

            if (!isChecked)
            {                
                playerControl.SetJump();
                playerControl.SetForward(true);
                _timerForChecking += 0.2f;
                currentAction = runToPoint;
                return;
            }
            else
            {                
                isChecked = Physics.CheckBox(playerTransform.position + playerTransform.up * (twoJumpAltitude + 0.2f) + playerTransform.forward * 1.5f, new Vector3(0.1f, 0.1f, 0.1f), playerTransform.rotation, ~Globals.ignoreTriggerMask);

                if (!isChecked)
                {
                    
                    currentAction = doubleJumpForward;
                    return;
                }
                else
                {
                    currentAction = null;
                    playerControl.SetForward(false);
                    _timerForChecking = 0.1f;
                    return;
                }
            }
        }
        
        _timerForChecking = 0.05f;
    }

    private void doubleJumpForward()
    {      
        if (!IsCanDoubleJump)
        {
            _timerForChecking = 0.1f;
            return;
        }

        if (!playerControl.IsJumping)
        {
            StartCoroutine(doubleJump());
        }
        
        _timerForChecking = 0.05f;
    }
    private IEnumerator doubleJump()
    {        
        playerControl.SetJump();
        playerControl.SetForward(true);        
        yield return new WaitForSeconds(0.1f);

        while (playerControl.GetRigidbody.velocity.y > 0.1f)
        {
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.1f);

        playerControl.SetJump();
        playerControl.SetForward(true);
        currentAction = runToPoint;
    }

    private void jumpForwardNoGround()
    {        
        if (!playerControl.IsJumping && playerControl.IsGrounded)
        {
            isChecked = Physics.CheckBox(playerTransform.position + playerTransform.forward * oneJumpDistance, new Vector3(0.2f, 1f, 0.2f), playerTransform.rotation, ~Globals.ignoreTriggerMask);

            if (isChecked)
            {                
                playerControl.SetJump();
                playerControl.SetForward(true);
                _timerForChecking += 0.1f;
                currentAction = runToPoint;
                return;
            }
            else
            {
                isChecked = Physics.CheckBox(playerTransform.position + playerTransform.forward * twoJumpDistance, new Vector3(0.2f, 1f, 0.2f), playerTransform.rotation, ~Globals.ignoreTriggerMask);

                if (isChecked)
                {
                    
                    currentAction = doubleJumpForward;
                    return;
                }
                else
                {
                    
                    playerControl.SetForward(false);
                    currentAction = null;
                    _timerForChecking = 0.1f;
                    return;
                }
            }
        }

        _timerForChecking = 0.05f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!usedNavPoints.Contains(other.gameObject) && other.TryGetComponent(out BotNavPoint p) && CurrentIndex < p.Index)
        {
            CurrentIndex = p.Index;
            decisionMaking();
            usedNavPoints.Add(other.gameObject);
        }
    }

    public void ResetIndexToValue(int newIndex)
    {
        CurrentIndex = newIndex;
        usedNavPoints.Clear();
        decisionMaking();
    }

}
