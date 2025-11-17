using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Assign 'MainInventoryGroup'")]
    public GameObject mainInventoryPanel; // assign MainInventoryGroup to this one

    [Header("Assign Player to this one")]
    public Health health; // assign Player to this one


    public Item item;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    private int NumberOfToolbarSlots = 7;
    private int NumberOfWeaponSlots = 2;
    private PlayerInputRouter playerInput; // Reference to the input router

    int selectedSlot = -1;

    private void Start()
    {
        // Find the PlayerInputRouter in the scene
        playerInput = FindFirstObjectByType<PlayerInputRouter>();
        ChangeSelectedSlot(0);
        if (mainInventoryPanel != null)
        {
            mainInventoryPanel.SetActive(false); // Start with the inventory hidden
        }
    }

    private void Update()
    {
        // if "I" is pressed down, toggle inventory panel
        if (playerInput.OpenInventoryPressed) // Use the new input property
        {
            if (mainInventoryPanel != null)
            {
                mainInventoryPanel.SetActive(!mainInventoryPanel.activeSelf);
            }
        }

        // if "E" is pressed down, use the selected item
        if (playerInput.UseItemPressed) // Use the new input property
        {
            Item itemInSlot = GetSelectedItem(false);

            if (itemInSlot != null && itemInSlot.type == ItemType.Consumable)
            {
                Item recievedItem = GetSelectedItem(true);
                Debug.Log("Used item: " + recievedItem.name);
                health.Heal(itemInSlot.healAmount);
            }
            else if (itemInSlot != null)
            {
                Debug.Log("Selected item is not consumable or weapon.");
            }
            else
            {
                Debug.Log("No item to use.");
            }
        }

        // Check for number key input to change selected slot
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < (NumberOfToolbarSlots + 1))
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    // Changes the selected inventory slot
    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
            
            // If previous slot was a weapon slot, unequip the weapon
            if (inventorySlots[selectedSlot] is InventoryWeaponSlot previousWeaponSlot)
            {
                if (previousWeaponSlot.WeaponPivot != null && previousWeaponSlot.WeaponPivot.transform.childCount > 0)
                {
                    previousWeaponSlot.UnequipWeapon();
                }
            }
        }

        inventorySlots[newValue].Select();
        selectedSlot = newValue;

        // If new slot is a weapon slot with a weapon item, equip it
        Debug.Log($"Changing to slot {newValue}, slot type: {inventorySlots[newValue].GetType()}");
        if (inventorySlots[newValue] is InventoryWeaponSlot weaponSlot)
        {
            Debug.Log("Slot is InventoryWeaponSlot");
            InventoryItem itemInSlot = weaponSlot.GetComponentInChildren<InventoryItem>();
            Debug.Log($"Item in slot: {(itemInSlot != null ? itemInSlot.name : "null")}");
            if (itemInSlot != null && itemInSlot.item != null)
            {
                Debug.Log($"Item type: {itemInSlot.item.type}, Weapon prefab: {(itemInSlot.item.weaponPrefab != null ? itemInSlot.item.weaponPrefab.name : "null")}");
                if (itemInSlot.item.type == ItemType.Weapon)
                {
                    if (itemInSlot.item.weaponPrefab != null)
                    {
                        Debug.Log("Calling EquipWeapon on weaponSlot");
                        weaponSlot.EquipWeapon(itemInSlot.item.weaponPrefab);
                    }
                    else
                    {
                        Debug.LogWarning("Weapon item has no weaponPrefab assigned");
                    }
                }
            }
            else
            {
                Debug.Log("No item in weapon slot");
            }
        }
        else
        {
            Debug.Log("Slot is NOT InventoryWeaponSlot");
        }
    }

    void EquipWeapon(Item weaponItem)
{
    if (weaponItem.weaponPrefab == null)
    {
        Debug.LogWarning($"Item {weaponItem.name} has no weapon prefab.");
        return;
    }

    // Find an available weapon slot (first 2 slots)
    InventoryWeaponSlot availableSlot = null;
    for (int i = 0; i < NumberOfWeaponSlots && i < inventorySlots.Length; i++)
    {
        if (inventorySlots[i] is InventoryWeaponSlot weaponSlot)
        {
            // Check if slot is empty or has the same weapon
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                availableSlot = weaponSlot;
                break;
            }
            else if (itemInSlot.item == weaponItem)
            {
                // Same weapon, just equip it
                availableSlot = weaponSlot;
                break;
            }
        }
    }

    // If no empty slot, use the first weapon slot (replace current weapon)
    if (availableSlot == null && inventorySlots.Length > 0 && inventorySlots[0] is InventoryWeaponSlot firstWeaponSlot)
    {
        availableSlot = firstWeaponSlot;
        // Unequip current weapon if there is one
        if (firstWeaponSlot.WeaponPivot != null && firstWeaponSlot.WeaponPivot.transform.childCount > 0)
        {
            firstWeaponSlot.UnequipWeapon();
        }
    }

    if (availableSlot != null && availableSlot.WeaponPivot != null)
    {
        availableSlot.EquipWeapon(weaponItem.weaponPrefab);
    }
    else
    {
        Debug.LogWarning("No weapon slot available or WeaponPivot not assigned.");
    }
}

    // Adds an item to the inventory - used in PickupItem.cs
    public bool AddItem(Item item)
    {
        // Check if any slot has the same item with count lower than max
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < item.maxStack && item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        // Find any empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    // Spawns a new item in the given slot
    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    // use item logic
    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if (use == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }

            return item;
        }

        return null;
    }
    
}
