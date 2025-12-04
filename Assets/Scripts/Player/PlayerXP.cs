using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerXP : MonoBehaviour
{
    public float currentXP = 0f;
    public float currentLevel = 1f;
    [HideInInspector] public float xpToNextLevel = 100f;
    public int skillPoints = 1;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI skillPointsText;

    private float xpMultiplier = 1f;

    public void SetXPMultiplier(float multiplier)
    {
        xpMultiplier = multiplier;
    }
    
    void Start()
    {
        // check if levelText, skillPointsText and xpText are assigned
        if (levelText == null || skillPointsText == null || xpText == null)
        {
            Debug.LogError("LevelText or SkillPointsText or XPText is not assigned. Assign them on the Player");
            return;
        }

        levelText.text = "LVL:" + currentLevel;
        skillPointsText.text = "SP:" + skillPoints;
        xpText.text = "XP: " + currentXP + " / " + xpToNextLevel;
    }

    public void AddXP(float xp) // Add XP - Used in EnemyBehaviorController
    {
        currentXP += xp * xpMultiplier;
        xpText.text = "XP: " + currentXP + " / " + xpToNextLevel;
        CheckLevel();
    }

    // Check if XP is enough to level up and also check skillpoints
    public void CheckLevel() {
        if (currentXP >= xpToNextLevel)
        {
            currentLevel++;
            currentXP = currentXP - xpToNextLevel;
            xpToNextLevel *= 1.1f;
            xpToNextLevel = Mathf.Round(xpToNextLevel); 
            xpText.text = currentXP + " / " + xpToNextLevel;
            skillPoints++;
            if (currentXP >= xpToNextLevel) {
                CheckLevel();
            }
        }
        levelText.text = "LVL:" + currentLevel;
        skillPointsText.text = "SP:" + skillPoints;
    }

    void Update()
    {
        
    }
}
