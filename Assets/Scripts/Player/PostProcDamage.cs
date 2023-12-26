// Script: PostProcDamage
// Description: This script manages a post-processing vignette effect to simulate damage impact on the player.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcDamage : MonoBehaviour
{
    // Reference to the Post Process Volume component
    PostProcessVolume _volume;

    // Vignette effect intensity
    private float intensity = 0;

    // Reference to the Vignette post-processing effect
    Vignette _vignette;

    // Reference to the PlayerHealth component
    PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Post Process Volume component attached to this GameObject
        _volume = GetComponent<PostProcessVolume>();

        // Try to get the Vignette settings from the profile
        _volume.profile.TryGetSettings<Vignette>(out _vignette);

        // Check if Vignette is found
        if (!_vignette)
        {
            Debug.LogError("No vignette found");
        }
        else
        {
            // Disable the vignette effect initially
            _vignette.enabled.Override(false);
        }

        // Get the PlayerHealth component
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is hit
        if (playerHealth.isHit)
        {
            // Trigger the damage effect coroutine
            StartCoroutine(TakeDamageEffect());
        }
    }

    // Coroutine for the damage effect
    private IEnumerator TakeDamageEffect()
    {
       
        float intensity = 1f;

        // Enable the vignette effect
        _vignette.enabled.Override(true);

        // Set the intensity of the vignette effect
        _vignette.intensity.Override(intensity);

        // Wait for a short duration
        yield return new WaitForSeconds(0.4f);

        // Gradually decrease the intensity until it reaches zero
        while (intensity > 0)
        {
            intensity -= 0.01f;

            // Ensure intensity does not go below zero
            if (intensity < 0) intensity = 0;

            // Update the intensity of the vignette effect
            _vignette.intensity.Override(intensity);

            // Wait for a short duration before the next intensity update
            yield return new WaitForSeconds(0.1f);
        }

        // Disable the vignette effect when the intensity reaches zero
        _vignette.enabled.Override(false);

        // Exit the coroutine
        yield break;
    }
}
