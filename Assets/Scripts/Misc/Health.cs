using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public float currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            if (CompareTag("Enemy"))
            {
                Destroy(gameObject);
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
    }
    
    public void Die()
    {
        Debug.Log("Die");
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit enemy with " + other.gameObject.name);
            TakeDamage(other.gameObject.GetComponent<Damage>().GetDamage());
        }
    }
}
