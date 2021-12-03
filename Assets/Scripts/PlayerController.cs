using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    [SerializeField] private bool lockMouse = false;
    public ParticleSystem dust;

    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private float horizontal;

    [Header("Ground")]
    [SerializeField] private bool isGrounded;
    public Transform groundCheck;
    [SerializeField] private float groundDistance = 0.3f;
    public LayerMask whatIsGround;

    [Header("Air")]
    [SerializeField] private float jumpForce = 23f;
    [SerializeField] private float secJumpForce = 13f;
    private int prevJumpDirection = 1;
    [SerializeField] private bool canTilt;
    public bool isFalling;
    private bool doubleJumpKeyHeld;
    public PlayerStats normalJump;
    public PlayerStats boostedJump;
    public PlayerStats pimpJump;

    [Header("Movement")]
    [SerializeField] private float maxXVelocity = 10f;
    [SerializeField] private float maxYVelocity = 20f;
    [SerializeField] private float controlForce = 60f;
    public Vector3 euler;

    [Header("Animation")]
    public Ease ease;
    public float duration;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() 
    {
        FreezeRotation();
        LockCursor();
    }

    private void FixedUpdate() 
    {
        HandleInput();
        HandleGrounded();
        HandleMovement();
        HandleRotation();
    }

    private void HandleRotation()
    {
        if (canTilt)
        {
            float zRotation = transform.eulerAngles.y;
            euler = transform.eulerAngles;
            zRotation += horizontal * 20f;
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                Quaternion.Euler(180f, rb.rotation.y, zRotation), 
                5f * Time.deltaTime);
        }
    }

    private void HandleInput()
    {
        if (Input.GetAxisRaw("Mouse X") < 0)
        {
            horizontal = -1;
        }
        else if (Input.GetAxisRaw("Mouse X") > 0) 
        {
            horizontal = 1;
        } 
        else 
        {
            horizontal = 0;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            doubleJumpKeyHeld = true;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            jumpForce = boostedJump.primaryJump;
            secJumpForce = boostedJump.secondaryJump;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            jumpForce = normalJump.primaryJump;
            secJumpForce = normalJump.secondaryJump;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            jumpForce = pimpJump.primaryJump;
            secJumpForce = pimpJump.secondaryJump;
        }
    }

    private void HandleMovement()
    {
        ClampVelocity();
        moveDirection = new Vector3(horizontal, 0f, 0f);
        rb.AddForce(moveDirection * controlForce, ForceMode.Acceleration);
    }

    private void HandleGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);
        isFalling = Falling();

        // auto jump if grounded and falling down
        if (isGrounded && isFalling)
        {
            // reset y-velocity to avoid inconsistencies before applying jump force
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            PlayJumpSpin();
            PlayDust();
        }

        // double jump
        if (!isGrounded && doubleJumpKeyHeld)
        {
            doubleJumpKeyHeld = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * secJumpForce, ForceMode.Impulse);
            PlaySecondaryJumpSpin();
            PlayDust();
        }
    }

    private void PlayJumpSpin()
    {
        canTilt = false;
        Vector3 rot = new Vector3(0, 360, 0);

        float direction = horizontal == 0 ? prevJumpDirection : horizontal;
        prevJumpDirection = (int) direction;

        rb
        .DORotate(rot * direction, duration, RotateMode.FastBeyond360)
        .SetEase(ease)
        .SetRelative(true)
        .OnComplete(allowTilt);
    }

    private void PlaySecondaryJumpSpin()
    {
        canTilt = false;
        Vector3 rot = new Vector3(0, 0, 360);

        // either 1 or -1
        float direction = horizontal == 0 ? prevJumpDirection : horizontal;
        prevJumpDirection = (int) direction;

        rb
        .DORotate(rot * direction, duration + 0.2f, RotateMode.FastBeyond360)
        .SetEase(ease)
        .SetRelative(true)
        .OnComplete(allowTilt);
    }

    private void allowTilt()
    {
        canTilt = true;
    }

    public void PlayDust()
    {
        dust.Play();
    }

    private bool Falling()
    {
        return rb.velocity.y < 0.1;
    }

    private void ClampVelocity()
    {
        // clamp the x-velocity
        if (rb.velocity.x > maxXVelocity)
        {
            rb.velocity = new Vector3(maxXVelocity, rb.velocity.y, rb.velocity.z);
        } 
        else if (rb.velocity.x < -maxXVelocity)
        {
            rb.velocity = new Vector3(-maxXVelocity, rb.velocity.y, rb.velocity.z);
        }

        // clamp the y-velocity
        if (rb.velocity.y > maxYVelocity)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxYVelocity, rb.velocity.z);
        } 
        else if (rb.velocity.y < -maxYVelocity)
        {
            rb.velocity = new Vector3(rb.velocity.x, -maxYVelocity, rb.velocity.z);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }

    private void LockCursor()
    {
        if (lockMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void FreezeRotation()
    {
        rb.freezeRotation = true;
    }
}
