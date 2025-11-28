using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerXP playerXP;
    [SerializeField] private PlayerMover2D playerMover;
    [SerializeField] private Health playerHealth;

    [Header("Upgrade Settings")]
    [SerializeField] private float speedMultiplierPerLevel = 0.1f; // 10% increase
    [SerializeField] private float healthMultiplierPerLevel = 0.1f; // 10% increase
    [SerializeField] private float healingMultiplierPerLevel = 0.2f; // 20% increase

    [Header("Current Levels")]
    public int speedLevel = 0;
    public int healthLevel = 0;
    public int healingLevel = 0;

    private float baseMoveSpeed;
    private float baseMaxHealth;

    private void Start()
    {
        if (playerXP == null) playerXP = GetComponent<PlayerXP>();
        if (playerMover == null) playerMover = GetComponent<PlayerMover2D>();
        if (playerHealth == null) playerHealth = GetComponent<Health>();

        // Store base values if possible, or assume current values are base
        // For PlayerMover2D, we might need to access moveSpeed directly. 
        // Since PlayerMover2D.moveSpeed is private/serialized, we might need to make it public or add a method.
        // For now, let's assume we can modify it or add a method to PlayerMover2D.
        // Wait, I need to check PlayerMover2D again to see if I can access moveSpeed.
    }

    public void UpgradeSpeed()
    {
        if (playerXP.skillPoints > 0)
        {
            playerXP.skillPoints--;
            playerXP.CheckLevel(); // Update UI
            speedLevel++;
            ApplySpeedUpgrade();
        }
    }

    public void UpgradeHealth()
    {
        if (playerXP.skillPoints > 0)
        {
            playerXP.skillPoints--;
            playerXP.CheckLevel(); // Update UI
            healthLevel++;
            ApplyHealthUpgrade();
        }
    }

    public void UpgradeHealing()
    {
        if (playerXP.skillPoints > 0)
        {
            playerXP.skillPoints--;
            playerXP.CheckLevel(); // Update UI
            healingLevel++;
        }
    }

    private void ApplySpeedUpgrade()
    {
        // We need to modify PlayerMover2D to allow speed changes
        // For now, I will add a public method to PlayerMover2D or make moveSpeed public.
        // Let's assume I will add SetMoveSpeedMultiplier or similar.
        float multiplier = 1f + (speedLevel * speedMultiplierPerLevel);
        playerMover.SetSpeedMultiplier(multiplier);
    }

    private void ApplyHealthUpgrade()
    {
        float multiplier = 1f + (healthLevel * healthMultiplierPerLevel);
        // We need to modify Health to allow maxHealth changes
        playerHealth.SetMaxHealthMultiplier(multiplier);
    }

    public float GetHealingMultiplier()
    {
        return 1f + (healingLevel * healingMultiplierPerLevel);
    }

    public float GetMoveSpeed()
    {
        if (playerMover != null)
            return playerMover.CurrentMoveSpeed;
        return 0f;
    }

    public float GetMaxHealth()
    {
        if (playerHealth != null)
            return playerHealth.maxHealth;
        return 0f;
    }
}
