using UnityEngine;
using System.Collections;
public class EnemyAttack : MonoBehaviour
{

    public float attackRange = 2f;
    public float attackDelay = 2f;
    public int attackAnimationAmount = 2;
    public GameObject rangedProjectilePrefab; 
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
    }

    void Update()
    {
        if (player != null)
        {
            if (ShouldAttack())
            {
                if (IsPlayerInRange() && !isAttacking)
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
                else if (!isAttacking)
                {
                    animatorHandler.SetChasing();
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
            GameObject projectile = Instantiate(rangedProjectilePrefab, transform.position, Quaternion.identity);
            DemonProjectile projectileScript = projectile.GetComponent<DemonProjectile>();
            if (projectileScript != null)
            {
                projectileScript.SetShootingEnemy(gameObject);
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

        int randomAttack = Random.Range(1, attackAnimationAmount + 1);
        animatorHandler.SetAttack(randomAttack);
    }

    private IEnumerator MeleeAttackWithDelay()
    {
        isAttacking = true;

        animatorHandler.SetBattleIdle();

        yield return new WaitForSeconds(attackDelay);

        MeleeAttack();

        isAttacking = false;
    }

    private IEnumerator RangedAttackWithDelay()
    {
        isAttacking = true;

        animatorHandler.SetBattleIdle();

        yield return new WaitForSeconds(attackDelay);

        RangedAttack();

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