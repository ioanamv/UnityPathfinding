using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistributionSelector : MonoBehaviour
{
    public TMP_Dropdown distrDropdown;
    private static int selectedDistr = 0;

    void Start()
    {
        distrDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(distrDropdown); });
    }

    private void DropdownValueChanged(TMP_Dropdown distrDropdown)
    {
        selectedDistr = distrDropdown.value;
        Debug.Log("Selected distr: " + selectedDistr);
    }

    public static int GetSelectedAlgorithm()
    {
        return selectedDistr;
    }

    public static void ResetSelectedDistribution()
    {
        selectedDistr = 0;
    }
}
