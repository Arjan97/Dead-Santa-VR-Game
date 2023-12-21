using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorHandler : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetIdle()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isChasing", false);
        animator.SetBool("isBattleIdle", false);
    }

    public void SetChasing()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isChasing", true);
        animator.SetBool("isBattleIdle", false);
    }

    public void SetBattleIdle()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isChasing", false);
        animator.SetBool("isBattleIdle", true);
    }

    public void SetHit()
    {
        animator.SetTrigger("isHit");
    }

    public void SetDead()
    {
        animator.SetBool("isDead", true);
    }

    public void SetAttack(int attackNumber)
    {
        animator.SetTrigger("Attack" + attackNumber);
    }
}
