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

    [Header("Explosion VFX Settings")]
    public GameObject explosionVFX;
    public float explosionDelay = 1f; // Delay in seconds before explosion plays

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

        if (explosionVFX != null)
        {
            StartCoroutine(ExplosionSequence());
        }
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
                
                StartCoroutine(EjectLootWithPhysics(spawnedLoot, targetPosition));
            }
        }
    }

    IEnumerator EjectLootWithPhysics(GameObject loot, Vector3 targetPosition)
    {
        BoxCollider2D itemCollider = loot.GetComponent<BoxCollider2D>();
        itemCollider.isTrigger = false;
        
        Rigidbody2D rb = loot.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = loot.AddComponent<Rigidbody2D>();
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.linearDamping = 2f;
        rb.freezeRotation = true;
        
        PhysicsMaterial2D bounceMaterial = new PhysicsMaterial2D("LootBounce");
        bounceMaterial.bounciness = 0.6f;
        bounceMaterial.friction = 0.4f;
        itemCollider.sharedMaterial = bounceMaterial;
        
        IgnoreCollisionsWith(itemCollider, new string[] { "Enemy", "LootBox" });
        
        PickupItem[] allItems = FindObjectsOfType<PickupItem>();
        foreach (PickupItem otherItem in allItems)
        {
            if (otherItem.gameObject != loot)
            {
                Collider2D otherCollider = otherItem.GetComponent<Collider2D>();
                if (otherCollider != null)
                {
                    Physics2D.IgnoreCollision(itemCollider, otherCollider, true);
                }
            }
        }
        
        Vector2 direction = ((Vector2)targetPosition - (Vector2)transform.position).normalized;
        float distance = Vector2.Distance(transform.position, targetPosition);
        rb.linearVelocity = direction * (distance / lootEjectDuration);
        
        yield return new WaitForSeconds(lootEjectDuration);
        
        Destroy(rb);
        itemCollider.isTrigger = true;
        itemCollider.sharedMaterial = null;
    }
    
    void IgnoreCollisionsWith(Collider2D itemCollider, string[] tags)
    {
        foreach (string tag in tags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objects)
            {
                Collider2D col = obj.GetComponent<Collider2D>();
                if (col != null)
                {
                    Physics2D.IgnoreCollision(itemCollider, col, true);
                }
            }
        }
    }

    IEnumerator ExplosionSequence()
    {
        yield return new WaitForSeconds(explosionDelay);

        ParticleSystem[] particleSystems = explosionVFX.GetComponentsInChildren<ParticleSystem>(true);
        
        if (particleSystems.Length == 0)
        {
            Debug.LogWarning("No particle systems found in ExplosionVFX!");
            yield break;
        }

        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }

        boxCollider.enabled = false;
        openedLootBoxSpriteRenderer.enabled = false;

        while (true)
        {
            bool anyPlaying = false;
            foreach (ParticleSystem ps in particleSystems)
            {
                if (ps != null && ps.isPlaying)
                {
                    anyPlaying = true;
                    break;
                }
            }
            
            if (!anyPlaying)
            {
                break;
            }
            
            yield return null;
        }

        Destroy(gameObject);
    }
}