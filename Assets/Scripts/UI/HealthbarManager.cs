using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarManager : MonoBehaviour
{
    public Image healthBar;
    public Health health; 
    public TextMeshProUGUI healthText;
    
    void Update()
    {
        if (health == null || healthBar == null)
        {
            return;
        }

        float normalizedHealth = 0f;
        if (health.maxHealth > 0f)
        {
            normalizedHealth = Mathf.Clamp01(health.currentHealth / health.maxHealth);
        }

        healthBar.fillAmount = normalizedHealth;

        if (healthText != null)
        {
            healthText.text = health.currentHealth.ToString("0") + "/" + health.maxHealth.ToString("0");
        }
    }
}
