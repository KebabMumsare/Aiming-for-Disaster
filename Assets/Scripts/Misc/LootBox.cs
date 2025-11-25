using UnityEngine;

public class LootBox : MonoBehaviour
{
    public Health health;
    public GameObject openedLootBox;
    public GameObject closedLootBox;
    public BoxCollider2D boxCollider;
    public SpriteRenderer closedLootBoxSpriteRenderer;
    public SpriteRenderer openedLootBoxSpriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    }

    // Update is called once per frame
    void Update()
    {
        if (health.currentHealth <= 0f)   
        {
            OpenLootBox();
        }
    }

    void OpenLootBox()
    {
        Debug.Log("LootBox opened");
        closedLootBox.SetActive(false);
        openedLootBox.SetActive(true);

    }
}
