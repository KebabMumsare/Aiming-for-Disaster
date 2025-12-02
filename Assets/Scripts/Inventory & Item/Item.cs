using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Only gameplay")]
    public TileBase tile;
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);
    public GameObject weaponPrefab;

    [Header("Only UI")]
    public bool stackable = true;
    public int maxStack = 0;

    [Header("Both")]
    public Sprite image;
    
    [Header("If heal item")]
    public float healAmount = 0f;

    [Header("Passive Buffs")]
    public List<PassiveBuff> passiveBuffs;
}

[System.Serializable]
public class ItemStat
{
    public string label;
    public string value;
}

public enum ItemType
{
    Weapon,
    Consumable,
    PassiveBuffItem
}

public enum ActionType
{
    Ranged,
    Melee,
    Use,
    Passive
}

public enum StatType
{
    MoveSpeed,
    Damage,
    XP
}

[System.Serializable]
public struct PassiveBuff
{
    public StatType statType;
    public float value; // Multiplier (e.g., 0.1 for +10%) or additive value
}
