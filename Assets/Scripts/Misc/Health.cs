using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Health : MonoBehaviour
{

    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public float currentHealth;
    public bool isImmortal = false; // if ticked, you wont die when reaching 0 health
    public event Action<float, float> OnHealthChanged;
    private float baseMaxHealth;

    void Start()
    {
        baseMaxHealth = maxHealth;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void SetMaxHealthMultiplier(float multiplier)
    {
        float newMaxHealth = baseMaxHealth * multiplier;
        float healthPercent = currentHealth / maxHealth;
        maxHealth = newMaxHealth;
        currentHealth = maxHealth * healthPercent;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0f && isImmortal == false)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    public void Die()
    {
        if (CompareTag("Enemy"))
            {
                Destroy(gameObject);
                var player = GameObject.FindGameObjectWithTag("Player");
                var enemyController = GetComponent<EnemyBehaviorController>();

                if (player != null && enemyController != null)
                {
                    var currencies = player.GetComponent<Currencies>();
                    if (currencies != null)
                    {
                        currencies.AddBullets(enemyController.bulletsReward); // give bullet
                        currencies.AddMagazines(enemyController.magazinesReward); // give magazine
                    }

                    player.GetComponent<PlayerXP>()?.AddXP(enemyController.xpReward); // give xp
                }
            }
            if (CompareTag("Player"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        Debug.Log("Die");
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the object colliding with is an enemy or lootbox
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "LootBox")
        {
            Debug.Log("Hit enemy with " + other.gameObject.name);
            TakeDamage(other.gameObject.GetComponent<Damage>().GetDamage());
        }
    }
}
