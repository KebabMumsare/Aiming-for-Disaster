using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTreeUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private GameObject panelRoot;

    [Header("Speed Upgrade")]
    [SerializeField] private Button speedUpgradeButton;
    [SerializeField] private TextMeshProUGUI speedLevelText;
    [SerializeField] private TextMeshProUGUI speedCostText;
    [SerializeField] private TextMeshProUGUI speedValueText;

    [Header("Health Upgrade")]
    [SerializeField] private Button healthUpgradeButton;
    [SerializeField] private TextMeshProUGUI healthLevelText;
    [SerializeField] private TextMeshProUGUI healthCostText;
    [SerializeField] private TextMeshProUGUI healthValueText;

    [Header("Healing Upgrade")]
    [SerializeField] private Button healingUpgradeButton;
    [SerializeField] private TextMeshProUGUI healingLevelText;
    [SerializeField] private TextMeshProUGUI healingCostText;
    [SerializeField] private TextMeshProUGUI healingValueText;

    [Header("General")]
    [SerializeField] private PlayerXP playerXP;

    private void Start()
    {
        if (skillManager == null)
        {
            // Try to find on player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                skillManager = player.GetComponent<SkillManager>();
                playerXP = player.GetComponent<PlayerXP>();
            }
        }

        if (skillManager == null)
        {
            Debug.LogError("SkillManager not found!");
            return;
        }

        // Add listeners
        speedUpgradeButton.onClick.AddListener(OnSpeedUpgradeClicked);
        healthUpgradeButton.onClick.AddListener(OnHealthUpgradeClicked);
        healingUpgradeButton.onClick.AddListener(OnHealingUpgradeClicked);

        UpdateUI();
    }

    private void Update()
    {
        // Update UI every frame or use events? For simplicity, update on enable or when points change.
        // But since points change elsewhere, let's just update in Update for now or check if we can optimize.
        // Optimization: Only update when panel is open.
        if (panelRoot.activeSelf)
        {
            UpdateUI();
        }
    }

    private void OnSpeedUpgradeClicked()
    {
        skillManager.UpgradeSpeed();
        UpdateUI();
    }

    private void OnHealthUpgradeClicked()
    {
        skillManager.UpgradeHealth();
        UpdateUI();
    }

    private void OnHealingUpgradeClicked()
    {
        skillManager.UpgradeHealing();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (skillManager == null || playerXP == null) return;

        if (speedLevelText != null)
            speedLevelText.text = $"Lvl: {skillManager.speedLevel}";
        
        if (healthLevelText != null)
            healthLevelText.text = $"Lvl: {skillManager.healthLevel}";
        
        if (healingLevelText != null)
            healingLevelText.text = $"Lvl: {skillManager.healingLevel}";

        // Costs are always 1 SP for now
        if (speedCostText != null)
            speedCostText.text = "Cost: 1 SP";
        
        if (healthCostText != null)
            healthCostText.text = "Cost: 1 SP";
        
        if (healingCostText != null)
            healingCostText.text = "Cost: 1 SP";

        // Update Stat Values
        if (speedValueText != null)
            speedValueText.text = $"Move Speed: {skillManager.GetMoveSpeed():F1}";

        if (healthValueText != null)
            healthValueText.text = $"Max Health: {skillManager.GetMaxHealth():F0}";

        if (healingValueText != null)
            healingValueText.text = $"Heal Multiplier: {skillManager.GetHealingMultiplier():F1}x";

        // Enable/Disable buttons based on points
        bool canAfford = playerXP.skillPoints > 0;
        
        if (speedUpgradeButton != null)
            speedUpgradeButton.interactable = canAfford;
        
        if (healthUpgradeButton != null)
            healthUpgradeButton.interactable = canAfford;
        
        if (healingUpgradeButton != null)
            healingUpgradeButton.interactable = canAfford;
    }

    public void ToggleVisibility()
    {
        panelRoot.SetActive(!panelRoot.activeSelf);
    }
}
