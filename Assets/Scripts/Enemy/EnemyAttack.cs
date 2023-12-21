using UnityEngine;
using System.Collections;
public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackDelay = 2f;
    public int attackAnimationAmount = 2;

    private bool isTaunting = false;
    private bool isAttacking = false;
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
                    StartCoroutine(AttackWithDelay());
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


    private void Attack()
    {
        int randomAttack = Random.Range(1, attackAnimationAmount + 1);
        animatorHandler.SetAttack(randomAttack);
    }

    private IEnumerator AttackWithDelay()
    {
        isAttacking = true;

        animatorHandler.SetBattleIdle();

        yield return new WaitForSeconds(attackDelay);

        Attack();

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