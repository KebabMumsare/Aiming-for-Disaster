using UnityEngine;

public class LootBoxSpawner : MonoBehaviour
{
    public GameObject lootBoxPrefab;
    private GameObject[] FloorTiles;
    public int minLootBoxes = 1;
    public int maxLootBoxes = 3;

    void Start()
    {
        FloorTiles = GameObject.FindGameObjectsWithTag("FloorTile");
        int numberOfLootBoxes = Random.Range(minLootBoxes, maxLootBoxes + 1);
        for (int i = 0; i < numberOfLootBoxes; i++)
        {
            GameObject floorTile = FloorTiles[Random.Range(0, FloorTiles.Length)];
            Instantiate(lootBoxPrefab, new Vector3(floorTile.transform.position.x + Random.Range(-1, 1), floorTile.transform.position.y + Random.Range(-1, 1) + 1, 0), Quaternion.identity);
        }
    }   
}