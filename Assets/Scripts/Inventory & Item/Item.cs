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
