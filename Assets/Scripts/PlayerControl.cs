using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Controls")]
    public float Horizontal;
    public float Vertical;
    public bool IsJump;
    public Animator _animator;

    [Header("Ragdoll")]
    public CapsuleCollider[] ragdollColliders;
    private Rigidbody[] ragdollRigidbodies;

    public float PlayerMaxSpeed { get; private set; }
    public float PlayerCurrentSpeed { get; private set; }
    public float PlayerVelocity { get; private set; }
    public float PlayerNonVerticalVelocity { get; private set; }
    public float PlayerVerticalVelocity { get; private set; }

    public bool IsGrounded { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsCanAct { get; private set; }

    private Rigidbody _rigidbody;
    private Transform _transform;
    private readonly float rotationKoeff = 0.05f;

    private float jumpCooldown;
    private float howLongNonGrounded;
    private float howLongMoving;

    

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        PlayerCurrentSpeed = 5;
        IsCanAct = true;

        ragdollRigidbodies = new Rigidbody[ragdollColliders.Length];
        for (int i = 0; i < ragdollColliders.Length; i++)
        {
            ragdollColliders[i].enabled = false;
            ragdollRigidbodies[i] = ragdollColliders[i].GetComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        if (jumpCooldown > 0) jumpCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            IsCanAct = false;
            _rigidbody.velocity = Vector3.zero;
            _animator.enabled = false;
            for (int i = 0; i < ragdollColliders.Length; i++)
            {
                ragdollColliders[i].enabled = true;
                //ragdollRigidbodies[i].velocity = Vector3.zero;
            }
            GameManager.Instance.GetCameraControl().SwapControlBody(ragdollRigidbodies[0].transform);
            ragdollRigidbodies[0].AddForce((Vector3.forward + Vector3.right + Vector3.up * 0.5f) * 20, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {                       
            for (int i = 0; i < ragdollColliders.Length; i++)
            {
                ragdollColliders[i].enabled = false;
                //ragdollRigidbodies[i].velocity = Vector3.zero;
            }
            _transform.position = ragdollRigidbodies[0].transform.position;
            _animator.enabled = true;
            GameManager.Instance.GetCameraControl().SwapControlBody(_transform);
            IsCanAct = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {        
        IsGrounded = checkGround();

        PlayerVelocity = _rigidbody.velocity.magnitude;
        PlayerNonVerticalVelocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z).magnitude;
        PlayerVerticalVelocity = new Vector3(0, _rigidbody.velocity.y, 0).magnitude;

        if (!IsGrounded)
        {
            howLongNonGrounded += Time.deltaTime;
            GravityScale();
        }
        else
        {
            howLongNonGrounded = 0;
            IsJumping = false;
            //if (_rigidbody.drag < 1) _rigidbody.drag = 1;
        }

        movement();
        makeJump();
        playAnimation();
    }

    private void makeJump()
    {
        if (IsJump && IsCanAct)
        {
            IsJump = false;
            if (IsGrounded && jumpCooldown <= 0)
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

    private void movement()
    {
        if (!IsCanAct) return;

        if (Mathf.Abs(Horizontal) > 0 || Mathf.Abs(Vertical) > 0)
        {
            float angle = Mathf.Atan2(Horizontal, Vertical) * 180 / Mathf.PI;
            _rigidbody.DORotate(new Vector3(0f, angle, 0f), rotationKoeff);

            if (PlayerNonVerticalVelocity < PlayerCurrentSpeed)
            {
                float koeff = PlayerCurrentSpeed * new Vector2(Horizontal, Vertical).magnitude - PlayerNonVerticalVelocity;
                if (!IsGrounded)
                {
                    koeff *= 0.5f;
                }

                koeff = koeff > 0 ? koeff : 0;
                
                _rigidbody.velocity += _transform.forward * koeff;                
            }
            howLongMoving = 0;
        }
        else
        {
            if (howLongMoving < 2 && IsGrounded)
            {
                howLongMoving++;
                _rigidbody.velocity = Vector3.zero;
            }
        }
    }

    private void playAnimation()
    {
        if (IsGrounded)
        {
            if (PlayerNonVerticalVelocity > 0.01f)
            {
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) _animator.Play("Run");
            }
            else
            {
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    _animator.StopPlayback();
                    _animator.Play("Idle");
                }                    
            }
        }
        else
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
        //if (_rigidbody.drag > 0) _rigidbody.drag = 0;

        if (_rigidbody.velocity.y >= 0)
        {
            _rigidbody.AddForce(Physics.gravity * Globals.GRAVITY_KOEFF * _rigidbody.mass);
        }
        else if (_rigidbody.velocity.y < 0)
        {
            if (fallingKoeff < 4000) fallingKoeff *= 1.2f;
            _rigidbody.AddForce(Physics.gravity * _rigidbody.mass * Globals.GRAVITY_KOEFF * fallingKoeff);
        }
    }
}
