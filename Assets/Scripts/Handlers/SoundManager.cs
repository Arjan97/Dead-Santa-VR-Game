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
    public AudioClip[] ambientSoundClips;

    // Interval between ambient sound plays in seconds
    public float ambientSoundInterval = 5f;
    public float minAmbientInterval = 5f; 
    public float maxAmbientInterval = 15f;
    private float lastAmbientSoundTime;

    // AudioSources for playing sound effects and music
    private AudioSource soundSource;
    private AudioSource musicSource;

    // Default volumes for sound effects and music
    public float soundVolume = 1f;
    public float musicVolume = 1f;
    public float ambientVolume = 1f;
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

        // Initialize lastAmbientSoundTime
        lastAmbientSoundTime = Time.time; 

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
    }

    private void Update()
    {
        // Play ambient sounds randomly
        PlayRandomAmbientSound();
    }


    private void PlayRandomAmbientSound()
    {
        if (ambientSoundClips.Length != 0)
        {
            // Check if enough time has passed since the last ambient sound play
            if (Time.time - lastAmbientSoundTime > ambientSoundInterval)
            {
                // Get a random ambient sound clip
                AudioClip randomAmbientClip = ambientSoundClips[Random.Range(0, ambientSoundClips.Length)];

                // Play the ambient sound
                PlaySound(randomAmbientClip.name, ambientVolume);

                // Update lastAmbientSoundTime
                lastAmbientSoundTime = Time.time;

                // Generate a new random interval for the next ambient sound play
                ambientSoundInterval = Random.Range(minAmbientInterval, maxAmbientInterval);
            }
        }
      
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
            // Find the first available AudioSource in the pool
            AudioSource soundEffectSource = GetAvailableSoundEffectSource();

            if (soundEffectSource != null)
            {
                // Set properties and play the sound effect
                soundEffectSource.transform.position = position;
                soundEffectSource.clip = clipToPlay;
                soundEffectSource.volume = Mathf.Clamp01(volume);
                soundEffectSource.pitch = Mathf.Clamp(pitch, 0.1f, 3f);
                // Set to 3d audio, linear rolloff mode and max distance
                soundEffectSource.spatialBlend = 1f;
                soundEffectSource.rolloffMode = AudioRolloffMode.Linear;
                soundEffectSource.maxDistance = soundTravelDistance;
                Debug.Log($"Playing sound: {soundName} at position: {position}");

                soundEffectSource.Play();
            }
            else
            {
                // Log a message or handle the case where no AudioSource is available
                Debug.LogWarning("No available AudioSource. Wait for one to be free or increase the pool size.");
            }
        }
    }

    private AudioSource GetAvailableSoundEffectSource()
    {
        // Check each AudioSource in the pool to find one that is not playing
        foreach (AudioSource source in soundEffectSources)
        {
            if (!IsSoundPlaying(source))
            {
                return source;
            }
        }

        // If all sources are currently playing, return null
        return null;
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

    // Find an audio clip by name in the specified arrays
    private AudioClip FindAudioClip(string clipName)
    {
        AudioClip foundClip;

        // Search in soundClips array
        foundClip = FindAudioClip(clipName, soundClips);
        if (foundClip != null)
        {
            return foundClip;
        }

        // Search in ambientSoundClips array
        foundClip = FindAudioClip(clipName, ambientSoundClips);
        if (foundClip != null)
        {
            return foundClip;
        }

        // Search in musicTracks array
        foundClip = FindAudioClip(clipName, musicTracks);
        if (foundClip != null)
        {
            return foundClip;
        }

        return null;
    }

}
