using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class Platform : MonoBehaviour
{

    private Rigidbody rb;
    private Vector3 velocity;
    private float horMoveSpeed = 0f;
    private float verMoveSpeed = 0f;

    private bool looping;
    private float loopTimer;
    private bool canMove;

    [SerializeField] private PlatformStats stats;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        InitializePlatform();
    }

    private void InitializePlatform()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        horMoveSpeed = stats.XMoveSpeed;
        verMoveSpeed = stats.YMoveSpeed;
        transform.localScale = new Vector3(stats.X, stats.Y, stats.Z);

        // loopDuration = stats.LoopDuration;
        // waitDuration = stats.WaitDuration;
        // stats.ForceBoost = stats.ForceBoost;
        // stats.Duration = stats.Duration;
        // stats.Ease = stats.Ease;
        // stats.YOffset = stats.YOffset;
    }

    void Start() 
    {
        RandomizeHorizontalMovementDirection();
    }

    private void Update() 
    {
        if (looping) LoopMovement();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            transform.position += new Vector3(horMoveSpeed, verMoveSpeed, 0f) * Time.deltaTime;
        }
    }

    private void LoopMovement()
    {
        if (loopTimer < 0)
        {
            canMove = false;
            StartCoroutine(ShiftMoveDirectionWithWaitDelay());
            loopTimer = stats.LoopDuration + stats.WaitDuration;
        }
        loopTimer -= Time.deltaTime;
    }

    IEnumerator ShiftMoveDirectionWithWaitDelay()
    {
        yield return new WaitForSeconds(stats.WaitDuration);
        ShiftAllMoveDirection();
        canMove = true;
    }

    private void ShiftAllMoveDirection()
    {
        horMoveSpeed *= -1;
        verMoveSpeed *= -1;
    }
    
    private void RandomizeHorizontalMovementDirection()
    {
        int randomDirection = UnityEngine.Random.Range(-1, 1);
        horMoveSpeed *= randomDirection;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Deadzone"))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Player Ground Check") )
        {
            PlayerController controller = other.GetComponentInParent<PlayerController>();
            if (controller.isFalling)
            {
                Rigidbody rigid = other.GetComponentInParent<Rigidbody>();
                rigid.AddForce(Vector3.up * stats.ForceBoost, ForceMode.Impulse);
                PlaySteppedAnim();
            }
        }
    }

    private void PlaySteppedAnim()
    {
        transform
            .DOMoveY(stats.YOffset, stats.Duration)
            .From(true)
            .SetEase(stats.Ease).Play();
    }
}
