using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class FullscreenTMPDropdownHandler : MonoBehaviour
{
    public TMP_Dropdown fullscreenDropdown;

    void Start()
    {
        if (fullscreenDropdown == null)
        {
            Debug.LogError("TMP_Dropdown is not assigned in the inspector!");
            return;
        }

        // Setup dropdown options
        List<string> options = new List<string> { "Fullscreen", "Windowed" };
        fullscreenDropdown.ClearOptions();
        fullscreenDropdown.AddOptions(options);

        // Set initial dropdown value according to current fullscreen state
        fullscreenDropdown.value = Screen.fullScreen ? 0 : 1;
        fullscreenDropdown.RefreshShownValue();

        // Add listener for dropdown value change
        fullscreenDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int index)
    {
        Screen.fullScreen = (index == 0);
        Debug.Log("Fullscreen mode is now: " + Screen.fullScreen);
    }

    void OnDestroy()
    {
        if (fullscreenDropdown != null)
            fullscreenDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }
}
