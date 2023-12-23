// Script: EnemyFlying
// Description: Controls the flying behavior of an enemy, allowing it to hover or fall based on its health status. 
// Additionally, enables the enemy to perform a dodge maneuver.

using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    // Dodge force for the maneuver
    public float dodgeForce = 20f;

    // References to other components
    private EnemyHealth enHealth;
    private EnemyMovement enMove;
    private Rigidbody rb;
    private Vector3 originalPosition;

    // Start is called before the first frame update
    private void Start()
    {
        // Get references to components and add Rigidbody if not present
        enHealth = GetComponent<EnemyHealth>();
        enMove = GetComponent<EnemyMovement>();
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check enemy health status and apply hovering or falling accordingly
        if (!enHealth.GetIsDead())
        {
            Hover();
        }
        else
        {
            FallToGround();
        }
    }

    // Enable hovering behavior
    void Hover()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    // Enable falling behavior
    void FallToGround()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    // Perform a dodge maneuver in the specified direction
    public void Dodge(Vector3 dodgeDirection)
    {
        rb.isKinematic = false;
        enMove.toggleMove = false;
        originalPosition = transform.position;

        // Check if the enemy is not dead before performing the dodge
        if (!enHealth.GetIsDead())
        {
            Debug.Log("Dodge!");
            transform.Translate(dodgeDirection * dodgeForce * Time.deltaTime);
        }

        enMove.toggleMove = true;
    }
}
