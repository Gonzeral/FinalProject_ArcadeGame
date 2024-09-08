using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scoring : MonoBehaviour
{
    // UI Text to display score
    public Text scoreText; 
    // life icons array
    public GameObject[] lifeIcons; 
    // Points penalty over time
    public int penalty = 10; 
    // Interval for time penalty
    public float penaltyTimer = 5.0f; 
    // Starting score
    private int score = 0; 
    // Lives at the beginning
    private int lives = 3; 
    // Used to track time passed for time penalty
    private float passedTime = 0f; 
    // Reference to GameOverManager
    private GameOverManager gameOverManager;

    // Immunity variables
    public GameObject ufoObject; // Reference to the player UFO object
    public float immunityDuration = 3.0f; // Duration of the immunity period in seconds
    public Color immunityColor = Color.yellow; // Color to indicate immunity
    private bool isImmune = false; // Flag for immunity
    private float immunityStart = 0f; // Timer to track immunity duration
    private Color originalColor; // Original color of UFO for when immunity is over
    private Renderer ufoRenderer; // Rendere component of UFO to get material

    void Start()
    {
        // Get reference to GameOverManager
        gameOverManager = FindObjectOfType<GameOverManager>();
        // Initialize score text and life icons, also start coroutine for time penalty
        UpdateScoreText();
        StartCoroutine(TimePenalty());
        UpdateLives();

        // Get UFO renderer component in children to modify color for immunity
        ufoRenderer = ufoObject.GetComponentInChildren<Renderer>();
        // Check for the UFO renderer component
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
        // Track time for time penalty and immunity
        passedTime += Time.deltaTime;
        // Update score text with every frame
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

    // Method to add points to score
    public void AddPoints(int points)
    {
        score += points;
        // Refresh score text 
        UpdateScoreText(); 
    }

    // Coroutine for the time penalty
    private IEnumerator TimePenalty()
    {
        while (true)
        {
            // Wait for penalty interval
            yield return new WaitForSeconds(penaltyTimer);
            // Subtract time penalty from score
            score -= penalty;
            // Refresh score text
            UpdateScoreText();
        }
    }

    // Method to handle losing lives
    public void LoseLife()
    {
        // Life is lost only when player is not immune
        if(!isImmune)
        {
            // Subtract one life
            lives -= 1;
            // Update life icons
            UpdateLives();
            // Check if player has no more lives
            if (lives <= 0)
            {
                // Update the highscore when game is over
                CheckHighScore();
                // Trigger GameOver state
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

    // Method to check if player is immune
    public bool IsImmune()
    {
        return isImmune;
    }

    // Method to update life icons
    private void UpdateLives()
    {
        for(int i = 0; i < lifeIcons.Length; i++)
        {
            // Set icons active according to player's current lives
            lifeIcons[i].SetActive(i < lives);
        }
    }

    // Update score text
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    // Check if score should be in highscores (Top 5)
    private void CheckHighScore()
    {
        List<int> highScores = GetHighScores();
        // When score is higher than lowest in highscores, update list
        if (score > highScores[highScores.Count - 1])
        {
            highScores.Add(score);
            // Sort highscores in descending order
            highScores.Sort((a, b) => b.CompareTo(a));
            // If there are more than 5 highscores, remove lowest highscore
            if (highScores.Count > 5)
            {
                highScores.Remove(highScores.Count - 1);
            }
            // Save new highscores
            SaveHighScores(highScores);
        }
    }

    // Get the list of highscores from PlayerPrefs
    private List<int> GetHighScores()
    {
        List<int> highScores = new List<int>();
        // Load top 5 highscores from PlayerPrefs
        for (int i = 0; i<5; i++)
        {
            // Set 0 as default when there is no highscore
            highScores.Add(PlayerPrefs.GetInt("Highscore" + i,0));
        }
        return highScores;
    }

    // Save the new list of highscores to PlayerPrefs
    private void SaveHighScores(List<int> highScores)
    {
        // Save each highscore to PlayerPrefs
        for(int i = 0; i<highScores.Count; i++)
        {
            PlayerPrefs.SetInt("Highscore" + i, highScores[i]);
        }
        // Save PlayerPrefs (Persistant)
        PlayerPrefs.Save();
    }

}
