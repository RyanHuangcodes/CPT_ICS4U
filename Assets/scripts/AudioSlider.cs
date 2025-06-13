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

        volumeSlider.value = 0;
        musicSource.volume = 0;
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
