using UnityEngine;

public class PickupItem : MonoBehaviour
{
     public Item itemToPickup;

     public InventoryManager inventoryManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool result = inventoryManager.AddItem(itemToPickup);
        if (result)
        {
            Debug.Log("Picked up: " + itemToPickup.name);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory full, could not pick up: " + itemToPickup.name);
        }
    }
}