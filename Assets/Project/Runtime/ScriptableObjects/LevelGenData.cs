using UnityEngine;

[CreateAssetMenu(fileName = "LevelGenData", menuName = "UpInTheSky/LevelGenData", order = 0)]
public class LevelGenData : ScriptableObject
{
    [SerializeField] private float triggerMoveDistance;
    [SerializeField] private float platformsCount;
    [SerializeField] private float yOffset;
    [SerializeField] private float xOffset;
    [SerializeField] private GameObject[] platforms; 

    public float TriggerMoveDistance { get => triggerMoveDistance; }
    public float PlatformsCount { get => platformsCount; }
    public float YOffset { get => yOffset; }
    public float XOffset { get => xOffset; }
    public GameObject[] Platforms { get => platforms; set => platforms = value; }
}