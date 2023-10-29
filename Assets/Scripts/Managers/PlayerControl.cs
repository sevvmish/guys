using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    [Header("Controls")]    
    public Animator _animator;
    public bool IsItMainPlayer;
    
    public EffectsControl effectsControl;

    [Header("Ragdoll")]
    public CapsuleCollider[] ragdollColliders;
    private Rigidbody[] ragdollRigidbodies;
    private bool isRagdollActive;

    public Transform CurrentActivePlatform { get; private set; }
    public Transform DangerZone { get; private set; }

    //INPUT
    public void SetHorizontal(float hor) => horizontal = hor;
    public void SetVertical(float ver) => vertical = ver;
    public void SetRotationAngle(float ang) => angleY = ang;
    public void SetJump() => isJump = true;
    public void SetForward() => isForward = true;
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;
    [SerializeField] private float angleY;
    [SerializeField] private float angleYForMobile;
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
    public bool IsJumping { get; private set; }
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
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.mass = Globals.MASS;
        _rigidbody.drag = Globals.DRAG;
        _rigidbody.angularDrag = Globals.ANGULAR_DRAG;

        _transform = GetComponent<Transform>();
        mainCollider = GetComponent<CapsuleCollider>();
        PlayerMaxSpeed = 5;
        PlayerCurrentSpeed = PlayerMaxSpeed;
        IsCanAct = true;

        ragdollRigidbodies = new Rigidbody[ragdollColliders.Length];
        for (int i = 0; i < ragdollColliders.Length; i++)
        {
            ragdollColliders[i].enabled = false;
            ragdollRigidbodies[i] = ragdollColliders[i].GetComponent<Rigidbody>();
            ragdollRigidbodies[i].useGravity = false;
        }
        isRagdollActive = false;
    }

    private void Update()
    {
        if (jumpCooldown > 0) jumpCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q) && IsItMainPlayer)
        {
            SetRagdollState(true);
            ragdollRigidbodies[0].AddForce((Vector3.forward + Vector3.right + Vector3.up * 0.5f) * 20, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.E) && IsItMainPlayer)
        {
            SetRagdollState(false);
        }

        if (!CurrentActivePlatform || !CurrentActivePlatform.CompareTag("Platform")) return;
        checkPlatforms();
    }

    // Update is called once per frame
    void FixedUpdate()
    {        
        IsGrounded = checkGround();
        checkShadow();
        PlayerVelocity = _rigidbody.velocity.magnitude;
        PlayerNonVerticalVelocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z).magnitude;
        PlayerVerticalVelocity = new Vector3(0, _rigidbody.velocity.y, 0).magnitude;
        //if (PlayerVelocity > 0.1f && IsItMainPlayer) print(PlayerVelocity);


        if (!IsGrounded)
        {
            howLongNonGrounded += Time.deltaTime;
            GravityScale();
        }
        else
        {
            howLongNonGrounded = 0;
            IsJumping = false;
            if (_rigidbody.drag < Globals.DRAG) _rigidbody.drag = Globals.DRAG;
        }
             
        if (isForward)
        {
            isForward = false;
            movement(true);
        }
        else
        {
            movement(false);
        }

        MakeJump();
        playAnimation();
    }

    public void MakeJump()
    {
        if (isJump)
        {
            isJump = false;
            if (IsGrounded && jumpCooldown <= 0 && IsCanAct)
            {
                _rigidbody.AddForce(Vector3.up * Globals.JUMP_POWER, ForceMode.Impulse);
                IsJumping = true;
                jumpCooldown = 0.3f;
            }
        }
    }

    private bool checkGround()
    {
        return Physics.CheckBox(_transform.position + Vector3.down * 0.2f, new Vector3(0.25f, 0.05f, 0.25f), Quaternion.identity);
    }

    private void checkShadow()
    {
        effectsControl.SetShadow(IsGrounded);
    }
        
    private void movement(bool forward)
    {
        if (!IsCanAct) return;

        if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0 || forward || Mathf.Abs(angleY) > 0)
        {
            if (Globals.IsMobile)
            {
                if ((Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0))
                {
                    /*
                    if (Mathf.Abs(angleY) > 0)
                    {
                        angleYForMobile += angleY;
                        _transform.eulerAngles = new Vector3(_transform.eulerAngles.x, angleYForMobile, _transform.eulerAngles.z);

                        cc.ChangeCameraAngleY(angleYForMobile);
                    }
                    else
                    {
                        float angle = Mathf.Atan2(horizontal, vertical) * 180 / Mathf.PI;                        
                        _transform.eulerAngles = new Vector3(_transform.eulerAngles.x, angleYForMobile + angle, _transform.eulerAngles.z);
                    }*/

                    if (Mathf.Abs(angleY) > 0)
                    {
                        angleYForMobile += angleY;
                        cc.ChangeCameraAngleY(angleYForMobile);
                    }

                    float angle = Mathf.Atan2(horizontal, vertical) * 180 / Mathf.PI;
                    _transform.eulerAngles = new Vector3(_transform.eulerAngles.x, angleYForMobile + angle, _transform.eulerAngles.z);

                }
                else if (horizontal == 0 && vertical == 0 && Mathf.Abs(angleY) > 0)
                {

                    angleYForMobile += angleY;
                    _transform.eulerAngles = new Vector3(_transform.eulerAngles.x, angleYForMobile, _transform.eulerAngles.z);
                    cc.ChangeCameraAngleY(angleYForMobile);
                }
            }            
            else if (!Globals.IsMobile && Mathf.Abs(angleY) > 0)
            {
                _transform.eulerAngles = new Vector3(_transform.eulerAngles.x, _transform.eulerAngles.y + angleY, _transform.eulerAngles.z);
                cc.ChangeCameraAngleY(_transform.eulerAngles.y);
            }

            angleY = 0;


            if (PlayerNonVerticalVelocity < PlayerCurrentSpeed)
            {
                float koeff = 0;

                if (forward)
                {
                    koeff = PlayerCurrentSpeed * new Vector2(1, 1).magnitude - PlayerNonVerticalVelocity;
                }
                else
                {
                    koeff = PlayerCurrentSpeed * new Vector2(horizontal, vertical).magnitude - PlayerNonVerticalVelocity;
                }
                                
                koeff = koeff > 0 ? koeff : 0;       
                
                if (Globals.IsMobile)
                {
                    _rigidbody.velocity += _transform.forward * koeff;
                }
                else
                {
                    if (vertical > 0 || forward)
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

            if (!Globals.IsMobile)
            {
                //_rigidbody.AddRelativeForce(_transform.right * horizontal, ForceMode.Impulse);

                if (horizontal > 0)
                {
                    //_rigidbody.AddRelativeForce(_transform.right * 15f, ForceMode.Force)
                    //_rigidbody.DOMove(_transform.position + _transform.right * 0.15f, Time.fixedDeltaTime*3);
                }
                else if (horizontal < 0)
                {
                    //_rigidbody.DOMove(_transform.position - _transform.right * 0.15f, Time.fixedDeltaTime*3);
                }
            }
        }
        else
        {
            if (howLongMoving < 1 && IsGrounded)
            {
                howLongMoving++;
                _rigidbody.velocity = Vector3.zero;
            }

            playIdle();
        }
    }

    private void playRun()
    {
        if (IsGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) _animator.Play("Run");
    }

    private void playIdle()
    {
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

    private void GravityScale()
    {
        float fallingKoeff = 1;
        if (_rigidbody.drag > 0) _rigidbody.drag = 0;

        if (_rigidbody.velocity.y >= 0)
        {
            _rigidbody.AddForce(Physics.gravity * Globals.GRAVITY_KOEFF * _rigidbody.mass);
        }
        else if (_rigidbody.velocity.y < 0)
        {
            if (fallingKoeff < 5) fallingKoeff *= 1.2f;
            _rigidbody.AddForce(Physics.gravity * _rigidbody.mass * Globals.GRAVITY_KOEFF * fallingKoeff);
        }
    }

    private void checkPlatforms()
    {        
        if (CurrentActivePlatform != null)
        {
            if (_transform.parent != CurrentActivePlatform)
            {
                _transform.SetParent(CurrentActivePlatform);
            }
            /*
            Vector3 delta = newPlatformPoint - lastPlatformPoint;
            if (delta.magnitude > 0)
            {
                _transform.position += delta;                
                lastPlatformPoint = newPlatformPoint;
            }*/
        }
        else
        {
            if (_transform.parent != null)
            {
                _transform.SetParent(null);
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.collider.CompareTag("Platform"))
        {
            if (CurrentActivePlatform != collision.collider.transform)
            {
                CurrentActivePlatform = collision.collider.transform;
                _transform.SetParent(CurrentActivePlatform);
            }            
        }        
        else if (collision != null && collision.collider.CompareTag("Danger"))
        {
            if (DangerZone != collision.collider.transform)
            {
                DangerZone = collision.collider.transform;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.transform == CurrentActivePlatform)
        {
            CurrentActivePlatform = null;
            _transform.SetParent(null);
        }
        else if (collision.collider.transform == DangerZone)
        {
            DangerZone = null;
        }
    }

    private void SetRagdollState(bool isActive)
    {
        if (isActive)
        {
            IsCanAct = false;
            _rigidbody.velocity = Vector3.zero;
            _animator.enabled = false;
            for (int i = 0; i < ragdollColliders.Length; i++)
            {
                ragdollColliders[i].enabled = true;
                ragdollRigidbodies[i].useGravity = true;
            }
            if (IsItMainPlayer) gm.GetCameraControl().SwapControlBody(ragdollRigidbodies[0].transform);
        }
        else
        {
            for (int i = 0; i < ragdollColliders.Length; i++)
            {
                ragdollColliders[i].enabled = false;
                ragdollRigidbodies[i].useGravity = false;
            }
            _transform.position = ragdollRigidbodies[0].transform.position;
            _animator.enabled = true;
            if (IsItMainPlayer) gm.GetCameraControl().SwapControlBody(_transform);
            IsCanAct = true;
        }

        isRagdollActive = isActive;
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
        yield return new WaitForSeconds(0.2f);
        SetRagdollState(false);
        _rigidbody.velocity = Vector3.zero;

        _transform.position = pos;
        _transform.eulerAngles = rot;

        IsDead = false;
        IsCanAct = true;
        PlayerCurrentSpeed = PlayerMaxSpeed;
    }

}
