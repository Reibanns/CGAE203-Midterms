using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private int score = 0;

    public void AddScore(int segments)
    {
        score += segments * 10;
        scoreText.text = "Score: " + score.ToString();
    }
}
