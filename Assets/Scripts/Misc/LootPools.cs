using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedItem
{
    public GameObject itemPrefab;
    public float weight = 1f;
}

public class LootPools : MonoBehaviour
{
    // CREATE LOOT POOL TIERS HERE
    [Header("Common Loot (Tier 0)")]
    [Tooltip("Items that can drop from tier 0 (Common) loot boxes. Drag prefabs and set weights.")]
    public List<WeightedItem> tier0Loot = new List<WeightedItem>();

    public GameObject GetRandomItemFromTier(int tierIndex)
    {
        List<WeightedItem> tierLoot = null;

        switch (tierIndex)
        {
            case 0:
                tierLoot = tier0Loot;
                break;
            default:
                Debug.LogWarning($"Loot tier {tierIndex} not found!");
                return null;
        }

        if (tierLoot == null || tierLoot.Count == 0)
        {
            Debug.LogWarning($"Tier {tierIndex} has no items!");
            return null;
        }

        float totalWeight = 0f;
        foreach (WeightedItem weightedItem in tierLoot)
        {
            if (weightedItem.itemPrefab != null && weightedItem.weight > 0f)
            {
                totalWeight += weightedItem.weight;
            }
        }

        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (WeightedItem weightedItem in tierLoot)
        {
            if (weightedItem.itemPrefab != null && weightedItem.weight > 0f)
            {
                currentWeight += weightedItem.weight;
                if (randomValue <= currentWeight)
                {
                    return weightedItem.itemPrefab;
                }
            }
        }

        return null;
    }
}