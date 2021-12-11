using UnityEngine;

public class DifficultyHandler : MonoBehaviour
{
    [SerializeField] private LevelDifficulty[] levels;
    private int baseDifficulty = 0;
    private int currentDifficulty;

    public bool freezeDifficultyUpdate = false;

    public int CurrentDifficulty { get => currentDifficulty; set => currentDifficulty = value; }

    public void InitializeDifficulty(int value)
    {
        currentDifficulty = value;
    }

    public void ResetDifficulty(int value)
    {
        currentDifficulty = baseDifficulty;
    }

    public void HandleDifficulty(float currentGameStateHeight)
    {
        if (!freezeDifficultyUpdate && IsRequirementsAchived(currentGameStateHeight))
        {
            if (currentDifficulty + 1 < levels.Length)
            {
                IncreaseDifficulty();
            }
        }
    }

    public void IncreaseDifficulty()
    {
        currentDifficulty++;
    }

    public bool IsRequirementsAchived(float criteriaValue)
    {
        if (criteriaValue >= levels[CurrentDifficulty].heightRequirement)
        {
            return true;
        }

        return false;
    }
}