using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PathfindingSelector : MonoBehaviour
{
    public TMP_Dropdown algorithmDropdown;
    private static int selectedAlgorithm = 0;

    void Start()
    {
        algorithmDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(algorithmDropdown); });
    }

    private void DropdownValueChanged(TMP_Dropdown algorithmDropdown)
    {
        selectedAlgorithm = algorithmDropdown.value;
        Debug.Log("Selected Algorithm: " + selectedAlgorithm);
    }

    public static int GetSelectedAlgorithm()
    {
        return selectedAlgorithm;
    }
    
    public static void ResetSelectedAlgorithm()
    {
        selectedAlgorithm = 0;
    }
}
