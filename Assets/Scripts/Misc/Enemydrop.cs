using UnityEngine;
using TMPro;

public class Enemydrop : MonoBehaviour
{
    [Header("Reward")]
    [Tooltip("Amount of coins this enemy gives on death")]
    public int coinsPerKill = 10;                // public & serialized

    [Header("Optional UI")]
    [Tooltip("Floating text prefab (Text or TMP inside)")]
    public GameObject floatingTextPrefab;        // public & serialized

    // Call this from your health/AI when the enemy dies.
    public void Die()
    {
        GiveReward();
        SpawnFloatingText();
        Destroy(gameObject);
    }

    // Gives the configured reward
    public void GiveReward()
    {
        Currency.AddAmount(coinsPerKill);
        Debug.Log($"Enemy killed -> +{coinsPerKill} coins (new balance: {Currency.Get()})");
    }

    public void SpawnFloatingText()
    {
        if (floatingTextPrefab == null) return;

        var go = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        var uiText = go.GetComponentInChildren<UnityEngine.UI.Text>();
        if (uiText != null) { uiText.text = $"+{coinsPerKill}"; return; }
        var tmp = go.GetComponentInChildren<TMP_Text>();
        if (tmp != null) { tmp.text = $"+{coinsPerKill}"; return; }
    }
}