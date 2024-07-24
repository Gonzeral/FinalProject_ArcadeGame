using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    public Text[] highScoreTexts; // Array of Texts to display highscores

    // Start is called before the first frame update
    void Start()
    {
        DisplayHighscores();
    }

    public void DisplayHighscores()
    {
        List<int> highScores = GetHighScores();
        for (int i = 0; i<highScoreTexts.Length; i++)
        {
            if (i < highScores.Count)
            {
                highScoreTexts[i].text = (i+1) + ". " + highScores[i].ToString();
            }
            else
            {
            highScoreTexts[i].text = (i+1) + ". 0";
            }

        }        
        
    }

    private List<int> GetHighScores()
    {
        List<int> highScores = new List<int>();
        for (int i = 0; i<5; i++)
        {
            highScores.Add(PlayerPrefs.GetInt("Highscore" + i, 0));
        }
        return highScores;
    }

}
