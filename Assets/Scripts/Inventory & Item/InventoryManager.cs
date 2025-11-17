using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Assign 'MainInventoryGroup'")]
    public GameObject mainInventoryPanel; // assign MainInventoryGroup to this one
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    private int NumberOfToolbarSlots = 7;
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
            } 
            else if (itemInSlot != null)
            {
                Debug.Log("Selected item is not consumable.");
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
        }

        inventorySlots[newValue].Select();
        selectedSlot = newValue;
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
