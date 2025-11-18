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

    [Header("Details")]
    [TextArea]
    public string description;
    public List<ItemStat> extraStats = new List<ItemStat>();
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
    WeaponUpgrade
}

public enum ActionType
{
    Throw,
    BoomerangThrow,
    Melee,
    Use
}
