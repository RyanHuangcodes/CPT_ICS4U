using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    private Slider slider;
    private AudioSource musicSource;

    void Start()
    {
        slider = GetComponent<Slider>();
        musicSource = FindObjectOfType<MusicPlayer247>().GetComponent<AudioSource>();
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        slider.value = savedVolume;
        musicSource.volume = savedVolume;
        slider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnVolumeChanged(float value)
    {
        if (musicSource != null)
        {
            musicSource.volume = value;
            PlayerPrefs.SetFloat("MusicVolume", value);
            PlayerPrefs.Save();
        }
    }
}
