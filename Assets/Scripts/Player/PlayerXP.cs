using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerXP : MonoBehaviour
{
    public float currentXP = 0f;
    public float currentLevel = 1f;
    public float xpToNextLevel = 100f;
    public int skillPoints = 1;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI levelText;
    
    
    void Start()
    {
        levelText.text = "LVL:" + currentLevel;
    }

    public void AddXP(float xp) // Add XP - Used in EnemyBehaviorController
    {
        currentXP += xp;
        xpText.text = "XP: " + currentXP + " / " + xpToNextLevel;
        CheckLevel();
    }

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
    }

    void Update()
    {
        
    }
}
