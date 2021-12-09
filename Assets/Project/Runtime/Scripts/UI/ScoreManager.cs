using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{

    private float currentScore;

    public float CurrentScore { get => currentScore; }

    [SerializeField] Transform playerTransform;

    private void Update() {
        var playerPos =  playerTransform.position.y;
        if ( playerPos > currentScore)
        {
            float addScore = playerPos - currentScore;
            AddScore(addScore);
        }
    }

    public void AddScore(float score)
    {
        currentScore += score;
    }
}