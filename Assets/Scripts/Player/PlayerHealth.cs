using UnityEngine;
using System.Collections;
using TMPro;
public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float beingHitCooldown = 2f;
    public bool isHit = false;
    public bool isDead = false;

    public Transform respawnPoint;
    public float respawnDelay = 5f;
    public TextMeshProUGUI respawnText;
    public TextMeshProUGUI respawnTimerText;
    public GameObject panel;
    public GameObject playerMovement;
    [SerializeField] private bool isInvincible = false;
    [SerializeField] private float currentHealth;
    private Coroutine respawnCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        panel.SetActive(false);
        playerMovement.SetActive(true);
    }

    public void TakeDamage(float damage)
    {
        if (!isInvincible && !isDead)
        {
            currentHealth -= damage;
            Debug.Log("Player got damaged for: " + damage);
            isHit = true;
            if (currentHealth <= 0)
            {
                Die();
            }
            StartCoroutine(Invincibility());
        }
    }

    void Die()
    {
        Debug.Log("Player has died!");
        panel.SetActive(true);
        playerMovement.SetActive(false);

        isDead = true;
        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
        }

        respawnCoroutine = StartCoroutine(Respawn());
    }

    IEnumerator Invincibility()
        {
        isInvincible = true;
        yield return new WaitForSeconds(beingHitCooldown);
        isInvincible = false;
        isHit = false;

    }

    IEnumerator Respawn()
    {
        float respawnTimer = respawnDelay;

        while (respawnTimer > 0)
        {
            respawnTimerText.text = "Respawning in... " + Mathf.CeilToInt(respawnTimer);
            yield return new WaitForSeconds(1f);
            respawnTimer -= 1f;
        }

        Debug.Log("Player has respawned!");
        currentHealth = maxHealth;
        transform.position = respawnPoint.position;
        panel.SetActive(false);
        playerMovement.SetActive(true);
        isDead = false;
    }
}
