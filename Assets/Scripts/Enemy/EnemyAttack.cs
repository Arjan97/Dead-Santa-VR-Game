// Script: EnemyAttack
// Description: Manages enemy attacks, including melee and ranged attacks. Determines when to attack based on player proximity and conditions.

using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    // Attack parameters
    public float attackRange = 2f;
    public float attackDelay = 2f;
    public float animationDelay = 0.3f;
    public int attackAnimationAmount = 2;
    public GameObject rangedProjectilePrefab;
    public Transform rangedProjectileSpawn;
    public float projectileSpeed = 5f;

    // Flags for attack state
    private bool isTaunting = false;
    private bool isAttacking = false;

    [SerializeField] private bool meleeEnabled = true;
    [SerializeField] private bool rangedEnabled = false;

    // Player and other components references
    private Transform player;
    private EnemyMovement enMove;
    private EnemyHealth enHealth;
    private EnemyAnimatorHandler animatorHandler;

    void Start()
    {
        // Find the player, get components, and adjust attack delay
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        enMove = GetComponent<EnemyMovement>();
        enHealth = GetComponent<EnemyHealth>();
        animatorHandler = GetComponent<EnemyAnimatorHandler>();
        attackDelay = attackDelay - animationDelay;
    }

    void Update()
    {
        // Check if player is present and handle attacks
        if (player != null)
        {
            if (ShouldAttack())
            {
                if (player.GetComponent<PlayerHealth>().isDead)
                {
                    PerformTaunt();
                }
                else if (IsPlayerInRange() && !isAttacking)
                {
                    if (meleeEnabled)
                    {
                        StartCoroutine(MeleeAttackWithDelay());
                    }
                    else if (rangedEnabled)
                    {
                        StartCoroutine(RangedAttackWithDelay());
                    }
                }
            }
            else
            {
                PerformTaunt();
            }
        }
    }

    // Check if enemy should attack based on player's hat type
    private bool ShouldAttack()
    {
        if (player != null)
        {
            PlayerHat playerHat = player.GetComponentInChildren<PlayerHat>();
            EnemyHealth enemyHat = GetComponent<EnemyHealth>();
            // Check if the enemy has a hat prefab before accessing its tag
            if (playerHat != null && playerHat.isEquipped && enemyHat != null && enemyHat.enemyHatPrefab != null && playerHat.hatTag == enemyHat.enemyHatPrefab.tag)
            {
                return false;
            }
            animatorHandler.SetTauntEnd();
            if (playerHat != null)
            {
                Debug.Log("should attack?" + playerHat.hatTag + " =/ " + enemyHat.tag);

            }
            return true;
        }

        return false;
    }

    // Perform a melee attack
    private void MeleeAttack()
    {
        if (!enHealth.GetIsDead())
        {
            int randomAttack = Random.Range(1, attackAnimationAmount + 1);
            animatorHandler.SetAttack(randomAttack);

            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }
    }

    // Perform a ranged attack
    private void RangedAttack()
    {
        if (!enHealth.GetIsDead())
        {
            if (rangedProjectilePrefab != null)
            {
                GameObject projectile = Instantiate(rangedProjectilePrefab, rangedProjectileSpawn.transform.position, Quaternion.identity);
                DemonProjectile projectileScript = projectile.GetComponent<DemonProjectile>();
                if (projectileScript != null)
                {
                    projectileScript.SetShootingEnemy(gameObject);
                    Destroy(projectile, 3f);
                }
                else
                {
                    Debug.LogWarning("DemonProjectile component not found on the projectile prefab.");
                }
                Vector3 direction = (player.position - transform.position).normalized;

                Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
                if (projectileRigidbody != null)
                {
                    projectileRigidbody.velocity = direction * projectileSpeed;
                }
                else
                {
                    Debug.LogWarning("Rigidbody component not found on the projectile prefab.");
                }
            }
            else
            {
                Debug.LogWarning("Ranged projectile prefab not assigned in the inspector.");
            }
        }
    }

    // Coroutine for delaying melee attack
    private IEnumerator MeleeAttackWithDelay()
    {
        isAttacking = true;
        enMove.toggleMove = false;
        animatorHandler.SetBattleIdle();
        MeleeAttack();
        yield return new WaitForSeconds(attackDelay);
        enMove.toggleMove = true;
        isAttacking = false;
    }

    // Coroutine for delaying ranged attack
    private IEnumerator RangedAttackWithDelay()
    {
        isAttacking = true;
        enMove.toggleMove = false;
        animatorHandler.SetBattleIdle();

        int randomSound = Random.Range(1, 4);
        SoundManager.Instance.PlaySoundAtPosition("Imp_" + randomSound, transform.position);

        int randomAttack = Random.Range(1, attackAnimationAmount + 1);
        animatorHandler.SetAttack(randomAttack);
        yield return new WaitForSeconds(animationDelay);
        RangedAttack();
        enMove.toggleMove = true;
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    // Check if player is within attack range
    public bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) < attackRange;
    }

    // Perform taunt if not already taunting
    private void PerformTaunt()
    {
        if (!isTaunting)
        {
            StartCoroutine(TauntCooldown());
            isTaunting = true;
            animatorHandler.SetTaunt();
        }
    }

    // Cooldown for taunt
    private IEnumerator TauntCooldown()
    {
        yield return new WaitForSeconds(attackDelay);
        isTaunting = false;
    }
}
