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
        healthBar.fillAmount = health.currentHealth / 100;
        healthText.text = health.currentHealth.ToString("0") + "/" + health.maxHealth.ToString("0");
    }
}
