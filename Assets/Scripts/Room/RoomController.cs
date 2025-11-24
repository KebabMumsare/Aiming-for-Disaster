using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class RoomController : MonoBehaviour
{
    public GameObject enemyContainer;
    
    [Header("Door Settings")]
    [Tooltip("The GameObject that holds the door objects as children (usually the Tilemap object).")]
    public GameObject doorParent;
    
    [Tooltip("Tag to search for on child GameObjects (e.g. 'Closed_Door')")]
    public string doorSearchTag = "Closed_Door";

    private List<GameObject> doorsToDisable = new List<GameObject>();
    private bool roomCleared = false;

    void Start()
    {
        if (Application.isPlaying)
        {
            Debug.Log($"[RoomController] Start called on {gameObject.name}");
        }

        if (doorParent == null)
        {
            if (Application.isPlaying) Debug.LogError("Door Parent not assigned in RoomController!");
            return;
        }

        ScanForDoors();
    }

    void ScanForDoors()
    {
        doorsToDisable.Clear();
        int foundCount = 0;

        // Iterate through all children of the Door Parent
        foreach (Transform child in doorParent.transform)
        {
            if (child.CompareTag(doorSearchTag))
            {
                doorsToDisable.Add(child.gameObject);
                foundCount++;
                if (Application.isPlaying) Debug.Log($"[RoomController] Found Door Child '{child.name}'");
            }
        }
        
        if (Application.isPlaying) Debug.Log($"[RoomController] Scan complete. Found {foundCount} doors to disable.");
    }

    void Update()
    {
        if (!Application.isPlaying) return;
        if (roomCleared) return;

        if (enemyContainer == null)
        {
            return;
        }

        // Check if there are any active enemies left
        if (CountActiveEnemies() == 0)
        {
            RoomCleared();
        }
    }

    int CountActiveEnemies()
    {
        int activeCount = 0;
        foreach (Transform child in enemyContainer.transform)
        {
            if (child.gameObject.activeSelf)
            {
                activeCount++;
            }
        }
        return activeCount;
    }

    void RoomCleared()
    {
        roomCleared = true;
        Debug.Log("Room Cleared! Disabling doors...");
        
        foreach (GameObject door in doorsToDisable)
        {
            if (door != null)
            {
                door.SetActive(false);
            }
        }
    }
}
