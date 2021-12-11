using UnityEngine;

public class DifficultyHandler : MonoBehaviour
{
    // [SerializeField] private LevelDifficulty[] levels;
    [SerializeField] private DifficultyData difficultyData;
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
            if (currentDifficulty + 1 < difficultyData.Levels.Count)
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
        if (criteriaValue >= difficultyData.Levels[currentDifficulty].heightRequirement)
        {
            return true;
        }

        return false;
    }
}