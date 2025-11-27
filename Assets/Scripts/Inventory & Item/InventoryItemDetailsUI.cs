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
    [SerializeField] public Image itemImage;

    public void Show(Item item, int stackCount)
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }

        if (itemNameText != null)
        {
            itemNameText.text = item != null ? $"{item.name}" : string.Empty;
        }

        if (typeText != null)
        {
            typeText.text = item != null ? $"{item.type}" : string.Empty;
        }

        if (item.stackable == false)
        {
            stackText.text = string.Empty;
        }

        if (item.stackable == true)
        {
            stackText.text = item != null ? $"Max Stack: {item.maxStack}" : string.Empty;
        }

        if (actionText != null)
        {
            actionText.text = item != null ? $"{item.actionType}" : string.Empty;
        }

        if (statsText != null)
        {
            statsText.text = item != null ? BuildStatsText(item) : string.Empty;
        }

        if (itemImage != null)
        {
            itemImage.sprite = item != null ? item.image : null;
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
            builder.AppendLine($"Heal Amount: {item.healAmount} HP");
        }

        if (builder.Length == 0)
        {
            builder.Append(string.Empty);
        }

        return builder.ToString().TrimEnd();
    }
}

