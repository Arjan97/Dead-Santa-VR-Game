// Script: EnemyWeaponDamageHandler
// Description: Handles the damage dealt by an enemy weapon when colliding with the player.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponDamageHandler : MonoBehaviour
{
    // Damage amount dealt by the enemy weapon
    public int damageAmount = 10;

    // Trigger callback when colliding with other colliders
    private void OnTriggerEnter(Collider other)
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
        }
    }
}
