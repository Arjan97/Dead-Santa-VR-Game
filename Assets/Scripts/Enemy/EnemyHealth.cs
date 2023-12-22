using UnityEngine;
using System.Collections;
public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    public GameObject playerHatPrefab;
    public GameObject enemyHatPrefab;
    public float destroyEnemyObjDelay = 4f;
    [SerializeField] private float currentHealth;
    private bool isHitOnCooldown = false; 
    [SerializeField] private float hitCooldownDuration = 1f; 
    private EnemyAnimatorHandler animatorHandler;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        animatorHandler = GetComponent<EnemyAnimatorHandler>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }

        if (!isHitOnCooldown && !isDead) 
        {
            StartCoroutine(HitCooldown()); 
            animatorHandler.SetHit();
            Debug.Log("Enemt has been hit!");
           
        }
    }

    private IEnumerator HitCooldown()
    {
        isHitOnCooldown = true; 
        yield return new WaitForSeconds(hitCooldownDuration);

        isHitOnCooldown = false; 
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Enemy has died!");
       
        animatorHandler.SetDead();
        if (enemyHatPrefab != null)
        {
            StartCoroutine(HatDropPositioningDelay());

        }
        Destroy(gameObject, destroyEnemyObjDelay);
    }

    private IEnumerator HatDropPositioningDelay()
    {
        yield return new WaitForSeconds(destroyEnemyObjDelay -1);
        
        GameObject hat = Instantiate(playerHatPrefab, enemyHatPrefab.transform.position, Quaternion.identity);
        enemyHatPrefab.SetActive(false);
        
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}
