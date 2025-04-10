using UnityEngine;
using TMPro;

public class LeaderboardEntry : MonoBehaviour
{
    public TextMeshProUGUI rollText;
    public TextMeshProUGUI scoreText;

    public void SetData(string roll, int score)
    {
        rollText.text = roll;
        scoreText.text = score.ToString();
    }

    public void UpdateScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }
    public int GetScore()
    {
        return int.Parse(scoreText.text);
    }
}