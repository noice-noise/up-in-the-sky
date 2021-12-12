using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "LevelDataContainer", menuName = "UpInTheSky/LevelDataContainer", order = 0)]
public class LevelDataContainer : ScriptableSingleton<LevelDataContainer>
{
    // TODO refactor these into extendable scriptable object enums
    [SerializeField] private PlatformStats stillPlatfromStats;
    [SerializeField] private PlatformStats movingPlatfromStats;
    [SerializeField] private PlatformStats boosterPlatformStats;
    [SerializeField] private PlatformStats fragilePlatformStats;

    public PlatformStats GetPlatformStats(PlatformType type)
    {
        switch (type)
        {
            case PlatformType.Still:
                return stillPlatfromStats;
            case PlatformType.Moving:
                return movingPlatfromStats;
            case PlatformType.Booster:
                return boosterPlatformStats;
            case PlatformType.Fragile:
                return fragilePlatformStats;
            default:
                Debug.LogError("Invalid PlatformType.");
                return null;
        }
    }

}