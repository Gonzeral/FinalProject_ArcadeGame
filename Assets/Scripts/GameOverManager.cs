using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public bool IsGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        gameOverScreen.SetActive(false);
    }

    public void GameOver()
    {
        IsGameOver = true;
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
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
