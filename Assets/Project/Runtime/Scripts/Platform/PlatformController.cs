using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class PlatformController : MonoBehaviour
{

    private Rigidbody rb;
    private Vector3 velocity;
    private float horMoveSpeed = 0f;
    private float verMoveSpeed = 0f;

    private bool canLoopMovement;
    private float loopTimer;
    private bool canMove;

    private int initialDirection;
    [SerializeField] private bool ignoreDeadzone;

    [SerializeField] private PlatformType type;
    private PlatformStats stats;


    public int InitialDirection { get => initialDirection; set => initialDirection = value; }
    public bool IgnoreDeadzone { get => ignoreDeadzone; set => ignoreDeadzone = value; }
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
        stats = LevelManager.Instance.LevelDataContainer.GetPlatformStats(type);
    }

    private void InitStats()
    {
        horMoveSpeed = stats.XMoveSpeed * initialDirection;
        verMoveSpeed = stats.YMoveSpeed;
        transform.localScale = new Vector3(stats.X, stats.Y, stats.Z);
        canLoopMovement = stats.CanLoopMovement;
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
        if (!IgnoreDeadzone && onDeadzoneLayer)
        {
            Destroy(gameObject);
        }
    }

    private void HandlePlatformTrigger(Collider other)
    {
        if (type == PlatformType.Fragile)
        {
            Destroy(gameObject, 0.5f);
        }
        else
        {
            Rigidbody rigid = other.GetComponentInParent<Rigidbody>();
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
