using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeController : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource musicSource;

    void Start()
    {
        if (volumeSlider == null || musicSource == null)
        {
            Debug.LogError("Assign slider and AudioSource in inspector!");
            return;
        }

        // Start slider at 0 (mute)
        volumeSlider.value = 0;
        musicSource.volume = 0;

        // Add listener for slider changes
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnVolumeChanged(float value)
    {
        musicSource.volume = value;
    }

    void OnDestroy()
    {
        volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
    }
}
