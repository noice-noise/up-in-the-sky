using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class Platform : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    private Vector3 velocity;
    [SerializeField] private float horMoveSpeed = 0f;
    [SerializeField] private float verMoveSpeed = 0f;

    [Header("Looping")]
    public bool looping;

    // using timer as a way to track when to shift
    [SerializeField] private float loopDuration = 5f;
    private float loopTimer;
    [SerializeField] private float waitDuration = 2f;

    [Header("Attribute")]
    [SerializeField] private float forceBoost = 0f;
    private bool canMove;

    [Header("Animation")]
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private Ease ease = Ease.InOutBounce;
    [SerializeField] private float yOffset = 0.2f;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
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
            loopTimer = loopDuration + waitDuration;
        }
        loopTimer -= Time.deltaTime;
    }

    IEnumerator ShiftMoveDirectionWithWaitDelay()
    {
        yield return new WaitForSeconds(waitDuration);
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
                rigid.AddForce(Vector3.up * forceBoost, ForceMode.Impulse);
                PlaySteppedAnim();
            }
        }
    }

    private void PlaySteppedAnim()
    {
        transform
            .DOMoveY(yOffset, duration)
            .From(true)
            .SetEase(ease).Play();
    }
}
