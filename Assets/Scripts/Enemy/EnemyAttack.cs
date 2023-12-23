using UnityEngine;
using System.Collections;
public class EnemyAttack : MonoBehaviour
{

    public float attackRange = 2f;
    public float attackDelay = 2f;
    public float animationDelay = 0.3f;
    public int attackAnimationAmount = 2;
    public GameObject rangedProjectilePrefab;
    public Transform rangedProjectileSpawn;
    public float projectileSpeed = 5f;

    private bool isTaunting = false;
    private bool isAttacking = false;
    [SerializeField] private bool meleeEnabled = true;
    [SerializeField] private bool rangedEnabled = false;

    private Transform player;
    private EnemyMovement enMove;
    private EnemyAnimatorHandler animatorHandler;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        enMove = GetComponent<EnemyMovement>();
        animatorHandler = GetComponent<EnemyAnimatorHandler>();
        attackDelay = attackDelay - animationDelay;
    }

    void Update()
    {
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

    private bool ShouldAttack()
    {
        if (player != null)
        {
            PlayerHat playerHat = player.GetComponentInChildren<PlayerHat>();
            if (playerHat != null && playerHat.isEquipped && playerHat.hatTag == "Santa")
            {
                return false;
            }
            animatorHandler.SetTauntEnd();
            return true;
        }

        return false;
    }

    private void MeleeAttack()
    {
        int randomAttack = Random.Range(1, attackAnimationAmount + 1);
        animatorHandler.SetAttack(randomAttack);
    }

    private void RangedAttack()
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

    private IEnumerator RangedAttackWithDelay()
    {
        isAttacking = true;
        enMove.toggleMove = false;
        animatorHandler.SetBattleIdle();

        int randomAttack = Random.Range(1, attackAnimationAmount + 1);
        animatorHandler.SetAttack(randomAttack);
        yield return new WaitForSeconds(animationDelay);
        RangedAttack();
        enMove.toggleMove = true;
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    public bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) < attackRange;
    }

    private void PerformTaunt()
    {
        if (!isTaunting)
        {
            StartCoroutine(TauntCooldown());
            isTaunting = true;
            animatorHandler.SetTaunt();
        }
    }

    private IEnumerator TauntCooldown()
    {
        yield return new WaitForSeconds(attackDelay);
        isTaunting = false;
    }
}