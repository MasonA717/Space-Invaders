using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    
    private int score;
    private int highScore;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        score = 0;
        highScore = PlayerPrefs.GetInt("HighScore", 0); // Load high score from PlayerPrefs

        UpdateUI();
    }

    public void IncrementScore(int points)
    {
        score += points;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore); // Save high score to PlayerPrefs
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = "SCORE " + score.ToString("D4"); // Display score with leading zeros
        highScoreText.text = "HIGH SCORE " + highScore.ToString("D4");
    }

    public void EndGame()
    {
        // Game over logic, reset the game or show game over screen
        Debug.Log("Game Over!");
    }
}