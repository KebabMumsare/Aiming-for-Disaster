using UnityEngine;

public class LootBox : MonoBehaviour
{
    public Health health;
    public GameObject openedLootBox;
    public GameObject closedLootBox;

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
