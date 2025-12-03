using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPersist : MonoBehaviour
{
    public static PlayerPersist Instance { get; private set; }
    
    [Header("Spawn Settings")]
    [Tooltip("Tag name of spawn points in scenes (e.g., 'SpawnPoint')")]
    public string spawnPointTag = "SpawnPoint";
    
    [Tooltip("If no spawn point found, spawn at this position")]
    public Vector2 defaultSpawnPosition = Vector2.zero;

    private void Awake()
    {
        // Singleton pattern - ensure only one player exists
        if (Instance != null && Instance != this)
        {
            // If a player already exists, destroy this one
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Subscribe to scene loaded event to reposition player
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe when destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reposition player to spawn point in new scene
        RepositionPlayer();
    }

    private void RepositionPlayer()
    {
        // Try to find a spawn point in the new scene
        GameObject spawnPoint = GameObject.FindGameObjectWithTag(spawnPointTag);
        
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
            Debug.Log($"Player repositioned to spawn point: {spawnPoint.name}");
        }
        else
        {
            // If no spawn point found, use default position
            transform.position = defaultSpawnPosition;
            Debug.LogWarning($"No spawn point with tag '{spawnPointTag}' found. Using default position: {defaultSpawnPosition}");
        }
    }

    // Public method to manually transition to a scene
    public void TransitionToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Public method to transition by scene index
    public void TransitionToScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}