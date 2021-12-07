using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    private bool lockMouse = true;
    public ParticleSystem dust;

    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private float horizontal;

    [Header("Ground")]
    [SerializeField] private bool isGrounded;
    public Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    public LayerMask whatIsGround;

    [Header("Air")]
    private float primaryJumpForce;
    private float secondaryJumpForce;

    private int prevJumpDirection = 1;
    private bool canTilt = false;
    public bool isFalling;
    private bool doubleJumpKeyHeld;

    [SerializeField] private bool canDoubleJump = true;
    [SerializeField] private bool doubleJumping;
    public PlayerJumpStats normalJump;
    public PlayerJumpStats boostedJump;
    public PlayerJumpStats pimpJump;

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
        primaryJumpForce = normalJump.primaryJump;
        secondaryJumpForce = normalJump.secondaryJump;
    }

    private void Update()
    {
        HandleInput();
        HandleGrounded();
        HandleJumping();
        HandleMovement();
        // HandleRotation();
    }

    private void HandleJumping()
    {
        HandlePrimaryJumping();
        HandleSecondaryJumping();
    }

    private void LateUpdate()
    {
        if (doubleJumping)
        {
            doubleJumping = false;
            PlaySecondaryJumpSpin();
            PlayDust();  
        } 
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
            primaryJumpForce = boostedJump.primaryJump;
            secondaryJumpForce = boostedJump.secondaryJump;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            primaryJumpForce = normalJump.primaryJump;
            secondaryJumpForce = normalJump.secondaryJump;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            primaryJumpForce = pimpJump.primaryJump;
            secondaryJumpForce = pimpJump.secondaryJump;
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

        if (isGrounded)
        {
            canDoubleJump = true;
        }
    }

    private void HandlePrimaryJumping()
    {
        // auto jump if grounded and falling down
        if (isGrounded && isFalling)
        {
            // reset y-velocity to avoid inconsistencies before applying jump force
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * primaryJumpForce, ForceMode.Impulse);
            PlayJumpSpin();
            PlayDust();
        }
    }

    private void HandleSecondaryJumping()
    {
        if (!isGrounded && canDoubleJump && doubleJumpKeyHeld)
        {
            canDoubleJump = false;
            doubleJumping = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * secondaryJumpForce, ForceMode.Impulse);
        }

        // reset key flag every function call
        doubleJumpKeyHeld = false;
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
