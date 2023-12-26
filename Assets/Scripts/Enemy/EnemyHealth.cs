// Script: EnemyHealth
// Description: Manages the health and behavior of an enemy, including taking damage, dying, and dropping a hat upon death.

using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    // Health parameters
    public float maxHealth = 50f;
    public GameObject playerHatPrefab;
    public GameObject enemyHatPrefab;
    public float destroyEnemyObjDelay = 4f;

    // Current health and hit cooldown parameters
    [SerializeField] private float currentHealth;
    private bool isHitOnCooldown = false;
    [SerializeField] private float hitCooldownDuration = 1f;

    // References to other components
    private EnemyAnimatorHandler animatorHandler;
    private bool isDead = false;
    private EnemyMovement enMove;
    private BattleChecker battleChecker;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        animatorHandler = GetComponent<EnemyAnimatorHandler>();
        enMove = GetComponent<EnemyMovement>();
        battleChecker = FindObjectOfType<BattleChecker>();
    }

    // Method for taking damage
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Check if the enemy should die
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }

        // Apply hit animation and cooldown
        if (!isHitOnCooldown && !isDead)
        {
            StartCoroutine(HitCooldown());
            animatorHandler.SetHit();
            Debug.Log("Enemy has been hit!");
        }
    }

    // Cooldown for being hit
    private IEnumerator HitCooldown()
    {
        enMove.toggleMove = false;
        isHitOnCooldown = true;
        yield return new WaitForSeconds(hitCooldownDuration);
        enMove.toggleMove = true;
        isHitOnCooldown = false;
    }

    // Method for handling death
    void Die()
    {
        isDead = true;
        Debug.Log("Enemy has died!");
        animatorHandler.SetDead();

        // Drop hat if specified
        if (enemyHatPrefab != null)
        {
            StartCoroutine(HatDropPositioningDelay());
        }
        // Destroy the enemy object after a delay
        battleChecker?.OnEnemyDestroyed();

        Destroy(gameObject, destroyEnemyObjDelay);
    }

    // Delay for positioning the dropped hat
    private IEnumerator HatDropPositioningDelay()
    {
        yield return new WaitForSeconds(destroyEnemyObjDelay - 1);

        // Instantiate and position the hat
        GameObject hat = Instantiate(playerHatPrefab, enemyHatPrefab.transform.position, Quaternion.identity);
        enemyHatPrefab.SetActive(false);
    }

    // Getter for the isDead flag
    public bool GetIsDead()
    {
        return isDead;
    }
}
