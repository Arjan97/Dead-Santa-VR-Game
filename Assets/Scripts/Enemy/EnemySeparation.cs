using UnityEngine;

public class EnemySeparation : MonoBehaviour
{
    public float separationDistance = 2f;
    public LayerMask enemyLayer;

    private void Update()
    {
        SeparateFromOtherEnemies();
    }

    void SeparateFromOtherEnemies()
    {
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, separationDistance, enemyLayer);

        foreach (Collider enemyCollider in nearbyEnemies)
        {
            if (enemyCollider.gameObject != gameObject) 
            {
                Vector3 separationDirection = transform.position - enemyCollider.transform.position;
                float distance = separationDirection.magnitude;

                if (distance < separationDistance)
                {
                    // Adjust the position to separate from the other enemy
                    Vector3 separationVector = separationDirection.normalized * (separationDistance - distance);
                    transform.position += separationVector;
                }
            }
        }
    }
}
