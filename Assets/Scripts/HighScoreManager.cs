using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    // Array of text components to show highscores
    public Text[] highScoreTexts;

    // Start is called before the first frame update
    void Start()
    {
        // Display the highscores when game starts
        DisplayHighscores();
    }

    // Method to display highscores in menu
    public void DisplayHighscores()
    {
        // Get saved highscores list
        List<int> highScores = GetHighScores();
        // Iterate through the highscores
        for (int i = 0; i<highScoreTexts.Length; i++)
        {
            // If there is a highscore for the index, show it
            if (i < highScores.Count)
            {
                // Display rank and score
                highScoreTexts[i].text = (i+1) + ". " + highScores[i].ToString();
            }
            // If there is no highscore, diesplay 0
            else
            {
                // Display placeholder 0
                highScoreTexts[i].text = (i+1) + ". 0";
            }

        }        
        
    }

    // Method to get highscores from PlayerPrefs
    private List<int> GetHighScores()
    {
        List<int> highScores = new List<int>();
        // Get top 5 highscores from PlayerPrefs
        for (int i = 0; i<5; i++)
        {
            // Get saved score or use 0 when no score is available
            highScores.Add(PlayerPrefs.GetInt("Highscore" + i, 0));
        }
        // Return list of highscores
        return highScores;
    }

}
