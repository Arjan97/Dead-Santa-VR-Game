using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageHandler : MonoBehaviour
{
    public float damageAmount = 10f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth healthComponent = collision.gameObject.GetComponent<EnemyHealth>();

            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damageAmount);
                Debug.Log("Enemy hit for: " + damageAmount);
            }

            Destroy(gameObject);
        }
    }
}
