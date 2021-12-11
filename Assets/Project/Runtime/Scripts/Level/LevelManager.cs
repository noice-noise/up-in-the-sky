using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{

    [SerializeField] private DifficultyHandler difficultyHandler;
    [SerializeField] private LevelGenerator levelGenerator;
    
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


    private void Update()
    {
        currentPlayerPosition = GameManager.Instance.player.position.y;

        levelGenerator.HandleLevelGeneration();
        difficultyHandler.HandleDifficulty(currentPlayerPosition);
    }

}
