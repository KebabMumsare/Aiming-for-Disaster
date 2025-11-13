using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Item itemToPickup;
    public InventoryManager inventoryManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collision detected with: {collision.gameObject.name}, Tag: {collision.tag}, Root: {collision.transform.root.name}");
        
        if (collision.CompareTag("Player") || collision.transform.root.CompareTag("Player"))
        {
            if (inventoryManager == null)
            {
                Debug.LogError("InventoryManager is not assigned!");
                return;
            }
            
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
}