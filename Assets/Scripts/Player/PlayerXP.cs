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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelText.text = "LVL:" + currentLevel;
    }

    public void AddXP(float xp)
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
            xpText.text = currentXP + " / " + xpToNextLevel;
            skillPoints++;
        }
        levelText.text = "LVL:" + currentLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
