using UnityEngine;

public class LootBox : MonoBehaviour
{
    public Health health;
    public GameObject openedLootBox;
    public GameObject closedLootBox;
    public BoxCollider2D boxCollider;
    public SpriteRenderer closedLootBoxSpriteRenderer;
    public SpriteRenderer openedLootBoxSpriteRenderer;

    public int LootBoxTier;
    public int LootAmount;
    public LootPools lootPools;

    private bool hasBeenOpened = false;

    void Start()
    {
        if (health == null)
        {
            health = GetComponent<Health>();
            Debug.Log("LootBox health not found, adding it");
        }
        else
        {
            Debug.Log("LootBox health found");
        }

        if (lootPools == null)
        {
            lootPools = FindFirstObjectByType<LootPools>();
            if (lootPools == null)
            {
                Debug.LogError("LootPools not found! Please assign it in the Inspector or add a LootPools component to the scene.");
            }
        }
        
        if (Random.Range(0, 2) == 0)
        {
            closedLootBoxSpriteRenderer.flipX = true;
            openedLootBoxSpriteRenderer.flipX = true;
        }
        else 
        {
            closedLootBoxSpriteRenderer.flipX = false;
            openedLootBoxSpriteRenderer.flipX = false;
        }

        //TODO: Add more rarities/tiers
        LootBoxTier = 0;
        LootAmount = Random.Range(2, 5);
        Debug.Log("Loot Box Tier: " + LootBoxTier + " Loot Amount: " + LootAmount);
    }

    void Update()
    {
        if (health.currentHealth <= 0f && !hasBeenOpened)   
        {
            OpenLootBox();
        }
    }

    void OpenLootBox()
    {
        hasBeenOpened = true;
        Debug.Log("LootBox opened");
        closedLootBox.SetActive(false);
        openedLootBox.SetActive(true);
        DropLoot();
    }

    void DropLoot()
    {
        for (int i = 0; i < LootAmount; i++)
        {
            GameObject loot = lootPools.GetRandomItemFromTier(LootBoxTier);
            if (loot != null)
            {
                Instantiate(loot, transform.position + (Vector3)(Random.insideUnitCircle * 2f + new Vector2(1f, 1f)), Quaternion.identity);
            }
        }
    }
}