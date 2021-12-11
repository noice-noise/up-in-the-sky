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

    private bool canLoopMovement;
    private float loopTimer;
    private bool canMove;

    public int initialDirection;
    public bool ignoreDeadzone;

    [SerializeField] private PlatformType type;
    private PlatformStats stats;


    // public int InitialDirection { get => initialDirection; set => initialDirection = value; }
    internal PlatformType Type { get => type; set => type = value; }

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        InitializePlatform();
    }

    private void InitializePlatform()
    {
        InitType();
        InitStats();
    }

    private void InitType()
    {
        stats = LevelGenerator.Instance.GetPlatformStats(Type);
    }

    private void InitStats()
    {
        Debug.Log("init:" + initialDirection);
        horMoveSpeed = stats.XMoveSpeed * initialDirection;
        verMoveSpeed = stats.YMoveSpeed;
        transform.localScale = new Vector3(stats.X, stats.Y, stats.Z);
        canLoopMovement = stats.CanLoopMovement;

        // loopDuration = stats.LoopDuration;
        // waitDuration = stats.WaitDuration;
        // stats.ForceBoost = stats.ForceBoost;
        // stats.Duration = stats.Duration;
        // stats.Ease = stats.Ease;
        // stats.YOffset = stats.YOffset;
    }

    void Start() 
    {
        // RandomizeHorizontalMovementDirection();
    }

    private void Update() 
    {
        if (canLoopMovement) LoopMovement();
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
        int randomDirection = 0;

        do
        {
            randomDirection = UnityEngine.Random.Range(-1, 2);
            Debug.Log(":" + randomDirection);
        } while (randomDirection == 0);

        horMoveSpeed *= randomDirection;
        Debug.Log(randomDirection);
    }

    // TODO delegate destroying to deadzone object
    private void OnTriggerEnter(Collider other) 
    {
        HandleDestroy(other);
        HandleTrigger(other);
    }

    private void HandleTrigger(Collider other)
    {
        if (other.gameObject.CompareTag("Player Ground Check") )
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();
            if (player.isFalling)
            {
                HandlePlatformTrigger(other);
            }
        }
    }

    private void HandleDestroy(Collider actor)
    {
        bool onDeadzoneLayer = actor.gameObject.layer == LayerMask.NameToLayer("Deadzone");
        if (!ignoreDeadzone && onDeadzoneLayer)
        {
            Destroy(gameObject);
        }
    }

    private void HandlePlatformTrigger(Collider actor)
    {
        if (type == PlatformType.Fragile)
        {
            Debug.Log("Fragile");
            Destroy(gameObject, 0.5f);
        }
        else
        {
            Rigidbody rigid = actor.GetComponentInParent<Rigidbody>();
            rigid.AddForce(Vector3.up * stats.ForceBoost, ForceMode.Impulse);
            PlaySteppedAnim();
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
