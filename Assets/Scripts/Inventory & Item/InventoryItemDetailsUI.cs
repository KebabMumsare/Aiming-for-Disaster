using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemDetailsUI : MonoBehaviour
{
    [SerializeField] public GameObject panelRoot;
    [SerializeField] public TextMeshProUGUI itemNameText;
    [SerializeField] public TextMeshProUGUI typeText;
    [SerializeField] public TextMeshProUGUI stackText;
    [SerializeField] public TextMeshProUGUI actionText;
    [SerializeField] public TextMeshProUGUI descriptionText;
    [SerializeField] public TextMeshProUGUI statsText;

    public void Show(Item item, int stackCount)
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }

        if (itemNameText != null)
        {
            itemNameText.text = item != null ? item.name : string.Empty;
        }

        if (typeText != null)
        {
            typeText.text = item != null ? $"Type: {item.type}" : string.Empty;
        }

        if (stackText != null)
        {
            stackText.text = item != null && item.stackable
                ? $"Stack: {item.maxStack}"
                : "Stack: Not stackable";
        }

        if (actionText != null)
        {
            actionText.text = item != null ? $"Action: {item.actionType}" : string.Empty;
        }

        if (descriptionText != null)
        {
            descriptionText.text = item != null && !string.IsNullOrWhiteSpace(item.description)
                ? item.description
                : "No description set.";
        }

        if (statsText != null)
        {
            statsText.text = item != null ? BuildStatsText(item) : string.Empty;
        }
    }

    public void Hide()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
    }

    public string BuildStatsText(Item item)
    {
        StringBuilder builder = new StringBuilder();

        if (item.type == ItemType.Consumable && item.healAmount > 0f)
        {
            builder.AppendLine($"Heal Amount: {item.healAmount}");
        }

        if (item.extraStats != null && item.extraStats.Count > 0)
        {
            foreach (var stat in item.extraStats)
            {
                if (stat == null || string.IsNullOrWhiteSpace(stat.label))
                {
                    continue;
                }

                builder.AppendLine($"{stat.label}: {stat.value}");
            }
        }

        if (builder.Length == 0)
        {
            builder.Append("No stats available.");
        }

        return builder.ToString().TrimEnd();
    }
}

