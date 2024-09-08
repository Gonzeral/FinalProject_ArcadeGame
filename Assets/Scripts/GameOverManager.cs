using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // Reference to GameOverScreen, UI element
    public GameObject gameOverScreen;
    // Reference to text UI element to show final score
    public Text endScoreText;
    // Flag for tracking game over state
    public bool IsGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        // Make GameOver screen hidden at start of the game
        gameOverScreen.SetActive(false);
    }

    // Method triggering GameOver state
    public void GameOver(int endScore)
    {
        // Set GameOver state (flag) to true
        IsGameOver = true;
        // Show the GameOver screen
        gameOverScreen.SetActive(true);
        // Stop time so game is frozen, no in-game actions
        Time.timeScale = 0f;

        // Check if final score text is assigned and update it with final score
        if(endScoreText != null)
        {
            // Display score at end of the game
            endScoreText.text = "Score: " + endScore.ToString();
        }

        // Enable the buttons for the GameOver screen
        EnableGameOverButtons();
    }

    // Method to restart game
    public void RestartGame()
    {
        // Reset GameOver flag
        IsGameOver = false;
        // Make time run normally again
        Time.timeScale = 1f;
        // Reload the game scene (In-game scene)
        SceneManager.LoadScene(1);
    }

    // Method to return to main menu
    public void ReturnToMainMenu()
    {
        // Reset GameOver flag
        IsGameOver = false;
        // Make time run normally again
        Time.timeScale = 1f;
        // Load main menu scene
        SceneManager.LoadScene(0);
    }

    // Method to enable the buttons in GameOver screen
    private void EnableGameOverButtons()
    {
        // Get children buttons of GameOver screen
        Button[] buttons= gameOverScreen.GetComponentsInChildren<Button>(true);
        // Iterate over the buttons and make them interactable
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }
}
