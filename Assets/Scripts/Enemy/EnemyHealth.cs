using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    [SerializeField] private float currentHealth;

    private EnemyAnimatorHandler animatorHandler;

    void Start()
    {
        currentHealth = maxHealth;
        animatorHandler = GetComponent<EnemyAnimatorHandler>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        animatorHandler.SetHit();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animatorHandler.SetDead();

        Destroy(gameObject, 4f);
    }
}
