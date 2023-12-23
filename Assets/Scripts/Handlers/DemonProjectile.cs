// Script: DemonProjectile
// Description: Handles the behavior of a projectile launched by a demon enemy, causing damage to the player upon collision.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonProjectile : MonoBehaviour
{
    // Damage amount inflicted by the projectile
    public float damageAmount = 20f;

    // Reference to the enemy shooting the projectile
    private GameObject shootingEnemy;

    // Triggered upon collision with other colliders
    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Get the player's health component
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            // Deal damage to the player if the health component is present
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }

            // Trigger player hit animation on the shooting enemy if available
            EnemyAnimatorHandler enemyAnimator = shootingEnemy?.GetComponent<EnemyAnimatorHandler>();

            if (enemyAnimator != null)
            {
                enemyAnimator.SetPlayerHitTrigger();
            }

            // Destroy the projectile after hitting the player
            Destroy(gameObject);
        }
    }

    // Set the shooting enemy reference
    public void SetShootingEnemy(GameObject enemy)
    {
        shootingEnemy = enemy;
    }
}
