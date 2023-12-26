// Script: BulletDamageHandler
// Description: Manages the damage dealt by a bullet upon collision with enemy objects, triggering appropriate actions.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageHandler : MonoBehaviour
{
    // Damage amount inflicted by the bullet
    public float damageAmount = 10f;
    // Indicates whether the bullet is a rock
    public bool isRock = false;
    public float minDamageVelocity = 4f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Callback when the bullet collides with another object
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Check if the velocity is above the minimum required velocity
            if (rb != null && rb.velocity.magnitude >= minDamageVelocity)
            {
                // Get the enemy's health component
                EnemyHealth healthComponent = collision.gameObject.GetComponent<EnemyHealth>();

                // Deal damage to the enemy if the health component is present
                if (healthComponent != null)
                {
                    healthComponent.TakeDamage(damageAmount);
                    Debug.Log("Enemy hit for: " + damageAmount);
                }
            }

            // Destroy the bullet after hitting an enemy (if it's not a rock)
            if (!isRock)
            {
                Destroy(gameObject);
            }
        }

        // Check if the collision involves a flying enemy
        if (collision.gameObject.CompareTag("Enemy_Flying"))
        {
            // Determine a random dodge direction (right or left)
            Vector3 dodgeDirection = Random.Range(0, 2) == 0 ? Vector3.right : Vector3.left;

            // Get the flying enemy component
            EnemyFlying flyer = collision.gameObject.GetComponent<EnemyFlying>();

            // Trigger dodge maneuver and destroy the bullet
            flyer.Dodge(dodgeDirection);
            Destroy(gameObject);
        }
    }
}
