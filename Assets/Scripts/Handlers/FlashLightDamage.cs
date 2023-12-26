// Script: FlashLightDamage
// Description: Handles damage dealt by a flashlight to flying enemies within its trigger area.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightDamage : MonoBehaviour
{
    // Damage per second inflicted by the flashlight
    public float damagePerSecond = 10f;

    // Trigger callback when colliding with other colliders
    private void OnTriggerStay(Collider other)
    {
        // Check if the collider belongs to a flying enemy
        if (other.CompareTag("Enemy_Flying"))
        {
            // Calculate damage based on damage per second and time delta
            float damage = damagePerSecond * Time.deltaTime;

            // Deal damage to the flying enemy
            other.GetComponent<EnemyHealth>().TakeDamage(damage);

            SoundManager.Instance.PlaySoundAtPosition("flyhit", other.transform.position);
            /*
            // Determine a random dodge direction (right or left) for the flying enemy
            Vector3 dodgeDirection = Random.Range(0, 2) == 0 ? Vector3.right : Vector3.left;

            // Trigger dodge maneuver for the flying enemy
            other.GetComponent<EnemyFlying>().Dodge(dodgeDirection); */ //Left out because it's too buggy as of now
        }
    }
}
