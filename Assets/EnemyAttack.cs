using UnityEngine;
using System.Collections;
public class EnemyAttack : MonoBehaviour
{
    public string playerTag = "Player";
    public float attackRange = 2f;
    public float attackDelay = 2f;
    public int attackAnimationAmount = 2;

    private bool isAttacking = false;
    private Transform player;
    private EnemyMovement enMove;
    private EnemyAnimatorHandler animatorHandler;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;
        enMove = GetComponent<EnemyMovement>();
        animatorHandler = GetComponent<EnemyAnimatorHandler>();
    }

    void Update()
    {
        if (player != null)
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
}
