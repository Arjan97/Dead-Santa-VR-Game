using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip[] soundClips;
    public AudioClip[] musicTracks;

    private AudioSource soundSource;
    private AudioSource musicSource;

    public float soundVolume = 1f;
    public float musicVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Create AudioSources for sound effects and music
        soundSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        // Set initial volumes
        soundSource.volume = soundVolume;
        musicSource.volume = musicVolume;
        PlayMusic("ForestMusic", 0.7f);
    }

    public void PlaySound(string soundName, float volume = 1f)
    {
        PlaySoundAtPosition(soundName, Vector3.zero, volume);
    }

    public void PlaySoundAtPosition(string soundName, Vector3 position, float volume = 1f)
    {
        AudioClip clipToPlay = FindAudioClip(soundName, soundClips);

        if (clipToPlay != null)
        {
            soundSource.transform.position = position;
            soundSource.clip = clipToPlay;
            soundSource.volume = Mathf.Clamp01(volume);
            soundSource.Play();
        }
    }

    public void PlayMusic(string trackName, float volume = 1f)
    {
        AudioClip trackToPlay = FindAudioClip(trackName, musicTracks);

        if (trackToPlay != null)
        {
            musicSource.clip = trackToPlay;
            musicSource.volume = Mathf.Clamp01(volume);
            musicSource.Play();
        }
    }

    public void SetSoundVolume(float volume)
    {
        soundVolume = Mathf.Clamp01(volume);
        soundSource.volume = soundVolume;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    private AudioClip FindAudioClip(string clipName, AudioClip[] clipArray)
    {
        foreach (AudioClip clip in clipArray)
        {
            if (clip.name == clipName)
            {
                return clip;
            }
        }

        Debug.LogWarning("Audio clip not found: " + clipName);
        return null;
    }
}
