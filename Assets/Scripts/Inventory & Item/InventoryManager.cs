using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Assign 'MainInventoryGroup'")]
    public GameObject mainInventoryPanel; // assign MainInventoryGroup to this one

    [Header("Assign Player to this one")]
    public Health health; // assign Player to this one
    public SkillManager skillManager; // assign Player to this one

    public Item item;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public GameObject skillTreeUI;
    [SerializeField] private InventoryItemDetailsUI itemDetailsUI;

    private int NumberOfToolbarSlots = 7;
    private int NumberOfWeaponSlots = 2;
    private PlayerInputRouter playerInput; // Reference to the input router

    int selectedSlot = -1;

    private void Start()
    {
        // Find the PlayerInputRouter in the scene
        playerInput = FindFirstObjectByType<PlayerInputRouter>();
        InitialiseSlots();
        ChangeSelectedSlot(0);
        if (itemDetailsUI != null)
        {
            itemDetailsUI.Hide();
        }
        if (mainInventoryPanel != null)
        {
            mainInventoryPanel.SetActive(false); // Start with the inventory hidden
        }
        if (skillTreeUI != null)
        {
            skillTreeUI.SetActive(false); // Start with the skill tree hidden
        }

        if (health == null)
        {
            Debug.LogError("Player health is not assigned. Assign it on the InventoryManager");
        }

        if (skillManager == null)
        {
             // Try to find it on the player (assuming health is on the player)
             if (health != null)
             {
                 skillManager = health.GetComponent<SkillManager>();
             }
             
             if (skillManager == null)
             {
                 Debug.LogWarning("SkillManager is not assigned and could not be found on the player. Healing upgrades will not work.");
             }
        }

        if (mainInventoryPanel == null)
        {
            Debug.LogError("MainInventoryPanel is not assigned. Assign 'Main Inventory Group' on the InventoryManager");
        }

        if (inventoryItemPrefab == null)
        {
            Debug.LogError("'InventoryItemPrefab' is not assigned. Assign the 'InventoryItem' prefab on the InventoryManager");
        }
        CalculatePassiveBuffs();
    }

    public bool IsInventoryOpen => mainInventoryPanel != null && mainInventoryPanel.activeSelf;

    private void Update()
    {
        // if "I" is pressed down, toggle inventory panel
        if (playerInput.OpenInventoryPressed) // Use the new input property
        {
            if (IsInventoryOpen)
            {
                mainInventoryPanel.SetActive(false);
                skillTreeUI.SetActive(false);
            }
            else
            {
                mainInventoryPanel.SetActive(true);
                skillTreeUI.SetActive(false);
            }
        }

        // if "E" is pressed down, use the selected item
        if (playerInput.UseItemPressed && !IsInventoryOpen) // Use the new input property
        {
            Item itemInSlot = GetSelectedItem(false);

            if (itemInSlot != null && itemInSlot.type == ItemType.Consumable)
            {
                Item recievedItem = GetSelectedItem(true);
                float healAmount = itemInSlot.healAmount;
                
                if (skillManager != null)
                {
                    healAmount *= skillManager.GetHealingMultiplier();
                }
                
                health.Heal(healAmount);
                UpdateSelectedItemDetails(selectedSlot);
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
    public void ChangeSelectedSlot(int newValue)
    {
        if (newValue < 0 || newValue >= inventorySlots.Length)
        {
            return;
        }

        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
            
            // If previous slot was a weapon slot, ensure the weapon is unequipped
            if (inventorySlots[selectedSlot] is InventoryWeaponSlot previousWeaponSlot)
            {
                previousWeaponSlot.UnequipWeapon();
            }
        }

        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        UpdateSelectedItemDetails(newValue);

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

    void InitialiseSlots()
    {
        if (inventorySlots == null)
        {
            return;
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] != null)
            {
                inventorySlots[i].Initialise(this, i);
            }
        }
    }

    void UpdateSelectedItemDetails(int slotIndex)
    {
        if (itemDetailsUI == null || slotIndex < 0 || slotIndex >= inventorySlots.Length)
        {
            return;
        }

        InventoryItem itemInSlot = inventorySlots[slotIndex].GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null && itemInSlot.item != null)
        {
            itemDetailsUI.Show(itemInSlot.item, itemInSlot.count);
        }
        else
        {
            itemDetailsUI.Hide();
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

            // Check if slot is a weapon slot and item is not a weapon
            if (slot is InventoryWeaponSlot && item.type != ItemType.Weapon)
            {
                continue;
            }

            // Check if slot is NOT a weapon slot and item IS a weapon
            if (!(slot is InventoryWeaponSlot) && item.type == ItemType.Weapon)
            {
                continue;
            }

            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < item.maxStack && item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                UpdateSelectedItemDetails(selectedSlot);
                CalculatePassiveBuffs();
                return true;
            }
        }

        // Find any empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];

            // Check if slot is a weapon slot and item is not a weapon
            if (slot is InventoryWeaponSlot && item.type != ItemType.Weapon)
            {
                continue;
            }

            // Check if slot is NOT a weapon slot and item IS a weapon
            if (!(slot is InventoryWeaponSlot) && item.type == ItemType.Weapon)
            {
                continue;
            }

            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                CalculatePassiveBuffs();
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
        UpdateSelectedItemDetails(selectedSlot);
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
                CalculatePassiveBuffs();
            }

            return item;
        }

        return null;
    }
    
    
    public void CalculatePassiveBuffs()
    {
        float moveSpeedMultiplier = 1f;
        float damageMultiplier = 1f;
        float xpMultiplier = 1f;

        // Iterate through all inventory slots
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item != null && itemInSlot.item.passiveBuffs != null)
            {
                foreach (var buff in itemInSlot.item.passiveBuffs)
                {
                    switch (buff.statType)
                    {
                        case StatType.MoveSpeed:
                            moveSpeedMultiplier += buff.value;
                            break;
                        case StatType.Damage:
                            damageMultiplier += buff.value;
                            break;
                        case StatType.XP:
                            xpMultiplier += buff.value;
                            break;
                    }
                }
            }
        }

        // Apply Move Speed Buff
        if (health != null)
        {
            var playerMover = health.GetComponent<PlayerMover2D>();
            if (playerMover != null)
            {
                playerMover.SetSpeedMultiplier(moveSpeedMultiplier);
            }
            
            var playerXP = health.GetComponent<PlayerXP>();
            if (playerXP != null)
            {
                playerXP.SetXPMultiplier(xpMultiplier);
            }
        }

        // Apply Damage Buff to equipped weapons
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot is InventoryWeaponSlot weaponSlot)
            {
                var weapon = weaponSlot.GetComponentInChildren<Weapon>();
                if (weapon != null)
                {
                    weapon.SetDamageMultiplier(damageMultiplier);
                }
            }
        }
    }
}