using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public string playerTag = "Player";
    public float chaseRange = 10f;
    public float movementSpeed = 3f;
    public float rotationSpeed = 3f;

    private Transform player;
    private EnemyAnimatorHandler animatorHandler;
    private EnemyAttack enAttack;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;
        animatorHandler = GetComponent<EnemyAnimatorHandler>();
        enAttack = GetComponent<EnemyAttack>();
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= chaseRange && !enAttack.IsPlayerInRange())
            {
                MoveTowardsPlayer();
            }
            else if (!enAttack.IsPlayerInRange() && distanceToPlayer >= chaseRange)
            {
                Idling();
            }
        }
    }

    public void MoveTowardsPlayer()
    {
        animatorHandler.SetChasing();

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }

    private void Idling()
    {
        animatorHandler.SetIdle();
    }
}
