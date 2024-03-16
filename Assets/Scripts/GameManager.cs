using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject victoryScreen;
    public GameObject defeatScreen;
    public TextMeshProUGUI resultText;

    private int score;
    private int highScore;
    private int baseEnemyPointValue = 50; // Base point value of each enemy
    private int scoreMultiplier = 1; // Initial score multiplier

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

    public void IncrementScore()
    {
        int points = baseEnemyPointValue * scoreMultiplier;
        score += points;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore); // Save high score to PlayerPrefs
        }

        UpdateUI();
    }

    // Method to update the score multiplier
    public void UpdateScoreMultiplier(int multiplierIncrement)
    {
        scoreMultiplier += multiplierIncrement;
    }

    private void UpdateUI()
    {
        scoreText.text = "SCORE " + score.ToString("D4"); // Display score with leading zeros
        highScoreText.text = "HIGH SCORE " + highScore.ToString("D4");
    }
    
    public void ResetScene()
    {
        // Reset the player position and enable player movement
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = Vector3.zero; // Set the player position to the center of the screen
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.enabled = true; // Enable player movement
            }
        }

        // Reset the enemies and barriers
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject); // Destroy all enemy GameObjects
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }
    
    public void EndGame(bool victory)
    {
        if (victory)
        {
            victoryScreen.SetActive(true);
            resultText.text = "You have defeated the horde!\n" +
                              "Your score was " + score + " points.\n" +
                              (score > highScore ? "New high score!" : "The high score is " + highScore);
        }
        else
        {
            defeatScreen.SetActive(true);
            resultText.text = "You have been overwhelmed by the horde!\n" +
                              "Your score was " + score + " points.\n" +
                              "The high score is " + highScore;
        }
        
        SceneManager.LoadScene("CreditsScene");
    }
    
    IEnumerator LoadCreditsScene()
    {
        yield return new WaitForSeconds(3.0f); // Adjust the delay as needed
        SceneManager.LoadScene("CreditsScene");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Method to count the total number of enemies in the scene
    public int GetTotalEnemyCount()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    // Method to count the total number of lives
    public int GetTotalLives()
    {
        // Implement logic to get the total number of lives
        return 3; // Example: return the fixed number of lives (3)
    }
}
