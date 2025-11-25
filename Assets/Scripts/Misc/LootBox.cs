using UnityEngine;
using System.Collections;

public class LootBox : MonoBehaviour
{
    public Health health;
    public GameObject openedLootBox;
    public GameObject closedLootBox;
    public BoxCollider2D boxCollider;
    public SpriteRenderer closedLootBoxSpriteRenderer;
    public SpriteRenderer openedLootBoxSpriteRenderer;

    [Header("Loot Box Settings")]
    public int lootBoxTier;
    public int minLootAmount = 2;
    public int maxLootAmount = 5;
    public LootPools lootPools;

    private int lootAmount;

    [Header("Loot Animation Settings")]
    public float lootEjectDistance = 3f;
    public float lootEjectDuration = 0.5f;
    public float minDistanceFromChest = 1.5f;

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
        lootBoxTier = 0;
        lootAmount = Random.Range(minLootAmount, maxLootAmount + 1);
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
        for (int i = 0; i < lootAmount; i++)
        {
            GameObject loot = lootPools.GetRandomItemFromTier(lootBoxTier);
            if (loot != null)
            {
                GameObject spawnedLoot = Instantiate(loot, transform.position, Quaternion.identity);
                
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                float distance = Random.Range(minDistanceFromChest, lootEjectDistance);
                Vector3 targetPosition = transform.position + (Vector3)(randomDirection * distance);
                
                StartCoroutine(EaseOutLoot(spawnedLoot.transform, targetPosition));
            }
        }
    }

    IEnumerator EaseOutLoot(Transform lootTransform, Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < lootEjectDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / lootEjectDuration;
            
            // Ease-out curve (starts fast, ends slow)
            t = 1f - Mathf.Pow(1f - t, 3f);
            
            lootTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            
            yield return null;
        }

        lootTransform.position = targetPosition;
    }
}