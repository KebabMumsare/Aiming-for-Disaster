using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Currency : MonoBehaviour
{
    public static Currency Instance { get; private set; }

    [Header("Starting / UI (optional)")]
    [SerializeField] private int startingBalance = 0;
    [SerializeField] private Text uiText;
    [SerializeField] private TMP_Text tmpText;

    public event Action<int> OnChange;

    private int _balance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _balance = Mathf.Max(0, startingBalance);
        Notify();
    }

    private void Notify()
    {
        OnChange?.Invoke(_balance);

        if (tmpText != null) tmpText.text = _balance.ToString();
        else if (uiText != null) uiText.text = _balance.ToString();
        else
        {
            var go = GameObject.Find("CurrencyText");
            if (go != null)
            {
                var t = go.GetComponent<TMP_Text>();
                if (t != null) t.text = _balance.ToString();
                else
                {
                    var u = go.GetComponent<Text>();
                    if (u != null) u.text = _balance.ToString();
                }
            }
        }
    }

    public int GetBalance() => _balance;

    public void Add(int amount)
    {
        amount = Mathf.FloorToInt(amount);
        if (amount <= 0) return;
        _balance += amount;
        Notify();
    }

    public bool Spend(int amount)
    {
        amount = Mathf.FloorToInt(amount);
        if (amount <= 0) return false;
        if (amount > _balance) return false;
        _balance -= amount;
        Notify();
        return true;
    }

    public void SetBalance(int amount)
    {
        _balance = Mathf.Max(0, Mathf.FloorToInt(amount));
        Notify();
    }

    // Static convenience wrappers for quick calls from other scripts
    public static int Get() => Instance != null ? Instance.GetBalance() : 0;
    public static void AddAmount(int amount)
    {
        if (Instance != null) Instance.Add(amount);
        else Debug.LogWarning("Currency.Instance is null. Attach Currency to a GameObject in the scene.");
    }
    public static bool SpendAmount(int amount)
    {
        if (Instance != null) return Instance.Spend(amount);
        Debug.LogWarning("Currency.Instance is null. Attach Currency to a GameObject in the scene.");
        return false;
    }
    public static void Set(int amount)
    {
        if (Instance != null) Instance.SetBalance(amount);
        else Debug.LogWarning("Currency.Instance is null. Attach Currency to a GameObject in the scene.");
    }
}