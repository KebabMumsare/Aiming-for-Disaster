using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;


    public void PickupItem(int id)
    {
        bool result = inventoryManager.AddItem(itemsToPickup[id]);
        if (result)
        {
            Debug.Log("Picked up: " + itemsToPickup[id].name);
        }
        else
        {
            Debug.Log("Inventory full, could not pick up: " + itemsToPickup[id].name);
        }
    }
}
