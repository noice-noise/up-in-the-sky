using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public Transform body;
    private Rigidbody rb;
    private bool lockMouse = true;
    public ParticleSystem dust;
    public Vector3 rbVelocity;

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
    [SerializeField] private bool allowTilting = true;
    public bool isFalling;
    private bool doubleJumpKeyHeld;

    [SerializeField] private bool canDoubleJump = true;
    [SerializeField] private bool doubleJumping;

    public PlayerJumpStats baseJump;
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
        primaryJumpForce = baseJump.primaryJump;
        secondaryJumpForce = baseJump.secondaryJump;
    }

    private void Update()
    {
        HandleInput();
        HandleGrounded();
        HandleJumping();
        HandleMovement();
        rbVelocity = rb.velocity;
        HandleRotation();
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
        // Transform body;
        if (allowTilting)
        {
            float zRotation = body.eulerAngles.y;
            euler = body.eulerAngles;
            zRotation += horizontal * 20f;
            body.rotation = Quaternion.Slerp(
                body.rotation, 
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
            primaryJumpForce = baseJump.primaryJump + boostedJump.primaryJump;
            secondaryJumpForce = baseJump.secondaryJump +  boostedJump.secondaryJump;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            primaryJumpForce = baseJump.primaryJump + normalJump.primaryJump;
            secondaryJumpForce = baseJump.secondaryJump +  normalJump.secondaryJump;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            primaryJumpForce = baseJump.primaryJump + pimpJump.primaryJump;
            secondaryJumpForce = baseJump.secondaryJump + pimpJump.secondaryJump;
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
        allowTilting = false;
        Vector3 rot = new Vector3(0, 360, 0);

        float direction = horizontal == 0 ? prevJumpDirection : horizontal;
        prevJumpDirection = (int) direction;

        body
        .DORotate(rot * direction, duration, RotateMode.FastBeyond360)
        .SetEase(ease)
        .SetRelative(true)
        .OnComplete(allowTilt);
    }

    private void PlaySecondaryJumpSpin()
    {
        allowTilting = false;
        Vector3 rot = new Vector3(0, 0, 360);

        // either 1 or -1
        float direction = horizontal == 0 ? prevJumpDirection : horizontal;
        prevJumpDirection = (int) direction;

        body
        .DORotate(rot * direction, duration + 0.2f, RotateMode.FastBeyond360)
        .SetEase(ease)
        .SetRelative(true)
        .OnComplete(allowTilt);
    }

    private void allowTilt()
    {
        allowTilting = true;
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
