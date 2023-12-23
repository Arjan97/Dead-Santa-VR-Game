// Script: EnemyAnimatorHandler
// Description: Manages the animation states and triggers for an enemy character.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorHandler : MonoBehaviour
{
    // Reference to the Animator component
    private Animator animator;

    // Start is called before the first frame update
    private void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    // Set the idle animation state
    public void SetIdle()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isChasing", false);
        animator.SetBool("isBattleIdle", false);
    }

    // Set the chasing animation state
    public void SetChasing()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isChasing", true);
        animator.SetBool("isBattleIdle", false);
    }

    // Set the battle idle animation state
    public void SetBattleIdle()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isChasing", false);
        animator.SetBool("isBattleIdle", true);
    }

    // Trigger the hit animation
    public void SetHit()
    {
        animator.SetTrigger("isHit");
    }

    // Set the dead animation state
    public void SetDead()
    {
        animator.SetBool("isDead", true);
    }

    // Trigger a specific attack animation
    public void SetAttack(int attackNumber)
    {
        animator.SetTrigger("Attack" + attackNumber);
    }

    // Set the taunt animation state
    public void SetTaunt()
    {
        animator.SetBool("isTaunting", true);
        animator.SetBool("isIdle", false);
        animator.SetBool("isChasing", false);
        animator.SetBool("isBattleIdle", false);
    }

    // End the taunt animation state
    public void SetTauntEnd()
    {
        animator.SetBool("isTaunting", false);
    }

    // Trigger the hitPlayer animation
    public void SetPlayerHitTrigger()
    {
        animator.SetTrigger("hitPlayer");
    }
}
