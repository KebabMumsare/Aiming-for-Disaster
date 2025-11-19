using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerXP : MonoBehaviour
{
    public float currentXP = 0f;
    public float currentLevel = 1f;
    public float xpToNextLevel = 100f;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI levelText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void AddXP(float xp)
    {
        currentXP += xp;
        xpText.text = "XP: " + currentXP + " / " + xpToNextLevel;
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = "LVL:" + currentLevel;
        if (currentXP >= xpToNextLevel)
        {
            currentLevel++;
            currentXP = currentXP - xpToNextLevel;
            xpToNextLevel *= 1.1f;
            xpText.text = currentXP + " / " + xpToNextLevel;
        }
    }
}
