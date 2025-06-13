using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer247 : MonoBehaviour
{
    private static MusicPlayer247 instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Duplicate MusicPlayer detected, destroying this one.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("MusicPlayer is now persistent.");

        AudioSource audio = GetComponent<AudioSource>();
        if (!audio.isPlaying)
        {
            Debug.Log("Starting music.");
            audio.loop = true;
            audio.playOnAwake = true;
            audio.Play();
        }
    }
}
