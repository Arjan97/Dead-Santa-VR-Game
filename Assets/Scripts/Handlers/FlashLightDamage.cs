using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightDamage : MonoBehaviour
{
    public float damagePerSecond = 10f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy_Flying"))
        {
            float damage = damagePerSecond * Time.deltaTime;
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
            Vector3 dodgeDirection = Random.Range(0, 2) == 0 ? Vector3.right : Vector3.left;
            other.GetComponent<EnemyFlying>().Dodge(dodgeDirection);
        }
    }
}
