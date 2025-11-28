using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Health : MonoBehaviour
{

    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public float currentHealth;
    public bool isImmortal = false;
    public event Action<float, float> OnHealthChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float baseMaxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseMaxHealth = maxHealth;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void SetMaxHealthMultiplier(float multiplier)
    {
        float newMaxHealth = baseMaxHealth * multiplier;
        // Adjust current health proportionally or just increase max? 
        // Usually in games, increasing max health also heals for the amount gained or keeps percentage.
        // Let's keep percentage constant or just add the difference.
        // Simple approach: Increase max health, current health stays same (unless it was full?)
        // Better approach: Maintain health percentage.
        
        float healthPercent = currentHealth / maxHealth;
        maxHealth = newMaxHealth;
        currentHealth = maxHealth * healthPercent;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0f && isImmortal == false)
        {
            if (CompareTag("Enemy"))
            {
                Destroy(gameObject);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerXP>().AddXP(GetComponent<EnemyBehaviorController>().xpReward); // Add XP when enemy is killed - found in EnemyBehaviorController
            }
            if (CompareTag("Player"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
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
        Debug.Log("Die");
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "LootBox")
        {
            Debug.Log("Hit enemy with " + other.gameObject.name);
            TakeDamage(other.gameObject.GetComponent<Damage>().GetDamage());
        }
    }
}
