using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreText : MonoBehaviour
{
    public TextMeshProUGUI playerScoreText, opponentScoreText;
    public static ScoreText Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        playerScoreText.text = "Player score: 0";
        opponentScoreText.text = "Opponent score: 0";
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
