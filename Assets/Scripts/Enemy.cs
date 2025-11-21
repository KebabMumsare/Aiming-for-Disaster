using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyVariant { A, B }

    [Header("Enemy settings")]
    public EnemyVariant variant = EnemyVariant.A;
    public int rewardA = 10;
    public int rewardB = 20;
    [Tooltip("If > 0 this overrides variant rewards")]
    public int customReward = 0;

    [Header("Optional UI")]
    public GameObject floatingTextPrefab; // optional prefab that displays "+X"

    // Call this method when the enemy dies (from health system)
    public void Die()
    {
        int reward = ResolveReward();
        Currency.AddAmount(reward);
        SpawnFloatingText(reward);
        Destroy(gameObject);
    }

    private int ResolveReward()
    {
        if (customReward > 0) return Mathf.FloorToInt(customReward);

        switch (variant)
        {
            case EnemyVariant.A: return Mathf.FloorToInt(rewardA);
            case EnemyVariant.B: return Mathf.FloorToInt(rewardB);
            default: return Mathf.FloorToInt(rewardA);
        }
    }

    private void SpawnFloatingText(int reward)
    {
        if (floatingTextPrefab == null) return;
        var go = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        // if prefab has a script to set text, try to set it (optional conventions)
        var txt = go.GetComponentInChildren<UnityEngine.UI.Text>();
        if (txt != null) { txt.text = $"+{reward}"; return; }
        var tmp = go.GetComponentInChildren<TMPro.TMP_Text>();
        if (tmp != null) { tmp.text = $"+{reward}"; return; }
    }
}
```// filepath: c:\Users\Ibraheem.alshabee\Desktop\portf\Aiming-for-Disaster\Assets\Scripts\Enemy.cs
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyVariant { A, B }

    [Header("Enemy settings")]
    public EnemyVariant variant = EnemyVariant.A;
    public int rewardA = 10;
    public int rewardB = 20;
    [Tooltip("If > 0 this overrides variant rewards")]
    public int customReward = 0;

    [Header("Optional UI")]
    public GameObject floatingTextPrefab; // optional prefab that displays "+X"

    // Call this method when the enemy dies (from health system)
    public void Die()
    {
        int reward = ResolveReward();
        Currency.AddAmount(reward);
        SpawnFloatingText(reward);
        Destroy(gameObject);
    }

    private int ResolveReward()
    {
        if (customReward > 0) return Mathf.FloorToInt(customReward);

        switch (variant)
        {
            case EnemyVariant.A: return Mathf.FloorToInt(rewardA);
            case EnemyVariant.B: return Mathf.FloorToInt(rewardB);
            default: return Mathf.FloorToInt(rewardA);
        }
    }

    private void SpawnFloatingText(int reward)
    {
        if (floatingTextPrefab == null) return;
        var go = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        // if prefab has a script to set text, try to set it (optional conventions)
        var txt = go.GetComponentInChildren<UnityEngine.UI.Text>();
        if (txt != null) { txt.text = $"+{reward}"; return; }
        var tmp = go.GetComponentInChildren<TMPro.TMP_Text>();
        if (tmp != null) { tmp.text = $"+{reward}"; return; }