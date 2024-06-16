using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlTime : MonoBehaviour
{
    public string prevLvl;
    public static Stopwatch swLvl = new Stopwatch();

    private void Awake()
    {
        if (MovementManager.noPlayer)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void Start()
    {
        if (MovementManager.noPlayer)
        {
            swLvl.Start();
        }
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (MovementManager.noPlayer)
        {
            swLvl.Stop();

            if (prevLvl!=null)
            {
                SaveToFile(prevLvl, swLvl.Elapsed.TotalSeconds);
            }
            
            swLvl.Reset();
            swLvl.Start();
        }
    }

    private void SaveToFile(string level, double elapsedTime)
    {
        //string filePath = Application.dataPath + "/level_times.csv";
        string directoryPath = "D:\\";
        string filePath = Path.Combine(directoryPath, "level_times.csv");

        if (!File.Exists(filePath))
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    writer.WriteLine("Level 1, Level 2, Level 3, Level 4, Level 5");
                }
            }
            catch (IOException e)
            {
                UnityEngine.Debug.LogError("Failed to create file: " + e.Message);
                return;
            }
        }

        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                if (level != "Level 5")
                    writer.Write($"{elapsedTime},");
                else writer.Write($"{elapsedTime} \n");
            }
        }
        catch (IOException e)
        {
            UnityEngine.Debug.LogError("Failed to write to file: " + e.Message);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
