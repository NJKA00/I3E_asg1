/*
 * Author: Kayden
 * Date: 09/06/2026
 * Description: Manages the game state, score, collectibles remaining, and win/lose conditions.
 */
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The player's current score.
    /// </summary>
    private float currentScore;

    /// <summary>
    /// Total number of collectibles in the scene. Set this in the Inspector.
    /// </summary>
    [SerializeField] private int collectibleAmount;

    /// <summary>
    /// How many collectibles are left to collect.
    /// </summary>
    private int collectibleLeft;

    /// <summary>
    /// Whether the game is currently over.
    /// </summary>
    private bool gameOver;

    /// <summary>
    /// Reference to the UIManager for displaying messages.
    /// </summary>
    [SerializeField] private UIManager uiManager;

    /// <summary>
    /// Text displaying the player's current score.
    /// </summary>
    [SerializeField] private TextMeshProUGUI scoreCountDisplay;

    /// <summary>
    /// Text displaying how many collectibles are remaining.
    /// </summary>
    [SerializeField] private TextMeshProUGUI remainingCollectibleDisplay;

    /// <summary>
    /// The Game Over screen panel, shown when the player dies.
    /// </summary>
    [SerializeField] private GameObject gameOverPanel;

    /// <summary>
    /// The Win screen panel, shown when the player escapes.
    /// </summary>
    [SerializeField] private GameObject winPanel;

    /// <summary>
    /// Text displaying the final score on the Win screen.
    /// </summary>
    [SerializeField] private TextMeshProUGUI winScoreText;

    /// <summary>
    /// Text displaying the final score on the Game Over screen.
    /// </summary>
    

    /// <summary>
    /// Called when the script is first initialised.
    /// </summary>
    void Start()
    {
        collectibleLeft = collectibleAmount;
        scoreCountDisplay.text = "Score: " + currentScore;
        remainingCollectibleDisplay.text = "Collectibles remaining: " + collectibleLeft;
    }

    /// <summary>
    /// Adds score when a collectible is picked up and updates the UI.
    /// </summary>
    /// <param name="amount">The score value of the collected item.</param>
    public void AddScore(float amount)
    {
        if (gameOver) return;

        currentScore += amount;
        collectibleLeft--;

        scoreCountDisplay.text = "Score: " + currentScore;

        if (collectibleLeft != 0)
        {
            remainingCollectibleDisplay.text = "Collectibles remaining: " + collectibleLeft;
        }
        else
        {
            remainingCollectibleDisplay.text = "All collectibles collected!";
        }
    }

    /// <summary>
    /// Called when the player dies. Shows the Game Over screen and restarts after 5 seconds.
    /// </summary>
    public void GameOver()
    {
        if (gameOver) return;
        gameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        StartCoroutine(RestartAfterDelay(5f));

        Debug.Log("Game Over!");
    }

    /// <summary>
    /// Called when the player escapes. Shows the Win screen with final score.
    /// </summary>
    public void Win()
    {
        if (gameOver) return;
        gameOver = true;

        winScoreText.text = "Final Score: " + currentScore;
        winPanel.SetActive(true);
        Time.timeScale = 0f;

        Debug.Log("You win!");
    }

    /// <summary>
    /// Waits for a set duration then restarts the scene.
    /// </summary>
    /// <param name="delay">How long to wait before restarting.</param>
    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}