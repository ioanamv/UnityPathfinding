using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLv1 : MonoBehaviour
{
    public static Stopwatch totalStopwatch;

    private void Start()
    {
        if (Pathfinding.noPlayer)
        {
            totalStopwatch = new Stopwatch();
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        if (Pathfinding.noPlayer)
        {
            totalStopwatch.Start();
        }
    }
}
