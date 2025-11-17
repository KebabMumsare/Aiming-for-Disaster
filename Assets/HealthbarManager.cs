using UnityEngine;
using UnityEngine.UI;

public class HealthbarManager : MonoBehaviour
{
    public Image healthBar;
    public Health health; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health.currentHealth / 100;
    }
}
