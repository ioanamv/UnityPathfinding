using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLv1 : MonoBehaviour
{
    public static Stopwatch stopwatch;

    private void Start()
    {
        if (Pathfinding.noPlayer)
        {
            stopwatch = new Stopwatch();
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        stopwatch.Start();
    }
}
