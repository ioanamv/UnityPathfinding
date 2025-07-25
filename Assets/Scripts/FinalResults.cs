using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class FinalResults : MonoBehaviour
{
    public TextMeshProUGUI playerBonuses, opponentBonuses, playerRounds, opponentRounds, BonusWinner, RoundsWinner;
    public Button playAgain;

    private void Start()
    {
        playerBonuses.text = ScoreManager.NoBonusesCollectedPlayer.ToString();
        opponentBonuses.text = ScoreManager.NoBonusesCollectedOpponent.ToString();
        playerRounds.text = ScoreManager.NoRoundsWonPlayer.ToString();
        opponentRounds.text = ScoreManager.NoRoundsWonOpponent.ToString();
        if (ScoreManager.NoRoundsWonPlayer>ScoreManager.NoRoundsWonOpponent)
        {
            RoundsWinner.text = "Winner for rounds won: \nPlayer";
        }
        else if (ScoreManager.NoRoundsWonPlayer < ScoreManager.NoRoundsWonOpponent)
        {
            RoundsWinner.text = "Winner for rounds won: \nOpponent";
        }
        else
        {
            RoundsWinner.text = "Draw by number of rounds won";
        }

        if (ScoreManager.NoBonusesCollectedPlayer > ScoreManager.NoBonusesCollectedOpponent)
        {
            BonusWinner.text = "Winner for bonus collection: \nPlayer";
        }
        else if (ScoreManager.NoBonusesCollectedPlayer < ScoreManager.NoBonusesCollectedOpponent)
        {
            BonusWinner.text = "Winner for bonus collection: \nOpponent";
        }
        else
        {
            BonusWinner.text = "Draw by number of bonuses collected";
        }
        if (MovementManager.noPlayer)
        {
            StartObservingAlg.totalStopwatch.Stop();
            print("game time:" + StartObservingAlg.totalStopwatch.Elapsed.TotalSeconds + " s");
        }

        if (MovementManager.noPlayer)
        {
            PlayAgain();
        }
    }

    public void LoadScene(string sceneName)
    {
        ScoreManager.ResetScores();
        PathfindingSelector.ResetSelectedAlgorithm();
        DistributionSelector.ResetSelectedDistribution();
        SceneManager.LoadScene(sceneName);
    }

    public void PlayAgain()
    {
        if (StartObservingAlg.repeats -1 > 0)
        {
            StartObservingAlg.repeats--;
            SceneManager.LoadScene("Level1");
        }
    }
}
