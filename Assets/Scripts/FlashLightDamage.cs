using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightDamage : MonoBehaviour
{
    public float damagePerSecond = 10f;

    private void OnTriggerStay(Collider other)
    {
        // Check if the collided object has an "Enemy" tag
        if (other.CompareTag("Enemy_Flying"))
        {
            // Apply damage over time based on the light intensity
            float damage = damagePerSecond * Time.deltaTime; //* GetComponent<Light>().intensity;
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }
}
