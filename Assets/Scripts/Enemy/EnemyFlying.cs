using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    public float dodgeForce = 20f;

    private EnemyHealth enHealth;
    private EnemyMovement enMove;
    private Rigidbody rb;
    private Vector3 originalPosition;

    private void Start()
    {
        enHealth = GetComponent<EnemyHealth>();
        enMove =  GetComponent<EnemyMovement>();
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        if (!enHealth.GetIsDead())
        {
            Hover();
        }
        else
        {
            FallToGround();
        }
    }

    void Hover()
    {
        rb.useGravity = false; 
        rb.isKinematic = true; 
    }

    void FallToGround()
    {
        rb.useGravity = true; 
        rb.isKinematic = false; 
    }

    public void Dodge(Vector3 dodgeDirection)
    {
        rb.isKinematic = false;
        enMove.toggleMove = false;
        originalPosition = transform.position;
        if (!enHealth.GetIsDead())
        {
            Debug.Log("Dodge!");
            transform.Translate(dodgeDirection * dodgeForce * Time.deltaTime); 
        }
        enMove.toggleMove = true;
    }
}
