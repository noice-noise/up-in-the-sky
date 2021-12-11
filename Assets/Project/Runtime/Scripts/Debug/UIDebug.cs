using TMPro;
using UnityEngine;

public class UIDebug : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI txtDebug;
    private GameManager gameManager;
    private LevelManager levelManager;
    private PlayerController playerController;

    private void Start()
    {
        gameManager = GameManager.Instance;
        levelManager = LevelManager.Instance;
        playerController = gameManager.player.gameObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
        DisplayDebugOverlay();    
    }

    private void DisplayDebugOverlay()
    {
        txtDebug.text
            = "\n" + "Player Velocity: " + playerController.rbVelocity
            + "\n" + "Player Position: " + gameManager.player.position
            + "\n" + "Difficulty Level: " + levelManager.DifficultyHandler.CurrentDifficulty;


        ;
    }

}