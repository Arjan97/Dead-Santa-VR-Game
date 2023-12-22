using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    public float hoverHeight = 10f;
    public float rotationForce = 5f; 
    private EnemyHealth enHealth;
    private Rigidbody rb;

    private void Start()
    {
        enHealth = GetComponent<EnemyHealth>();
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
}
