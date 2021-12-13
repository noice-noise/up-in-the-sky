using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{

    [SerializeField] private DifficultyHandler difficultyHandler;
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private LevelDataContainer levelDataContainer;
    
    public float currentPlayerPosition;


    public DifficultyHandler DifficultyHandler
    {
        get { return difficultyHandler; }
        set
        {
            difficultyHandler = value;
        }
    }

    public LevelGenerator LevelGenerator
    {
        get { return levelGenerator; }
        set
        {
            levelGenerator = value;
        }
    }

    public LevelDataContainer LevelDataContainer
    {
        get { return levelDataContainer; }
        set
        {
            levelDataContainer = value;
        }
    }

    private void Update()
    {
        currentPlayerPosition = GameManager.Instance.player.position.y;

        levelGenerator.HandleLevelGeneration();
        difficultyHandler.HandleDifficulty(currentPlayerPosition);
    }

}
