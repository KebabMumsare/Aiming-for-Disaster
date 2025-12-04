using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class LootBoxSpawner : MonoBehaviour
{
    public GameObject lootBoxPrefab;
    public int minLootBoxes = 1;
    public int maxLootBoxes = 3;

    void Start()
    {
        GameObject tilemapObject = GameObject.FindGameObjectWithTag("FloorTiles");
        
        if (tilemapObject == null)
        {
            Debug.LogError("No GameObject with tag 'FloorTiles' found!");
            return;
        }

        Tilemap tilemap = tilemapObject.GetComponent<Tilemap>();
        
        if (tilemap == null)
        {
            Debug.LogError("GameObject with tag 'FloorTiles' does not have a Tilemap component!");
            return;
        }

        //Getting all tile positions from the tilemap
        List<Vector3> tilePositions = new List<Vector3>();
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(cellPosition);

                if (tile != null)
                {
                    // Convert cell position to world position
                    Vector3 worldPosition = tilemap.CellToWorld(cellPosition) + tilemap.tileAnchor;
                    tilePositions.Add(worldPosition);
                }
            }
        }

        if (tilePositions.Count == 0)
        {
            Debug.LogWarning("No tiles found in the tilemap!");
            return;
        }

        int numberOfLootBoxes = Random.Range(minLootBoxes, maxLootBoxes + 1);
        numberOfLootBoxes = Mathf.Min(numberOfLootBoxes, tilePositions.Count); // Don't spawn more than available tiles

        for (int i = 0; i < numberOfLootBoxes; i++)
        {
            int randomIndex = Random.Range(0, tilePositions.Count);
            Vector3 spawnPosition = tilePositions[randomIndex];
            Instantiate(lootBoxPrefab, new Vector3(spawnPosition.x, spawnPosition.y, 0), Quaternion.identity, gameObject.transform);
            
            //Remove the used position to avoid spawning multiple loot boxes on the same tile
            tilePositions.RemoveAt(randomIndex);
        }
    }   
}