using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "PlatformStats_Type", menuName = "Platform/Platform Stats", order = 0)]
public class PlatformStats : ScriptableObject
{
    [Header("Size")]
    [SerializeField] private float x = 1f;
    [SerializeField] private float y = 1f;
    [SerializeField] private float z = 1f;

    [Header("Attribute")]
    [SerializeField] private float xMoveSpeed;
    [SerializeField] private float yMoveSpeed;
    [SerializeField] private float forceBoost;

    [Header("Looping")]
    [SerializeField] private float loopDuration;
    [SerializeField] private float waitDuration;

    [Header("Animation")]
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;

    public float XMoveSpeed { get => xMoveSpeed; }
    public float YMoveSpeed { get => yMoveSpeed; }
    public float ForceBoost { get => forceBoost; }
    public float LoopDuration { get => loopDuration; }
    public float WaitDuration { get => waitDuration; }
    public float Duration { get => duration; }
    public Ease Ease { get => ease; }
    public float XOffset { get => xOffset; }
    public float YOffset { get => yOffset; }
    public float X { get => x; }
    public float Y { get => y; }
    public float Z { get => z; }
}