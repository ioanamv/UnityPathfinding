using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartObservingAlg : MonoBehaviour
{
    public static Stopwatch swPerLvl;
    public static Stopwatch totalStopwatch;
    public TMP_InputField number;
    public static int repeats;

    private void Start()
    {
        swPerLvl = new Stopwatch();
        totalStopwatch = new Stopwatch();
    }

    public void LoadScene(string sceneName)
    {
        MovementManager.noPlayer = true;
        Int32.TryParse(number.text, out repeats);
        SceneManager.LoadScene(sceneName);
        swPerLvl.Start();
        totalStopwatch.Start();
    }
}