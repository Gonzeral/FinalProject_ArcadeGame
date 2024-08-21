using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public Text endScoreText;
    public bool IsGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        gameOverScreen.SetActive(false);
    }

    public void GameOver(int endScore)
    {
        IsGameOver = true;
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;

        if(endScoreText != null)
        {
            // Display score at end of the game
            endScoreText.text = "Your Score: " + endScore.ToString();
        }

        EnableGameOverButtons();
    }

    public void RestartGame()
    {
        IsGameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void ReturnToMainMenu()
    {
        IsGameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void EnableGameOverButtons()
    {
        Button[] buttons= gameOverScreen.GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }
}
