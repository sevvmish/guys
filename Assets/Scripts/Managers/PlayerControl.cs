using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    [Header("Controls")]
    private Animator _animator;
    public Skins CurrentSkin { get; private set; }
    public AnimationStates AnimationState { get; private set; }
    public bool IsItMainPlayer { get; private set; }
    private ConditionControl conditions;
    private EffectsControl effectsControl;
    private AbilityManager abilityManager;

    [Header("Ragdoll")]
    private CapsuleCollider[] ragdollColliders;
    private Rigidbody[] ragdollRigidbodies;
    private Vector3[] ragdollPos;
    private Vector3[] ragdollRot;
    
    private bool isRagdollFollow;
    private bool isRagdollHasContact;
    private RagdollPartCollisionChecker collisionChecker;

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
    private bool isFunnySound;

    public bool IsPlatformTouched { get; private set; }

    //SPEED
    public float PlayerMaxSpeed { get; private set; }
    public float PlayerCurrentSpeed { get; private set; }
    public float PlayerVelocity { get; private set; }
    public float PlayerNonVerticalVelocity { get; private set; }
    public float PlayerVerticalVelocity { get; private set; }

    //CONDITIONS
    public bool IsGrounded { get; private set; }
    public bool IsRagdollActive { get; private set; }
    public bool IsCanJump { get; private set; }
    public bool IsCanWalk { get; private set; }
    public bool IsSpeedChanged { get; private set; }
    public bool IsFinished { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsIdle { get; private set; }
    public bool IsSlide { get; private set; }

    public bool IsFreeFall { get; private set; }
    public void SetFreeFall(bool isFree) => IsFreeFall = isFree;

    public bool IsSecondJump { get; private set; }
    public bool IsFloating { get; private set; }
    public bool IsCanAct { get; private set; }
    public bool IsDead { get; private set; }

    private Rigidbody _rigidbody;
    private CapsuleCollider mainCollider;
    private Transform _transform;
    private GameManager gm;
    private CameraControl cc;
    private BotAI bot;

    private float jumpCooldown;
    private float howLongNonGrounded;
    private float howLongMoving;
    private float checkGroundTimer;

    private WaitForSeconds ZeroOne = new WaitForSeconds(0.1f);

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        cc = GameManager.Instance.GetCameraControl();
        playerLocation = gm.GetPlayersLocation();
        gameObject.AddComponent<ConditionControl>();
        conditions = GetComponent<ConditionControl>();
        conditions.SetData(this, effectsControl);

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.mass = PhysicsCustomizing.GetData(PhysicObjects.Player).Mass;
        _rigidbody.drag = PhysicsCustomizing.GetData(PhysicObjects.Player).Drag;
        _rigidbody.angularDrag = PhysicsCustomizing.GetData(PhysicObjects.Player).AngularDrag;

        _transform = GetComponent<Transform>();
        mainCollider = GetComponent<CapsuleCollider>();
        
        if (Globals.IsMobile)
        {
            PlayerMaxSpeed = 7.5f;
        }
        else
        {
            PlayerMaxSpeed = 8f;
        }
        
        //PlayerMaxSpeed = Globals.BASE_SPEED;
        PlayerCurrentSpeed = PlayerMaxSpeed;
        IsCanAct = true;
        IsCanJump = true;
        IsCanWalk = true;

        //Abilities        
        abilityManager = this.gameObject.GetComponent<AbilityManager>();
        abilityManager.SetData(this, effectsControl);
        effectsControl.SetData(_animator, _rigidbody);
    }

    public void SetEffectControl(EffectsControl ef) => effectsControl = ef;

    public void SetSkinData(CapsuleCollider[] ragdolls, Animator anim, Skins skin)
    {
        CurrentSkin = skin;

        _animator = anim;
        AnimationState = AnimationStates.Idle;
        _animator.Play("Idle");

        ragdollColliders = ragdolls;

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
        ragdollRigidbodies[0].gameObject.AddComponent<RagdollPartCollisionChecker>();
        collisionChecker = ragdollRigidbodies[0].GetComponent<RagdollPartCollisionChecker>();
        collisionChecker.LinkToPlayerControl = this;
        IsRagdollActive = false;

    }

    public void SetPlayerToMain()
    {
        IsItMainPlayer = true;
        if (Globals.IsLowFPS) effectsControl.SetShadow(this);
    }

    public void StopJumpPermission(float seconds)
    {
        StartCoroutine(jumpPermission(seconds));
    }
    private IEnumerator jumpPermission(float seconds)
    {
        IsCanJump = false;

        for (float i = 0; i < seconds; i += 0.1f)
        {
            yield return ZeroOne;
            if (IsDead || IsRagdollActive) break;
        }

        IsCanJump = true;
    }

    public void SetSlide(bool isActive)
    {
        IsSlide = isActive;
        StartCoroutine(setSliderData());
    }
    private IEnumerator setSliderData()
    {
        yield return ZeroOne;
        mainCollider.material = gm.GetSlidingPhysicsMaterial();        
    }

    public void StopWalkPermission(float seconds)
    {
        StartCoroutine(walkPermission(seconds));
    }
    private IEnumerator walkPermission(float seconds)
    {
        IsCanWalk = false;

        for (float i = 0; i < seconds; i += 0.1f)
        {
            yield return ZeroOne;
            if (IsDead || IsRagdollActive) break;
        }

        IsCanWalk = true;
    }

    public void StopActPermission(float seconds)
    {
        StartCoroutine(actPermission(seconds));
    }
    private IEnumerator actPermission(float seconds)
    {
        IsCanAct = false;

        for (float i = 0; i < seconds; i += 0.1f)
        {
            yield return ZeroOne;
            if (IsDead) break;
        }

        IsCanAct = true;
    }

    public void ChangeSpeed(float multiplier, float seconds)
    {        
        StartCoroutine(changeSpeed(multiplier, seconds));
    }
    private IEnumerator changeSpeed(float multiplier, float seconds)
    {
        PlayerCurrentSpeed *= multiplier;
        IsSpeedChanged = true;
                
        for (float i = 0; i < seconds; i+=0.1f)
        {            
            yield return ZeroOne;
            if (IsDead || IsRagdollActive) break;
        }

        IsSpeedChanged = false;
        PlayerCurrentSpeed /= multiplier;
    }

    private void Update()
    {
        if (!gm.IsGameStarted || IsFinished)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }

        if (jumpCooldown > 0) jumpCooldown -= Time.deltaTime;

        if (!IsSlide)
        {
            if (isForward && IsCanWalk)
            {
                movement(true);
            }
            else
            {
                movement(false);
            }
        }
        else
        {
            if (isForward && IsCanWalk)
            {
                movementSliding(true);
            }
            else
            {
                movementSliding(false);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gm.IsGameStarted || IsFinished) return;

        if (checkGroundTimer >= Time.fixedDeltaTime * 3)
        {
            checkGroundTimer = 0;
            IsGrounded = checkGround();
        }
        else
        {
            checkGroundTimer += Time.fixedDeltaTime;
        }



        //IsGrounded = checkGround();
        //if (IsItMainPlayer) print(IsGrounded);
        if (IsItMainPlayer && Globals.IsLowFPS) checkShadow();
        PlayerVelocity = _rigidbody.velocity.magnitude;
        PlayerNonVerticalVelocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z).magnitude;
        PlayerVerticalVelocity = new Vector3(0, _rigidbody.velocity.y, 0).magnitude;

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

    public void FinishReached()
    {
        if (IsFinished) return;
        IsFinished = true;
        IsRunning = false;
        IsIdle = true;
        isJump = false;
        IsSecondJump = false;
        isForward = false;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        AnimationState = AnimationStates.Idle;
        _animator.Play("IdlePlus");
    }

    private void makeJump()
    {        
        isJump = false;
        if (!IsCanAct || IsRagdollActive) return;
        
        if (IsGrounded && jumpCooldown <= 0 && !IsJumping && IsCanJump)
        {
            float addKoef = 1;
           
            _rigidbody.velocity = Vector3.zero;
            effectsControl.MakeJumpFX();
            
            _animator.Play("JumpStart");
            AnimationState = AnimationStates.Fly;

            if (IsSlide)
            {
                addKoef = 2.2f;
                //_rigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
            }

            _rigidbody.AddRelativeForce(Vector3.up * Globals.JUMP_POWER * addKoef, ForceMode.Impulse);
            IsJumping = true;
            jumpCooldown = 0.2f;
        }    
        else if (!IsSlide && !IsGrounded && IsJumping && !IsSecondJump && jumpCooldown <= 0 && IsCanJump)
        {            
            jumpCooldown = 0.2f;
            effectsControl.MakeJumpFX();
            IsSecondJump = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddRelativeForce(Vector3.up * Globals.JUMP_POWER * 0.5f + Vector3.forward * 2, ForceMode.Impulse);
            
            _animator.Play("JumpStart");
            AnimationState = AnimationStates.Fly;
        }        
    }

    private bool checkGround()
    {
        //bool result = Physics.CheckBox(_transform.position + Vector3.down * 0.2f, new Vector3(0.25f, 0.05f, 0.25f), _transform.rotation, 3, QueryTriggerInteraction.Ignore);
        bool result = Physics.CheckBox(_transform.position + Vector3.down * 0f, new Vector3(0.25f, 0.05f, 0.25f), _transform.rotation, 3, QueryTriggerInteraction.Ignore);



        if (!IsGrounded && result && PlayerVerticalVelocity > 5 && !IsRagdollActive) effectsControl.MakeLandEffect();

        if (!IsGrounded && result)
        {
            //if (IsItMainPlayer) print(_animator.GetCurrentAnimatorStateInfo(0) + " = " + AnimationState);
            //playIdle();
            IsRunning = false;
            IsIdle = true;
            AnimationState = AnimationStates.Idle;
            _animator.Play("Idle");

            if (horizontal == 0 && vertical == 0 && jumpCooldown <= 0)
            {
                //if (!IsSlide) _rigidbody.velocity = Vector3.zero;
            }
        }

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

    private void movementSliding(bool forward)
    {
        if (!IsCanAct || !IsCanWalk || (IsPlatformTouched && !IsGrounded))
        {
            if (AnimationState != AnimationStates.Idle)
            {
                playIdle();
            }

            return;
        }

        if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(angleY) > 0)
        {
            angleYForMobile += angleY;
            
            if (Mathf.Abs(horizontal) > 0)
            {
                angleYForMobile += horizontal * 2f;
                //int sign = horizontal > 0 ? 1 : -1;
                //_rigidbody.AddRelativeForce(Vector3.right * sign * 100, ForceMode.Force);
            }

            if (Globals.IsMobile)
            {
                _rigidbody.DORotate(new Vector3(_transform.eulerAngles.x, angleYForMobile, _transform.eulerAngles.z), Time.deltaTime * 10);
            }
            else
            {
                _rigidbody.MoveRotation(Quaternion.Euler(new Vector3(_transform.eulerAngles.x, angleYForMobile, _transform.eulerAngles.z)));
            }
            
            //_rigidbody.MoveRotation(Quaternion.Euler(new Vector3(_transform.eulerAngles.x, angleYForMobile, _transform.eulerAngles.z)));
        }

        if (!IsFreeFall)
        {
            if (!IsFinished) _rigidbody.AddForce(_transform.forward * 160 + Vector3.down * 140, ForceMode.Force);
        }
        else
        {
            if (!IsFinished) _rigidbody.AddForce(_transform.forward * 160 + Vector3.down * 30, ForceMode.Force);
        }
               

        horizontal = 0;
        angleY = 0;
    }


    private void movement(bool forward)
    {        
        if (!IsCanAct || !IsCanWalk || (IsPlatformTouched && !IsGrounded))
        {            
            if (AnimationState != AnimationStates.Idle)
            {
                playIdle();
            }
                
            return;
        }
            

        if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0 || forward || Mathf.Abs(angleY) > 0)
        {
            float turnKoeff = PlayerCurrentSpeed * 0.03f;
                        
            if ((Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0))
            {                    
                if (Mathf.Abs(angleY) > 0)
                {
                    angleYForMobile += angleY;                    
                }

                float angle = Mathf.Atan2(horizontal, vertical) * 180 / Mathf.PI;

                if (Globals.IsMobile)
                {
                    _rigidbody.DORotate(new Vector3(_transform.eulerAngles.x, angleYForMobile + angle, _transform.eulerAngles.z), Time.deltaTime * 10);
                }
                else
                {
                    _rigidbody.MoveRotation(Quaternion.Euler(new Vector3(_transform.eulerAngles.x, angleYForMobile + angle, _transform.eulerAngles.z)));
                }

                //_rigidbody.DORotate(new Vector3(_transform.eulerAngles.x, angleYForMobile + angle, _transform.eulerAngles.z), 0);
                //_rigidbody.MoveRotation(Quaternion.Euler(new Vector3(_transform.eulerAngles.x, angleYForMobile + angle, _transform.eulerAngles.z)));
            }
            else if (horizontal == 0 && vertical == 0 && Mathf.Abs(angleY) > 0)
            {
                angleYForMobile += angleY;
                                

                if (Globals.IsMobile)
                {
                    _rigidbody.DORotate(new Vector3(_transform.eulerAngles.x, angleYForMobile, _transform.eulerAngles.z), Time.deltaTime * 10);
                }
                else
                {
                    _rigidbody.MoveRotation(Quaternion.Euler(new Vector3(_transform.eulerAngles.x, angleYForMobile, _transform.eulerAngles.z)));
                }
                
                //_rigidbody.DORotate(new Vector3(_transform.eulerAngles.x, angleYForMobile, _transform.eulerAngles.z), 0);                
                //_rigidbody.MoveRotation(Quaternion.Euler(new Vector3(_transform.eulerAngles.x, angleYForMobile, _transform.eulerAngles.z)));
            }

            //if (angleY != 0) print(angleY);
            angleY = 0;
                        

            if (forward)
            {
                forward = false;
                vertical = 1;
            }

            float addSpeedKoeff = 1f;

            if (Globals.IsMobile)
            {
                addSpeedKoeff += Globals.SPEED_INC_IN_NONGROUND_MOBILE;
            }
            else
            {
                addSpeedKoeff += Globals.SPEED_INC_IN_NONGROUND_PC;
            }


            if (!IsFreeFall)
            {
                if ((PlayerVelocity < PlayerCurrentSpeed && IsGrounded) || (PlayerVelocity < PlayerCurrentSpeed * addSpeedKoeff && !IsGrounded))
                {
                    float koeff = 0;
                    float addKoeff = IsGrounded ? 1 : addSpeedKoeff;

                    koeff = PlayerCurrentSpeed * addKoeff * new Vector2(horizontal, vertical).magnitude - PlayerVelocity;

                    koeff = koeff > 0 ? koeff : 0;

                    _rigidbody.velocity += _transform.forward * koeff;

                    if (koeff > 0) playRun();
                }
            }
            else
            {
                if ((PlayerNonVerticalVelocity < PlayerCurrentSpeed && IsGrounded) || (PlayerNonVerticalVelocity < PlayerCurrentSpeed * addSpeedKoeff && !IsGrounded))
                {
                    float koeff = 0;
                    float addKoeff = IsGrounded ? 1 : addSpeedKoeff;

                    koeff = PlayerCurrentSpeed * addKoeff * new Vector2(horizontal, vertical).magnitude - PlayerNonVerticalVelocity;

                    koeff = koeff > 0 ? koeff : 0;

                    _rigidbody.velocity += _transform.forward * koeff;

                    if (koeff > 0) playRun();
                }
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
        if (AnimationState == AnimationStates.Run) return;

        IsRunning = true;
        IsIdle = false;

        if (IsGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            _animator.Play("Run");
            AnimationState = AnimationStates.Run;
        }            
    }

    private void playIdle()
    {
        if (AnimationState == AnimationStates.Idle) return;

        IsRunning = false;
        IsIdle = true;

        if (IsGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            AnimationState = AnimationStates.Idle;
            _animator.Play("Idle");
        }
    }

    private void playAnimation()
    {
        if (IsSlide)
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("JumpLoop")) _animator.Play("JumpLoop");
            return;
        }

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

    
    private void OnCollisionEnter(Collision collision)
    {        
        if (collision != null && collision.collider.gameObject.layer == Globals.LAYER_DANGER)
        {            
            float i = collision.impulse.magnitude;
            Vector3 impulse = collision.impulse;
            //if (i < Globals.BASE_SPEED) return;
            

            ApplyForceType punchType = ApplyForceType.Punch_easy;
            float additionalForce = 1;

            if (collision.gameObject.TryGetComponent(out DangerZoneAdditionalData addData))
            {
                punchType = addData.punchType;
                additionalForce = addData.AdditionalForce;
            }

            if (i > Globals.MAX_HIT_IMPULSE_MAGNITUDE)
            {
                float dif = Globals.MAX_HIT_IMPULSE_MAGNITUDE / i;
                impulse *= dif;
                additionalForce = 1f;
            }/*
            else if (i < Globals.MIN_HIT_IMPULSE_MAGNITUDE)
            {
                //return;
            }*/
            else if (i < Globals.MIN_HIT_IMPULSE_MAGNITUDE)
            {
                if (i < 1) i = 1;
                float dif = Globals.MIN_HIT_IMPULSE_MAGNITUDE / i;
                impulse *= dif;
            }

            
            ApplyTrapForce(impulse, collision.GetContact(0).point, punchType, additionalForce);
        }

        if (collision != null && collision.collider.gameObject.layer == Globals.LAYER_PLAYER && collision.gameObject.TryGetComponent(out Rigidbody another))
        {
            if (another.TryGetComponent(out ConditionControl cc) && !cc.HasCondition(Conditions.frozen))
            {
                another.AddForce((another.transform.position - _transform.position).normalized * Globals.PLAYERS_COLLIDE_FORCE, ForceMode.Impulse);
            }

            int rnd = UnityEngine.Random.Range(0, 100);

            if (rnd > 75)
            {
                effectsControl.MakeSmallPunchSound();
            }
            
            _rigidbody.AddForce((_transform.position - another.transform.position).normalized * Globals.PLAYERS_COLLIDE_FORCE, ForceMode.Impulse);
        }

        if (collision != null && collision.collider.gameObject.layer == 0)
        {
            IsPlatformTouched = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.layer == 0)
        {
            IsPlatformTouched = false;
        }
    }

    public void SetRagdollState(bool isActive)
    {

        if (isActive)
        {
            _transform.SetParent(playerLocation);

            IsCanAct = false;
            IsJumping = false;
            IsFloating = false;
            IsSecondJump = false;
            IsFreeFall = false;
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
            ragdollColliders[i].transform.localPosition = ragdollPos[i];
            ragdollColliders[i].transform.localEulerAngles = ragdollRot[i];
        }

        yield return new WaitForSeconds(0.2f);
        
        collisionChecker.IsRagdollHasContact = false;
        _animator.enabled = true;
        AnimationState = AnimationStates.None;
       
        IsRagdollActive = false;
        IsCanAct = true;
        IsCanWalk = true;

        if (!IsItMainPlayer)
        {        
            if (bot == null) bot = GetComponent<BotAI>();
            bot.IsCanDoubleJump = true;
            bot.IsCanJump = true;
            bot.IsCanRun = true;
        }

        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < ragdollColliders.Length; i++)
        {
            ragdollColliders[i].transform.DOKill();
            ragdollColliders[i].transform.localPosition = ragdollPos[i];
            ragdollColliders[i].transform.localEulerAngles = ragdollRot[i];
        }
    }

    
    public void Respawn(Vector3 pos, Vector3 rot)
    {        
        if (gm.GameType == GameTypes.Dont_fall)
        {
            if (IsItMainPlayer)
            {
                IsDead = true;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                _rigidbody.useGravity = false;
                gm.AddPlayerFinished(this);
            }
            else
            {
                IsDead = true;
                gameObject.SetActive(false);
            }
            
        }
        else
        {
            StartCoroutine(playRespawn(pos, rot));
        }

        
    }
    private IEnumerator playRespawn(Vector3 pos, Vector3 rot)
    {
        IsDead = true;
        IsCanAct = false;
        isJump = false;
        IsJumping = false;
        IsFloating = false;
        IsSecondJump = false;
        IsFreeFall = false;
        abilityManager.HideAbility(false);
        _transform.SetParent(playerLocation);

        yield return new WaitForSeconds(0.2f);


        IsCanAct = true;
        IsCanJump = true;
        IsCanWalk = true;

        if (IsRagdollActive) SetRagdollState(false);
        _rigidbody.velocity = Vector3.zero;
        _animator.StopPlayback();
        if (IsItMainPlayer)
        {
            cc.ResetCameraOnRespawn(new Vector3(0, rot.y, 0));
            //angleYForMobile = 0;

            angleYForMobile = rot.y;
        }
        else
        {
            if (bot == null) bot = GetComponent<BotAI>();
            bot.IsCanDoubleJump = true;
            bot.IsCanJump = true;
            bot.IsCanRun = true;
        }

        ragdollRigidbodies[0].transform.localPosition = Vector3.zero;


        //playIdle();
        //AnimationState = AnimationStates.Idle;
        //_animator.Play("Idle");
        AnimationState = AnimationStates.None;

        _transform.position = pos;
        _transform.eulerAngles = new Vector3(0, rot.y, 0);
        
        effectsControl.PlayRespawnEffect();

        IsDead = false;
        IsSpeedChanged = false;
        PlayerCurrentSpeed = PlayerMaxSpeed;
    }

    public void ApplyTrapForce(Vector3 forceVector, Vector3 contactPoint, ApplyForceType punchType, float additionalForce)
    {
        if (IsRagdollActive || !IsCanAct) return;
        if (additionalForce == 0) additionalForce = 1;
                
        if (forceVector.magnitude > 30)
        {
            effectsControl.MakeFunnySound(80);
        }

        effectsControl.PlayPunchEffect(punchType, contactPoint);

        SetRagdollState(true);

        //ragdollRigidbodies[0].AddRelativeTorque(new Vector3(0, 1, 0.5f) * 1000f, ForceMode.Impulse);

        for (int i = 0; i < ragdollRigidbodies.Length; i++)
        {            
            ragdollRigidbodies[i].velocity = forceVector * additionalForce;
            //ragdollRigidbodies[i].AddTorque(new Vector3(0, 1, 0.5f) * 1000f, ForceMode.Impulse);
        }
        
        if (punchType == ApplyForceType.Punch_large)
        {        
            if (IsItMainPlayer) gm.ShakeScreen(0.3f, 2, 30);
            StartCoroutine(addAdditionalForceWhenLargePunch(forceVector));
        }

        StartCoroutine(playTurnOffRagdoll());
    }

    private IEnumerator playTurnOffRagdoll()
    {        
        float timer = 0;
        while (ragdollRigidbodies[0].velocity.magnitude > Globals.BASE_SPEED && !IsDead && !IsGrounded)
        {
            timer += 0.1f;
            yield return ZeroOne;

        }

        while(!IsGrounded && !IsDead)
        {
            timer += 0.1f;
            yield return ZeroOne;
            if (timer > 1) break;
        }

        

        if (timer < 1 && !IsDead)
        {
            yield return new WaitForSeconds(1 - timer);
        }

        //yield return new WaitForSeconds(0.5f);
        SetRagdollState(false);
    }
    
    private IEnumerator addAdditionalForceWhenLargePunch(Vector3 addForce)
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime);

        for (int j = 0; j < 3; j++)
        {            
            for (int i = 0; i < ragdollRigidbodies.Length; i++)
            {
                ragdollRigidbodies[i].AddForce(addForce * 5, ForceMode.Force);
            }
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }        
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

public enum AnimationStates
{
    None,
    Idle,
    Run,
    Fly
}
