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

    // Immunity variables
    public GameObject ufoObject; // Reference to the player UFO object
    public float immunityDuration = 3.0f; // Duration of the immunity period in seconds
    public Color immunityColor = Color.yellow; // Color to indicate immunity
    private bool isImmune = false;
    private float immunityStart = 0f;
    private Color originalColor;
    private Renderer ufoRenderer;

    void Start()
    {
        gameOverManager = FindObjectOfType<GameOverManager>();
        UpdateScoreText();
        StartCoroutine(TimePenalty());
        UpdateLives();

        // Get UFO renderer component in children
        ufoRenderer = ufoObject.GetComponentInChildren<Renderer>();
        if(ufoRenderer != null)
        {
            // Store color before immunity color gets involved
            originalColor = ufoRenderer.material.color;
        }
        else
        {
            Debug.LogWarning("No renderer found on UFO");
        }
        
    }

    void Update()
    {
        passedTime += Time.deltaTime;
        UpdateScoreText();

        // Check when immunity ends and set it back to false
        if(isImmune)
        {
            immunityStart -= Time.deltaTime;
            if(immunityStart <= 0f)
            {
                isImmune = false;
                // Reset color back to normal after immunity
                if(ufoRenderer != null)
                {
                    ufoRenderer.material.color = originalColor;
                }
            }

        }
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
        if(!isImmune)
        {
            lives -= 1;
            UpdateLives();
            if (lives <= 0)
            {
                CheckHighScore();
                gameOverManager.GameOver(score);
            } 

            // Start immunity
            isImmune = true;
            immunityStart = immunityDuration;

            // Activate immunity color
            if(ufoRenderer != null)
            {
                ufoRenderer.material.color = immunityColor;
            }
            
        }
    }

    public bool IsImmune()
    {
        return isImmune;
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
