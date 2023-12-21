using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageHandler : MonoBehaviour
{
    public float damageAmount = 10f;

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "Enemy" tag
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Attempt to get the Health script from the collided object
            EnemyHealth healthComponent = collision.gameObject.GetComponent<EnemyHealth>();

            // If the Health script is found, apply damage
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damageAmount);
            }

            // Destroy the bullet after hitting an enemy
            Destroy(gameObject);
        }
    }
}
