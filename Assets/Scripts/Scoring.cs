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
            CheckHighScore();
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

    private void CheckHighScore()
    {
        List<int> highScores = GetHighScores();
        if (score > highScores[highScores.Count - 1])
        {
            highScores.Add(score);
            highScores.Sort((a, b) => b.CompareTo(a));
            if (highScores.Count > 5)
            {
                highScores.Remove(highScores.Count - 1);
            }
            SaveHighScores(highScores);
        }
    }

    private List<int> GetHighScores()
    {
        List<int> highScores = new List<int>();
        for (int i = 0; i<5; i++)
        {
            highScores.Add(PlayerPrefs.GetInt("Highscore" + i,0));
        }
        return highScores;
    }

    private void SaveHighScores(List<int> highScores)
    {
        for(int i = 0; i<highScores.Count; i++)
        {
            PlayerPrefs.SetInt("Highscore" + i, highScores[i]);
        }
        PlayerPrefs.Save();
    }

}
