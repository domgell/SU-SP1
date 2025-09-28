using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; 
    public int score;

    private void FixedUpdate()
    {
        scoreText.text = $"Score: {score}";

        if (score >= 10)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("WinScreen");
        }
    }
}
