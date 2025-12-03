using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Put this on a trigger (e.g. a door/portal) to move the player to another scene.
/// Works together with PlayerPersist, so the same player object is kept between scenes.
/// </summary>
public class SceneTransition : MonoBehaviour
{
    [Header("Target Scene")]
    [Tooltip("Drag the scene asset from your project to reference it directly.")]
    public Object targetScene;

    [Header("Trigger Settings")]
    [Tooltip("Tag of the object that can trigger this transition (usually 'Player').")]
    public string triggerTag = "Player";

    [Tooltip("If set to None, transition happens immediately on enter. Otherwise this key must be pressed while inside the trigger.")]
    public KeyCode requiredKey = KeyCode.None;

    private bool _playerInRange;

    private void Update()
    {
        if (!_playerInRange)
            return;

        // If no key is required, we already transitioned on enter.
        if (requiredKey == KeyCode.None)
            return;

        if (Input.GetKeyDown(requiredKey))
        {
            DoTransition();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(triggerTag))
            return;

        _playerInRange = true;

        if (requiredKey == KeyCode.None)
        {
            // Auto-transition when entering the trigger
            DoTransition();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(triggerTag))
            return;

        _playerInRange = false;
    }

    private void DoTransition()
    {
        if (targetScene == null)
        {
            Debug.LogError("SceneTransition: No target scene assigned!");
            return;
        }

        // Extract scene name from the asset path
        string sceneName = GetSceneNameFromAsset(targetScene);
        
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"SceneTransition: Could not extract scene name from {targetScene.name}. Make sure it's a scene asset.");
            return;
        }

        if (PlayerPersist.Instance != null)
        {
            PlayerPersist.Instance.TransitionToScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private string GetSceneNameFromAsset(Object sceneAsset)
    {
        if (sceneAsset == null)
            return null;

#if UNITY_EDITOR
        // In editor, we can get the asset path and extract the scene name
        string assetPath = AssetDatabase.GetAssetPath(sceneAsset);
        if (string.IsNullOrEmpty(assetPath))
            return null;

        // Extract just the scene name without path and extension
        // e.g., "Assets/Scenes/MyScene.unity" -> "MyScene"
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
        return sceneName;
#else
        // At runtime, we use the object name as fallback
        // Note: This requires the scene to be in Build Settings with the correct name
        return sceneAsset.name;
#endif
    }
}


