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
    public bool toggleMove = true;

    // Player and component references
    private Transform player;
    private EnemyAnimatorHandler animatorHandler;
    private EnemyAttack enAttack;

    // Start is called before the first frame update
    void Start()
    {
        // Find the player and get component references
        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;
        animatorHandler = GetComponent<EnemyAnimatorHandler>();
        enAttack = GetComponent<EnemyAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is present
        if (player != null)
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
    }

    // Set idle animation state
    private void Idling()
    {
        animatorHandler.SetIdle();
    }
}
