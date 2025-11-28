using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Health health;
    [SerializeField] private Image healthBarFill;

    private void Start()
    {
        // Try to find Health component in parent if not assigned
        if (health == null)
        {
            health = GetComponentInParent<Health>();
        }

        if (health != null)
        {
            health.OnHealthChanged += UpdateHealthBar;
            // Initialize
            UpdateHealthBar(health.currentHealth, health.maxHealth);
        }
        else
        {
            Debug.LogError("Health component not found for EnemyHealthBar on " + gameObject.name);
        }
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBarFill != null)
        {
            float fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
            healthBarFill.fillAmount = fillAmount;
        }
    }

    private void LateUpdate()
    {
        // Optional: Make the health bar face the camera
        // transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        // Since it's 2D, we probably don't need LookAt unless the canvas is in World Space and rotating?
        // In 2D, World Space Canvases usually just stay flat. 
        // If the enemy rotates, the canvas will rotate with it. 
        // To keep it upright, we can force rotation to identity.
        transform.rotation = Quaternion.identity;
    }
}
