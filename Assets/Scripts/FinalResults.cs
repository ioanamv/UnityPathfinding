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
            RoundsWinner.text = "Winner for rounds won: Player";
        }
        else if (ScoreManager.NoRoundsWonPlayer < ScoreManager.NoRoundsWonOpponent)
        {
            RoundsWinner.text = "Winner for rounds won: Opponent";
        }
        else
        {
            RoundsWinner.text = "Draw by number of rounds won";
        }

        if (ScoreManager.NoBonusesCollectedPlayer > ScoreManager.NoBonusesCollectedOpponent)
        {
            BonusWinner.text = "Winner for bonus collection: Player";
        }
        else if (ScoreManager.NoBonusesCollectedPlayer < ScoreManager.NoBonusesCollectedOpponent)
        {
            BonusWinner.text = "Winner for bonus collection: Opponent";
        }
        else
        {
            BonusWinner.text = "Draw by number of bonuses collected";
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
