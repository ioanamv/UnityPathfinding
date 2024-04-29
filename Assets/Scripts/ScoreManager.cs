using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int PlayerScore { get; set; }
    public static int OpponentScore { get; set; }

    private void Start()
    {
        PlayerScore = 0;
        OpponentScore = 0;
    }

    public static void IncrementPlayerScore()
    {
        PlayerScore++;
    }

    public static void IncrementOpponentScore()
    {
        OpponentScore++;
    }
}
