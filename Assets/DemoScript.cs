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

    public void GetSelectedItem()
    {
        Item recievedItem = inventoryManager.GetSelectedItem(false);
        if (recievedItem != null)
        {
            Debug.Log("Selected item: " + recievedItem.name);
        }
        else
        {
            Debug.Log("No item selected.");
        }
    }

    public void UseSelectedItem()
    {
        Item recievedItem = inventoryManager.GetSelectedItem(true);
        if (recievedItem != null)
        {
            Debug.Log("Used item: " + recievedItem.name);
        }
        else
        {
            Debug.Log("No item used.");
        }
    }
}
