using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    public Text scoreText;
    public int penalty = 10; 
    public float penaltyTimer = 5.0f;
    private int score = 0;
    private float passedTime = 0f;

    void Start()
    {
        UpdateScoreText();
        StartCoroutine(TimePenalty());
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

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
