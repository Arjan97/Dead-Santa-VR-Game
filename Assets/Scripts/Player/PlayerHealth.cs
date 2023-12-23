using UnityEngine;
using System.Collections;
public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float beingHitCooldown = 2f;
    public bool isHit = false;

    [SerializeField] private bool isInvincible = false;
    [SerializeField] private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            isHit = true;
            if (currentHealth <= 0)
            {
                Die();
            }
            StartCoroutine(Invincibility());
        }
    }

    IEnumerator Invincibility()
        {
        isInvincible = true;
        yield return new WaitForSeconds(beingHitCooldown);
        isInvincible = false;
        isHit = false;

    }
    void Die()
    {
        Debug.Log("Player has died!");
    }
}
