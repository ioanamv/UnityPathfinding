using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int PlayerScore { get; set; }
    public static int OpponentScore { get; set; }

    public static int NoBonusesCollectedPlayer, NoBonusesCollectedOpponent, NoRoundsWonPlayer, NoRoundsWonOpponent;

    public static TextMeshProUGUI playerScoreText, opponentScoreText, finalState;

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

    public static void ResetScoresForNextLevel()
    {
        NoBonusesCollectedPlayer += PlayerScore;
        NoBonusesCollectedOpponent += OpponentScore;
        PlayerScore = 0;
        OpponentScore = 0;
    }

    public static void PrintFinalState(int state)
    {
        switch (state)
        {
            case 0:
                ScoreText.Instance.finalState.text = "Player wins this round";
                NoRoundsWonPlayer++;
                break;
            case 1:
                ScoreText.Instance.finalState.text = "Opponent wins this round";
                NoRoundsWonOpponent++;
                break;
            case 2:
                ScoreText.Instance.finalState.text = "Draw";
                NoRoundsWonPlayer++;
                NoRoundsWonOpponent++;
                break;
        }
        ScoreText.Instance.nextLevelButton.gameObject.SetActive(true);
    }

    public static void ResetScores()
    {
        NoBonusesCollectedPlayer = 0;
        NoBonusesCollectedOpponent = 0;
        NoRoundsWonPlayer= 0; 
        NoRoundsWonOpponent = 0;
    }
}