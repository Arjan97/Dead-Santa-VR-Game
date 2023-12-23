using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonProjectile : MonoBehaviour
{
    public float damageAmount = 15f;

    private GameObject shootingEnemy;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }

            EnemyAnimatorHandler enemyAnimator = shootingEnemy?.GetComponent<EnemyAnimatorHandler>();

            if (enemyAnimator != null)
            {
                enemyAnimator.SetPlayerHitTrigger();
            }

            Destroy(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void SetShootingEnemy(GameObject enemy)
    {
        shootingEnemy = enemy;
    }
}
