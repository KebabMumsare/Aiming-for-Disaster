using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Item itemToPickup;
    public InventoryManager inventoryManager;

    private float spawnTime;
    private float pickupDelay = 1.0f;

    void Awake()
    {
        spawnTime = Time.time;
        if (inventoryManager == null)
        {
            GameObject managerObj = GameObject.FindGameObjectWithTag("InvManager");
            if (managerObj != null)
            {
                inventoryManager = managerObj.GetComponent<InventoryManager>();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryPickup(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TryPickup(collision);
    }

    private void TryPickup(Collider2D collision)
    {
        if (Time.time < spawnTime + pickupDelay) return;

        // Debug.Log($"Collision detected with: {collision.gameObject.name}, Tag: {collision.tag}, Root: {collision.transform.root.name}");
        
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
                // Debug.Log("Inventory full, could not pick up: " + itemToPickup.name);
            }
        }
    }

    public void SetItem(Item newItem)
    {
        itemToPickup = newItem;
        if (itemToPickup != null && itemToPickup.image != null)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = itemToPickup.image;
            }
        }
    }

    public void AnimateDrop(Vector3 targetPosition)
    {
        StartCoroutine(DropAnimation(targetPosition));
    }

    private System.Collections.IEnumerator DropAnimation(Vector3 targetPosition)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 startPosition = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // Linear movement for now, can add arc later
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            
            yield return null;
        }

        transform.position = targetPosition;
    }
}