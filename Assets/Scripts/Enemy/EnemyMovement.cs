// Script: EnemyMovement
// Description: Handles the movement behavior of an enemy, including chasing the player within a specified range.

using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Player-related parameters
    public string playerTag = "Player";
    public float chaseRange = 10f;

    // Movement parameters
    public float movementSpeed = 3f;
    public float rotationSpeed = 3f;
    public bool toggleMove = true; //To temp disable movement when hit etc.

    // Player and component references
    private Transform player;
    private EnemyAnimatorHandler animatorHandler;
    private EnemyAttack enAttack;
    private EnemyHealth enHealth;
    
    // Dynamic sounds
    private float soundTimer;
    private float minSoundDelay = 4f;
    private float maxSoundDelay = 10f;

    void Start()
    {
        // Find the player and get component references
        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;
        animatorHandler = GetComponent<EnemyAnimatorHandler>();
        enAttack = GetComponent<EnemyAttack>();
        enHealth = GetComponent<EnemyHealth>();
        ResetSoundTimer();
    }

    void Update()
    {
        // Check if the player is present
        if (player != null && !enHealth.GetIsDead()) //Added check for dead, fixed bug where dead enemies would still chase
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Move towards the player if within chase range and not in attack range
            if (distanceToPlayer <= chaseRange && !enAttack.IsPlayerInRange() && toggleMove)
            {
                MoveTowardsPlayer();
            }
            // Idle if player is out of chase range
            else if (!enAttack.IsPlayerInRange() && distanceToPlayer >= chaseRange && toggleMove)
            {
                Idling();
            }
        }
    }

    // Move towards the player
    public void MoveTowardsPlayer()
    {
        // Set chasing animation state
        animatorHandler.SetChasing();

        // Calculate direction and rotation to face the player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Move forward towards the player
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

        if (gameObject.tag == "Enemy")
        {
            // Check if it's time to play a sound
            if (Time.time >= soundTimer)
            {
                PlayChaseSound("Zombie_Moan_", 3, 8);

                // Reset the timer with a random interval
                ResetSoundTimer();
            }
        }

        if (gameObject.tag == "EnemyBoss")
        {
            // Check if it's time to play a sound
            if (Time.time >= soundTimer)
            {
                PlayChaseSound("Big_Monster_", 1, 3);

                // Reset the timer with a random interval
                ResetSoundTimer();
            }
        }
    }
    void PlayChaseSound(string audioClip, int minRange, int maxRange)
    {
        int randomSound = Random.Range(minRange, maxRange);
        SoundManager.Instance.PlaySoundAtPosition(audioClip + randomSound, transform.position);
    }

    void ResetSoundTimer()
    {
        // Set a new random interval for the next sound
        soundTimer = Time.time + Random.Range(minSoundDelay, maxSoundDelay);
    }

    // Set idle animation state
    private void Idling()
    {
        animatorHandler.SetIdle();
    }
}
