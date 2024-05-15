using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    public TextMeshProUGUI playerScoreText, opponentScoreText, finalState;
    public Button nextLevelButton;
    public static ScoreText Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        playerScoreText.text = "Player score: 0";
        opponentScoreText.text = "Opponent score: 0";
        nextLevelButton.gameObject.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
