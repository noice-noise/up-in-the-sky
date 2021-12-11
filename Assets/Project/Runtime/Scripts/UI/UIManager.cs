using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    // refactor to scoremanager
    [SerializeField] private TextMeshProUGUI txtScore;


    private void Update()
    {
        txtScore.text = ((int) ScoreManager.Instance.CurrentScore).ToString();
    }


}
