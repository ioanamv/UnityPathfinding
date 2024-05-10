using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int PlayerScore { get; set; }
    public static int OpponentScore { get; set; }

    public static TextMeshProUGUI playerScoreText, opponentScoreText;

    private void Start()
    {
        PlayerScore = 0;
        OpponentScore = 0;
    }

    public static void IncrementPlayerScore()
    {
        PlayerScore++;
        ScoreText.Instance.playerScoreText.text = "Player score: " + PlayerScore.ToString();
    }

    public static void IncrementOpponentScore()
    {
        OpponentScore++;
        ScoreText.Instance.opponentScoreText.text = "Opponent score: " + OpponentScore.ToString();
    }
}