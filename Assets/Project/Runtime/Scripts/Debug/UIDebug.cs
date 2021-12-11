using TMPro;
using UnityEngine;

public class UIDebug : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI txtDebug;

    private void Update()
    {
        DisplayDebugOverlay();    
    }

    private void DisplayDebugOverlay()
    {
        txtDebug.text
            = "Current Position: " + GameManager.Instance.player.position
            + "\n" + "Difficulty Level: " + LevelManager.Instance.DifficultyHandler.CurrentDifficulty;


        ;
    }

}