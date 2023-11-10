using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    [Header("Controls")]    
    public Animator _animator;
    public bool IsItMainPlayer { get; private set; }
    private ConditionControl conditions;
    public EffectsControl effectsControl;

    [Header("Ragdoll")]
    public CapsuleCollider[] ragdollColliders;
    private Rigidbody[] ragdollRigidbodies;
    private Vector3[] ragdollPos;
    private Vector3[] ragdollRot;
    
    private bool isRagdollFollow;
    private bool isRagdollHasContact;
    private RagdollPartCollisionChecker collisionChecker;

    /*
    public Transform CurrentActivePlatform { get; private set; }
    public bool IsOnPlatform { get; private set; }
    public Transform DangerZone { get; private set; }*/
    private Transform playerLocation;

    //INPUT
    public float angleYForMobile { get; private set; }
    public void SetHorizontal(float hor) => horizontal = hor;
    public void SetVertical(float ver) => vertical = ver;
    public void SetRotationAngle(float ang) => angleY = ang;
    public void SetJump() => isJump = true;
    public Rigidbody GetRigidbody => _rigidbody;
    public void SetForward(bool isOk) => isForward = isOk;
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;
    [SerializeField] private float angleY;
    
    private bool isJump;    
    private bool isForward;

    //SPEED
    public float PlayerMaxSpeed { get; private set; }
    public float PlayerCurrentSpeed { get; private set; }
    public float PlayerVelocity { get; private set; }
    public float PlayerNonVerticalVelocity { get; private set; }
    public float PlayerVerticalVelocity { get; private set; }

    //CONDITIONS
    public bool IsGrounded { get; private set; }
    public bool IsRagdollActive { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsIdle { get; private set; }

    public bool IsSecondJump { get; private set; }
    public bool IsFloating { get; private set; }
    public bool IsCanAct { get; private set; }
    public bool IsDead { get; private set; }

    private Rigidbody _rigidbody;
    private CapsuleCollider mainCollider;
    private Transform _transform;
    public GameManager gm;
    private CameraControl cc;

    private float jumpCooldown;
    private float howLongNonGrounded;
    private float howLongMoving;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        cc = GameManager.Instance.GetCameraControl();
        playerLocation = gm.GetPlayerLocation();
        gameObject.AddComponent<ConditionControl>();
        conditions = GetComponent<ConditionControl>();
        conditions.SetData(this, effectsControl);

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.mass = PhysicsCustomizing.GetData(PhysicObjects.Player).Mass;
        _rigidbody.drag = PhysicsCustomizing.GetData(PhysicObjects.Player).Drag;
        _rigidbody.angularDrag = PhysicsCustomizing.GetData(PhysicObjects.Player).AngularDrag;

        _transform = GetComponent<Transform>();
        mainCollider = GetComponent<CapsuleCollider>();
        PlayerMaxSpeed = Globals.BASE_SPEED;
        PlayerCurrentSpeed = PlayerMaxSpeed;
        IsCanAct = true;

        ragdollRigidbodies = new Rigidbody[ragdollColliders.Length];
        ragdollPos = new Vector3[ragdollColliders.Length];
        ragdollRot = new Vector3[ragdollColliders.Length];
        for (int i = 0; i < ragdollColliders.Length; i++)
        {
            ragdollColliders[i].enabled = false;
            ragdollRigidbodies[i] = ragdollColliders[i].GetComponent<Rigidbody>();
            ragdollRigidbodies[i].useGravity = false;
            ragdollRigidbodies[i].mass = PhysicsCustomizing.GetData(PhysicObjects.Ragdoll).Mass;
            ragdollRigidbodies[i].drag = PhysicsCustomizing.GetData(PhysicObjects.Ragdoll).Drag;
            ragdollRigidbodies[i].angularDrag = PhysicsCustomizing.GetData(PhysicObjects.Ragdoll).AngularDrag;

            ragdollPos[i] = ragdollColliders[i].transform.localPosition;
            ragdollRot[i] = ragdollColliders[i].transform.localEulerAngles;
        }
        collisionChecker = ragdollRigidbodies[0].GetComponent<RagdollPartCollisionChecker>();
        IsRagdollActive = false;
        
    }

    public void SetPlayerToMain()
    {
        IsItMainPlayer = true;
        effectsControl.SetShadow(this);
    }

    public void ChangeSpeed(float multiplier, float seconds)
    {
        StartCoroutine(changeSpeed(multiplier, seconds));
    }
    private IEnumerator changeSpeed(float multiplier, float seconds)
    {
        PlayerCurrentSpeed *= multiplier;

        if (IsItMainPlayer) print(PlayerCurrentSpeed);

        for (float i = 0; i < seconds; i+=0.1f)
        {            
            yield return new WaitForSeconds(0.1f);
            if (IsDead && IsRagdollActive) break;
        }

        PlayerCurrentSpeed /= multiplier;
    }

    private void Update()
    {
        if (jumpCooldown > 0) jumpCooldown -= Time.deltaTime;
                
        if (Input.GetKeyDown(KeyCode.Q) && IsItMainPlayer)
        {
            SetRagdollState(true);
        }

        if (Input.GetKeyDown(KeyCode.E) && IsItMainPlayer)
        {
            SetRagdollState(false);
        }

        if (Input.GetKeyDown(KeyCode.U) && IsItMainPlayer)
        {
            _rigidbody.AddRelativeForce(Vector3.forward * 5, ForceMode.Impulse);
        }

        if (isForward)
        {
            //isForward = false;
            movement(true);
        }
        else
        {
            movement(false);
        }
        
        /*
        if ( !CurrentActivePlatform || (CurrentActivePlatform && !CurrentActivePlatform.gameObject.activeSelf) || !CurrentActivePlatform.CompareTag("Platform"))
        {
            if (IsOnPlatform)
            {                
                _transform.SetParent(playerLocation);
                IsOnPlatform = false;
            }
        }
        else
        {
            checkPlatforms();
        }*/
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {        
        IsGrounded = checkGround();
        //if (!IsItMainPlayer) print(IsSecondJump);
        checkShadow();
        PlayerVelocity = _rigidbody.velocity.magnitude;
        PlayerNonVerticalVelocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z).magnitude;
        PlayerVerticalVelocity = new Vector3(0, _rigidbody.velocity.y, 0).magnitude;
        //if (PlayerVelocity > 0.1f && !IsItMainPlayer) print(PlayerVelocity);
        
        if (!IsGrounded)
        {
            howLongNonGrounded += Time.deltaTime;
            
            if (!IsRagdollActive)
            {
                GravityScale(_rigidbody);
            }
            else
            {
                for (int i = 0; i < ragdollRigidbodies.Length; i++)
                {
                    GravityScale(ragdollRigidbodies[i]);
                }
            }
            
        }
        else
        {
            howLongNonGrounded = 0;

            /*
            if (IsRunning && _rigidbody.mass != PhysicsCustomizing.GetData(PhysicObjects.Player).Mass)
            {
                _rigidbody.mass = PhysicsCustomizing.GetData(PhysicObjects.Player).Mass;
            }
            else if (IsIdle && _rigidbody.mass != PhysicsCustomizing.GetData(PhysicObjects.Player).Mass * 50)
            {
                _rigidbody.mass = PhysicsCustomizing.GetData(PhysicObjects.Player).Mass * 50;
            }
            */

            /*
            if (IsRunning && _rigidbody.drag != PhysicsCustomizing.GetData(PhysicObjects.Player).Drag)
            {
                _rigidbody.drag = PhysicsCustomizing.GetData(PhysicObjects.Player).Drag;
            }
            else if (IsIdle && _rigidbody.drag != PhysicsCustomizing.GetData(PhysicObjects.Player).Drag * 50)
            {
                _rigidbody.drag = PhysicsCustomizing.GetData(PhysicObjects.Player).Drag * 50;
            }*/

            if (_rigidbody.drag != PhysicsCustomizing.GetData(PhysicObjects.Player).Drag) _rigidbody.drag = PhysicsCustomizing.GetData(PhysicObjects.Player).Drag;
        }

        if (isJump) makeJump();


        playAnimation();
        if (isRagdollFollow)
        {
            isRagdollHasContact = collisionChecker.IsRagdollHasContact;
            _rigidbody.MovePosition(ragdollRigidbodies[0].transform.position);
        }
        else if (!IsItMainPlayer && !IsRagdollActive)
        {
            ragdollRigidbodies[0].transform.localPosition = Vector3.zero;
        }
    }

    private void makeJump()
    {        
        isJump = false;
        if (!IsCanAct || IsRagdollActive) return;

        if (IsGrounded && jumpCooldown <= 0 && !IsJumping)
        {
            _rigidbody.velocity = Vector3.zero;
            effectsControl.MakeJumpFX();
            _animator.Play("JumpStart");
            _rigidbody.AddRelativeForce(Vector3.up * Globals.JUMP_POWER/* + Vector3.forward * PlayerNonVerticalVelocity * 4*/, ForceMode.Impulse);
            IsJumping = true;
            jumpCooldown = 0.1f;
        }    
        else if (!IsGrounded && IsJumping && !IsSecondJump && jumpCooldown <= 0)
        {
            effectsControl.MakeJumpFX();
            IsSecondJump = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddRelativeForce(Vector3.up * Globals.JUMP_POWER * 0.5f + Vector3.forward * 2, ForceMode.Impulse);
            _animator.Play("JumpStart");
        }        
    }

    private bool checkGround()
    {
        bool result = Physics.CheckBox(_transform.position + Vector3.down * 0.2f, new Vector3(0.25f, 0.05f, 0.25f), Quaternion.identity, 3, QueryTriggerInteraction.Ignore);
        if (!IsGrounded && result && PlayerVerticalVelocity > 5 && !IsRagdollActive) effectsControl.MakeLandEffect();  
        
        if (result)
        {            
            if (jumpCooldown <= 0) IsJumping = false;
            IsFloating = false;
            IsSecondJump = false;
        }
                        
        return result;
    }

    private void checkShadow()
    {
        if (IsItMainPlayer)
        {
            effectsControl.ShowShadow(IsCanAct && !IsRagdollActive);
        }
        else
        {
            effectsControl.ShowShadow(IsGrounded && !IsRagdollActive);
        }
        
    }
        
    private void movement(bool forward)
    {
        if (!IsCanAct) return;

        if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0 || forward || Mathf.Abs(angleY) > 0)
        {
            float turnKoeff = PlayerCurrentSpeed * 0.03f;

            if (Globals.IsMobile)
            {
                if ((Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0))
                {                    
                    if (Mathf.Abs(angleY) > 0)
                    {
                        angleYForMobile += angleY;
                    }

                    float angle = Mathf.Atan2(horizontal, vertical) * 180 / Mathf.PI;
                    _rigidbody.DORotate(new Vector3(_transform.eulerAngles.x, angleYForMobile + angle, _transform.eulerAngles.z), 0);
                    //_transform.eulerAngles = new Vector3(_transform.eulerAngles.x, angleYForMobile + angle, _transform.eulerAngles.z);

                }
                else if (horizontal == 0 && vertical == 0 && Mathf.Abs(angleY) > 0)
                {

                    angleYForMobile += angleY;                    
                    _rigidbody.DORotate(new Vector3(_transform.eulerAngles.x, angleYForMobile, _transform.eulerAngles.z), 0);
                    //_transform.eulerAngles = new Vector3(_transform.eulerAngles.x, angleYForMobile, _transform.eulerAngles.z);
                }
            }            
            else if (!Globals.IsMobile && Mathf.Abs(angleY) > 0)
            {
                angleYForMobile += angleY;
                _rigidbody.DORotate(new Vector3(_transform.eulerAngles.x, angleYForMobile, _transform.eulerAngles.z), 0);
                //_transform.eulerAngles = new Vector3(_transform.eulerAngles.x, angleYForMobile, _transform.eulerAngles.z);
            }

            angleY = 0;

            if (forward)
            {
                forward = false;
                vertical = 1;
            }

            if ((PlayerVelocity < PlayerCurrentSpeed && IsGrounded) || (PlayerVelocity < PlayerCurrentSpeed * 1.25f && !IsGrounded))
            {
                float koeff = 0;
                float addKoeff = IsGrounded ? 1 : 1.3f;
                                                
                koeff = PlayerCurrentSpeed * addKoeff * new Vector2(horizontal, vertical).magnitude - PlayerVelocity;
                                                
                koeff = koeff > 0 ? koeff : 0;       
                
                if (Globals.IsMobile)
                {
                    _rigidbody.velocity += _transform.forward * koeff;
                }
                else
                {
                    if (vertical > 0)
                    {
                        _rigidbody.velocity += _transform.forward * koeff + _transform.right * horizontal;
                        
                    }
                    else if (vertical < 0)
                    {
                        _rigidbody.velocity += _transform.forward * -1 * koeff + _transform.right * horizontal;
                    }
                    else if (vertical == 0 && horizontal != 0)
                    {
                        _rigidbody.velocity += _transform.forward * koeff + _transform.right * horizontal;
                    }
                    
                }

                if (koeff > 0) playRun();

            }
            
            howLongMoving = 0;
            horizontal = 0;
            vertical = 0;
        }
        else
        {
            if (howLongMoving < 2 && IsGrounded)
            {                
                howLongMoving++;                
                _rigidbody.velocity = Vector3.zero;
            }

            playIdle();
        }
    }


    private void playRun()
    {
        IsRunning = true;
        IsIdle = false;

        if (IsGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) _animator.Play("Run");
    }

    private void playIdle()
    {
        IsRunning = false;
        IsIdle = true;

        if (IsGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _animator.Play("Idle");
        }
    }

    private void playAnimation()
    {
        if (!IsGrounded)        
        {
            if (IsJumping)
            {
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("JumpStart") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("JumpLoop")) _animator.Play("JumpStart");
            }
            else if (howLongNonGrounded > 0.1f)
            {
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("JumpLoop")) _animator.Play("JumpLoop");
            }
        }
    }

    private void GravityScale(Rigidbody r)
    {
        float fallingKoeff = 1;
        //if (r.drag != 2) r.drag = 2;

        if (r.velocity.y >= 0)
        {
            r.AddForce(Physics.gravity * Globals.GRAVITY_KOEFF * r.mass);
        }
        else if (r.velocity.y < 0)
        {
            if (!IsFloating)
            {
                if (r.drag != 2) r.drag = 2;
                if (fallingKoeff < 8) fallingKoeff *= 1.2f;
                r.AddForce(Physics.gravity * r.mass * Globals.GRAVITY_KOEFF * fallingKoeff);
            }
            else
            {
                //if (r.drag != 3) r.drag = 3;
                r.AddRelativeForce(Vector3.forward * 20, ForceMode.Force);
                r.AddForce(Physics.gravity * r.mass * Globals.GRAVITY_KOEFF * 0.1f);
            }            
        }
    }

    /*
    public void FreePlatformStatusForPlayer()
    {
        if (IsOnPlatform)
        {
            _transform.SetParent(playerLocation);
            IsOnPlatform = false;
            CurrentActivePlatform = null;
        }
    }

    private void checkPlatforms()
    {        
        if (CurrentActivePlatform != null)
        {
            if (_transform.parent != CurrentActivePlatform)
            {
                _transform.SetParent(CurrentActivePlatform);
                IsOnPlatform = true;
            }            
        }
        else
        {
            if (IsOnPlatform)
            {
                _transform.SetParent(playerLocation);
                IsOnPlatform = false;
            }
        }
        
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision != null && collision.collider.CompareTag("Platform"))
        {
            if (CurrentActivePlatform != collision.collider.transform)
            {
                CurrentActivePlatform = collision.collider.transform;
                _transform.SetParent(CurrentActivePlatform);
                IsOnPlatform = true;
            }            
        }        
        else if (collision != null && collision.collider.CompareTag("Danger"))
        {
            if (DangerZone != collision.collider.transform)
            {
                DangerZone = collision.collider.transform;
            }
        }*/

        if (collision != null && collision.collider.gameObject.layer == Globals.LAYER_DANGER)
        {            
            float i = collision.impulse.magnitude;
            if (i < Globals.BASE_SPEED) return;

            
            //if (i > 30)
            //{
                ApplyTrapForce(collision.impulse, collision.GetContact(0).point, ApplyForceType.Punch_easy);
            //}
        }
    }

    
    private void OnCollisionExit(Collision collision)
    {
        /*
        if (collision.collider.transform == CurrentActivePlatform)
        {
            CurrentActivePlatform = null;
            if (IsOnPlatform)
            {
                _transform.SetParent(playerLocation);
                IsOnPlatform = false;
            }                
        }
        else if (collision.collider.transform == DangerZone)
        {
            DangerZone = null;
        }*/
    }

    private void SetRagdollState(bool isActive)
    {
        if (isActive)
        {
            IsCanAct = false;
            IsJumping = false;
            IsFloating = false;
            IsSecondJump = false;
            _rigidbody.velocity = Vector3.zero;

            _rigidbody.mass = 0.1f;
            mainCollider.enabled = false;
            //_rigidbody.useGravity = false;
            //_rigidbody.isKinematic = true;
            
            _animator.enabled = false;
            for (int i = 0; i < ragdollColliders.Length; i++)
            {
                ragdollColliders[i].enabled = true;
                ragdollRigidbodies[i].useGravity = true;
            }
            isRagdollFollow = true;
            IsRagdollActive = true;
            
        }
        else
        {
            Vector3 vel = ragdollRigidbodies[0].velocity;
            ragdollRigidbodies[0].velocity = Vector3.zero;            
            Vector3 pos = ragdollRigidbodies[0].transform.position;
            for (int i = 0; i < ragdollColliders.Length; i++)
            {
                ragdollColliders[i].enabled = false;
                ragdollRigidbodies[i].useGravity = false;                
            }            
            StartCoroutine(playRagdollOff(pos, vel));
            
        }        
    }
    
    private IEnumerator playRagdollOff(Vector3 pos, Vector3 vel)
    {
        isRagdollFollow = false;
        _transform.position = pos;
        mainCollider.enabled = true;
        _rigidbody.mass = Globals.MASS;
        _rigidbody.velocity = vel;

        for (int i = 0; i < ragdollColliders.Length; i++)
        {            
            ragdollColliders[i].transform.DOLocalMove(ragdollPos[i], 0.2f);
            ragdollColliders[i].transform.DOLocalRotate(ragdollRot[i], 0.2f);
        }

        yield return new WaitForSeconds(0.2f);
        
        collisionChecker.IsRagdollHasContact = false;
        _animator.enabled = true;
        IsCanAct = true;
        IsRagdollActive = false;
    }

    public void Respawn(Vector3 pos, Vector3 rot)
    {        
        StartCoroutine(playRespawn(pos, rot));
    }
    private IEnumerator playRespawn(Vector3 pos, Vector3 rot)
    {
        IsDead = true;
        IsCanAct = false;
        isJump = false;
        IsJumping = false;
        IsFloating = false;
        IsSecondJump = false;

        yield return new WaitForSeconds(0.2f);
        if (IsRagdollActive) SetRagdollState(false);
        _rigidbody.velocity = Vector3.zero;
        _animator.StopPlayback();
        if (IsItMainPlayer)
        {
            cc.ResetCameraOnRespawn();
            angleYForMobile = 0;
        }

        ragdollRigidbodies[0].transform.localPosition = Vector3.zero;
        
        playIdle();

        _transform.position = pos;
        _transform.eulerAngles = rot;
        
        effectsControl.PlayRespawnEffect();

        IsDead = false;
        IsCanAct = true;
        PlayerCurrentSpeed = PlayerMaxSpeed;
    }


    public void ApplyTrapForce(Vector3 forceVector, Vector3 contactPoint, ApplyForceType punchType)
    {
        if (IsRagdollActive || !IsCanAct) return;

        effectsControl.PlayPunchEffect(punchType, contactPoint);

        SetRagdollState(true);
        
        for (int i = 0; i < ragdollRigidbodies.Length; i++)
        {            
            ragdollRigidbodies[i].velocity = forceVector;
        }        
        StartCoroutine(playTurnOffRagdoll(2));
    }
    private IEnumerator playTurnOffRagdoll(float sec)
    {    
        while (ragdollRigidbodies[0].velocity.magnitude > Globals.BASE_SPEED)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        SetRagdollState(false);
    }

}

public struct PhysicsCustomizing
{
    public float Mass;
    public float Drag;
    public float AngularDrag;

    public static PhysicsCustomizing GetData(PhysicObjects _type)
    {
        PhysicsCustomizing result = new PhysicsCustomizing();
        switch (_type)
        {
            case PhysicObjects.Player:
                result.Mass = Globals.MASS;
                result.Drag = Globals.DRAG;
                result.AngularDrag = Globals.ANGULAR_DRAG;
                break;

            case PhysicObjects.Ragdoll:
                result.Mass = Globals.RAGDOLL_MASS;
                result.Drag = Globals.RAGDOLL_DRAG;
                result.AngularDrag = Globals.RAGDOLL_ANGULAR_DRAG;
                break;
        }

        return result;
    }

    

}

public enum PhysicObjects
{
    Player,
    Ragdoll
}

public enum ApplyForceType
{
    Punch_easy,
    Punch_medium,
    Punch_large
}
