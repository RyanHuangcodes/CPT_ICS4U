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

        List<string> options = new List<string> { "Fullscreen", "Windowed" };
        fullscreenDropdown.ClearOptions();
        fullscreenDropdown.AddOptions(options);
        fullscreenDropdown.value = Screen.fullScreen ? 0 : 1;
        fullscreenDropdown.RefreshShownValue();
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
