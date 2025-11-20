using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBarManager : MonoBehaviour
{
    public Image xpBar;
    public PlayerXP playerXP;

    void Update()
    {
        if (playerXP == null || xpBar == null)
        {
            return;
        }
        xpBar.fillAmount = playerXP.currentXP / playerXP.xpToNextLevel;
    }
}
