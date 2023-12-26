// Script: SoundManager
// Description: Manages sound effects and music in the game, providing methods to play and control audio.
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton instance to ensure only one SoundManager exists
    public static SoundManager Instance;

    // Maximum distance for 3D sound spatialization
    public float soundTravelDistance = 20f;

    public string songName;
    // Arrays for storing sound effects and music tracks
    public AudioClip[] soundClips;
    public AudioClip[] musicTracks;

    // AudioSources for playing sound effects and music
    private AudioSource soundSource;
    private AudioSource musicSource;

    // Default volumes for sound effects and music
    public float soundVolume = 1f;
    public float musicVolume = 1f;

    // Create a pool of AudioSources for sound effects to prevent overlapping/cancellations
    private List<AudioSource> soundEffectSources = new List<AudioSource>();
    private int currentSoundEffectIndex = 0;

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

        // Create a pool of AudioSources for sound effects
        for (int i = 0; i < 5; i++) 
        {
            AudioSource soundEffectSource = gameObject.AddComponent<AudioSource>();
            soundEffectSources.Add(soundEffectSource);
        }
    }

    // Start method called on the first frame; sets initial volumes and plays an initial music track
    private void Start()
    {
        // Set initial volumes
        soundSource.volume = soundVolume;
        musicSource.volume = musicVolume;

        // Play the initial music track
        PlayMusic(songName, musicVolume);
        PlaySound("Monster_Noise");
    }

    // Play a sound effect without specifying a position
    public void PlaySound(string soundName, float volume = 1f)
    {
        PlaySoundAtPosition(soundName, Vector3.zero, volume);
    }

    public void PlaySoundAtPosition(string soundName, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        AudioClip clipToPlay = FindAudioClip(soundName, soundClips);

        if (clipToPlay != null)
        {
            // Get the next available AudioSource from the pool
            AudioSource soundEffectSource = soundEffectSources[currentSoundEffectIndex];

            // Check if the AudioSource is currently playing
            if (!IsSoundPlaying(soundEffectSource))
            {
                // Set properties and play the sound effect
                soundEffectSource.transform.position = position;
                soundEffectSource.clip = clipToPlay;
                soundEffectSource.volume = Mathf.Clamp01(volume);
                soundEffectSource.pitch = Mathf.Clamp(pitch, 0.1f, 3f);

                soundEffectSource.Play();

                // Increment the index for the next available AudioSource
                currentSoundEffectIndex = (currentSoundEffectIndex + 1) % soundEffectSources.Count;
            }
            else
            {
                // Log a message or handle the case where the AudioSource is already playing
                Debug.LogWarning("Sound is already playing. Wait for it to finish or use a different AudioSource.");
            }
        }
    }

    // Check if an AudioSource is currently playing
    private bool IsSoundPlaying(AudioSource audioSource)
    {
        return audioSource.isPlaying && audioSource.time < audioSource.clip.length;
    }

    // Play a music track
    public void PlayMusic(string trackName, float volume = 1f, float playbackPosition = 0f)
    {
        AudioClip trackToPlay = FindAudioClip(trackName, musicTracks);

        if (trackToPlay != null)
        {
            musicSource.clip = trackToPlay;
            musicSource.volume = Mathf.Clamp01(volume);
            musicSource.time = playbackPosition;
            musicSource.Play();
        }
    }
    //So music doesn't restart each time
    public float GetMusicPlaybackPosition()
    {
        return musicSource.time;
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
