using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DifficultyData", menuName = "UpInTheSky/DifficultyData", order = 0)]
public class DifficultyData : ScriptableObject
{
    [SerializeField] private List<LevelDifficulty> levels;

    public List<LevelDifficulty> Levels { get => levels; set => levels = value; }
}