using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scoring : MonoBehaviour
{
    public Text scoreText;
    public GameObject[] lifeIcons; // life icons array
    public int penalty = 10; 
    public float penaltyTimer = 5.0f;
    private int score = 0;
    private int lives = 3; // Lives at the beginning
    private float passedTime = 0f;
    private GameOverManager gameOverManager;

    void Start()
    {
        gameOverManager = FindObjectOfType<GameOverManager>();
        UpdateScoreText();
        StartCoroutine(TimePenalty());
        UpdateLives();
    }

    void Update()
    {
        passedTime += Time.deltaTime;
        UpdateScoreText();
    }

    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private IEnumerator TimePenalty()
    {
        while (true)
        {
            yield return new WaitForSeconds(penaltyTimer);
            score -= penalty;
            UpdateScoreText();
        }
    }

    public void LoseLife()
    {
        lives -= 1;
        UpdateLives();
        if (lives <= 0)
        {
            gameOverManager.GameOver();
        }
    }

    private void UpdateLives()
    {
        for(int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].SetActive(i < lives);
        }
    }
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

}
