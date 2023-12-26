// Script: PlayerHealth
// Description: Manages the health and respawn functionality of the player.

using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    // Maximum health of the player
    public float maxHealth = 100f;

    // Cooldown period during which the player is invincible after being hit
    public float beingHitCooldown = 2f;

    // Indicates whether the player is currently hit
    public bool isHit = false;

    // Indicates whether the player is dead
    public bool isDead = false;

    // Respawn point for the player
    public Transform respawnPoint;

    // Delay before respawning after death
    public float respawnDelay = 10f;

    // Text displaying respawn information
    public TextMeshProUGUI respawnText;

    // Text displaying the countdown timer for respawn
    public TextMeshProUGUI respawnTimerText;

    // Panel GameObject representing the UI for respawn
    public GameObject panel;

    // GameObject representing the player's movement
    public GameObject playerMovement;

    // Indicates whether the player is invincible
    [SerializeField] private bool isInvincible = false;

    // Current health of the player
    [SerializeField] private float currentHealth;

    // Coroutine reference for respawn
    private Coroutine respawnCoroutine;

    void Start()
    {
        // Initialize current health to maximum health
        currentHealth = maxHealth;

        // Deactivate the respawn panel and activate the player's movement
        panel.SetActive(false);
        playerMovement.SetActive(true);
    }

    // Function to handle player taking damage
    public void TakeDamage(float damage)
    {
        // Check if the player is not invincible and not dead
        if (!isInvincible && !isDead)
        {
            // Reduce the current health by the damage amount
            currentHealth -= damage;
            Debug.Log("Player got damaged for: " + damage);

            // Play a sound at a specific position
            int randomPainSound = Random.Range(1, 7);
            SoundManager.Instance.PlaySoundAtPosition("Pain_" + randomPainSound, transform.position);

            // Set isHit to true
            isHit = true;

            // Check if the current health is less than or equal to 0
            if (currentHealth <= 0)
            {
                // Call the Die function
                Die();
            }

            // Start the invincibility coroutine
            StartCoroutine(Invincibility());
        }
    }

    // Function to handle player death
    void Die()
    {
        Debug.Log("Player has died!");

        // Activate the respawn panel and deactivate the player's movement
        panel.SetActive(true);
        playerMovement.SetActive(false);

        // Set isDead to true
        isDead = true;

        // Check if the respawnCoroutine is not null and stop it
        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
        }

        // Invoke the ReloadScene function after the specified respawnDelay
        Invoke("ReloadScene", respawnDelay);

        // Start the Respawn coroutine
        respawnCoroutine = StartCoroutine(Respawn());
    }

    // Coroutine for invincibility after being hit
    IEnumerator Invincibility()
    {
        // Set isInvincible to true
        isInvincible = true;

        // Wait for the specified beingHitCooldown duration
        yield return new WaitForSeconds(beingHitCooldown);

        // Set isInvincible to false and isHit to false
        isInvincible = false;
        isHit = false;
    }

    // Coroutine for handling respawn countdown
    IEnumerator Respawn()
    {
        // Initialize the respawnTimer to the respawnDelay
        float respawnTimer = respawnDelay;

        // Loop until the respawnTimer is greater than 0
        while (respawnTimer > 0)
        {
            // Update the respawnTimerText with the countdown
            respawnTimerText.text = "Respawning in... " + Mathf.CeilToInt(respawnTimer);

            // Wait for 1 second
            yield return new WaitForSeconds(1f);

            // Decrease the respawnTimer by 1
            respawnTimer -= 1f;
        }
        /*
        Debug.Log("Player has respawned!");
        currentHealth = maxHealth;
        transform.position = respawnPoint.position;
        panel.SetActive(false);
        playerMovement.SetActive(true);
        isDead = false; */ // Old respawn code
    }

    // Function to reload the current scene (hard reset)
    void ReloadScene()
    {
        // Get the index of the currently active scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the current scene
        SceneManager.LoadScene("Menu");
    }
}
