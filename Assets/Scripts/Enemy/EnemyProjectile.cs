using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 10f;
    public float lifeTime = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.MovePosition(rb.position + (Vector2)transform.up * speed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacles") || other.gameObject.layer == LayerMask.NameToLayer("Default")) // Assuming default/obstacles block shots
        {
             // Optional: Add a check to not destroy on enemy collision if they share layers
             if (!other.CompareTag("Enemy"))
             {
                 Destroy(gameObject);
             }
        }
    }
}
