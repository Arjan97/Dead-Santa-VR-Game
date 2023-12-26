// Script: SoundManager
// Description: Manages sound effects and music in the game, providing methods to play and control audio.

using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton instance to ensure only one SoundManager exists
    public static SoundManager Instance;

    // Maximum distance for 3D sound spatialization
    public float soundTravelDistance = 20f;

    // Arrays for storing sound effects and music tracks
    public AudioClip[] soundClips;
    public AudioClip[] musicTracks;

    // AudioSources for playing sound effects and music
    private AudioSource soundSource;
    private AudioSource musicSource;

    // Default volumes for sound effects and music
    public float soundVolume = 1f;
    public float musicVolume = 1f;

    // Awake method called before Start; handles singleton pattern and initializes AudioSources
    private void Awake()
    {
        // Check if an instance of SoundManager already exists
        if (Instance == null)
        {
            // If not, set this instance as the singleton and ensure it persists between scenes
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this duplicate instance
            Destroy(gameObject);
        }

        // Create AudioSources for sound effects and music
        soundSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();

        // Configure 3D spatialization for sound effects
        soundSource.spatialBlend = 1f; // 1 means fully 3D, 0 means 2D
        soundSource.rolloffMode = AudioRolloffMode.Linear;
        soundSource.maxDistance = soundTravelDistance;
    }

    // Start method called on the first frame; sets initial volumes and plays an initial music track
    private void Start()
    {
        // Set initial volumes
        soundSource.volume = soundVolume;
        musicSource.volume = musicVolume;

        // Play the initial music track
        PlayMusic("ForestMusic", 0.7f);
    }

    // Play a sound effect without specifying a position
    public void PlaySound(string soundName, float volume = 1f)
    {
        PlaySoundAtPosition(soundName, Vector3.zero, volume);
    }

    // Play a sound effect at a specific position
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

    // Play a music track
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

    // Set the volume for sound effects
    public void SetSoundVolume(float volume)
    {
        soundVolume = Mathf.Clamp01(volume);
        soundSource.volume = soundVolume;
    }

    // Set the volume for music tracks
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    // Find an audio clip by name in the specified array
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
