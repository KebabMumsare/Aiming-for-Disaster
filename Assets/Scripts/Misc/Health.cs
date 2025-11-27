using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        // If this is an enemy, award XP to player and give currency / call Enemydrop
        if (CompareTag("Enemy"))
        {
            // Award XP (do this before the enemy GameObject is destroyed)
            var player = GameObject.FindGameObjectWithTag("Player");
            var ebc = GetComponent<EnemyBehaviorController>();
            if (player != null && ebc != null)
            {
                var pxp = player.GetComponent<PlayerXP>();
                if (pxp != null) pxp.AddXP(ebc.xpReward);
            }

            // Try to use Enemydrop if present (it will handle giving currency and destroying the enemy)
            var drop = GetComponent<Enemydrop>();
            if (drop != null)
            {
                drop.Die();
            }
            else
            {
                // fallback: add a default amount and destroy
                Currency.AddAmount(10);
                Destroy(gameObject);
            }

            Die();
            return;
        }

        // If this is the player, reload the scene
        if (CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Die();
            return;
        }

        // Generic fallback for other objects
        Destroy(gameObject);
        Die();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} died.");
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit enemy with " + other.gameObject.name);
            var dmg = other.gameObject.GetComponent<Damage>()?.GetDamage() ?? 0f;
            TakeDamage(dmg);
        }
    }
}
