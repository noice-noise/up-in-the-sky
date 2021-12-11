using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DifficultyData", menuName = "UpInTheSky/DifficultyData", order = 0)]
public class DifficultyData : ScriptableObject
{
    public List<LevelDifficulty> levels;
}