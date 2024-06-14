using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlTime : MonoBehaviour
{
    public string prevLvl;
    public static Stopwatch swLvl = new Stopwatch();

    private void Awake()
    {
        if (Pathfinding.noPlayer)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void Start()
    {
        if (Pathfinding.noPlayer)
        {
            swLvl.Start();
        }
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (Pathfinding.noPlayer)
        {
            swLvl.Stop();
            print(prevLvl + " : " + swLvl.Elapsed.TotalSeconds);
            swLvl.Reset();
            swLvl.Start();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
