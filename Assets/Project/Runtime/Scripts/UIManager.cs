using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    // refactor to scoremanager
    [SerializeField] private TextMeshProUGUI txtScore;
    [SerializeField] private TextMeshProUGUI txtDebug;

    private void Update()
    {
        txtScore.text = ((int) ScoreManager.Instance.CurrentScore).ToString();
        DisplayDebugOverlay();
    }

    private void DisplayDebugOverlay()
    {
        txtDebug.text 
            = "Current Position: " + GameManager.Instance.player.position
            + ""
            ;
    }
}
