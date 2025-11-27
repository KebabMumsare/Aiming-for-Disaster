using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Currency : MonoBehaviour
{
    public static Currency Instance { get; private set; }

    [Header("Starting / UI (optional)")]
    public int startingBalance = 0;           // public & serialized
    public Text uiText;                       // assign in inspector
    public TMP_Text tmpText;                  // assign in inspector

    public event Action<int> OnChange;

    [Header("Runtime (visible)")]
    [SerializeField] public int balance;      // public & serialized so you can watch it in Inspector
    public int Balance => balance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        balance = Mathf.Max(0, startingBalance);
        Notify();
    }

    public void Notify()
    {
        OnChange?.Invoke(balance);

        if (tmpText != null)
        {
            tmpText.text = balance.ToString();
            return;
        }

        if (uiText != null)
        {
            uiText.text = balance.ToString();
            return;
        }

        var go = GameObject.Find("CurrencyText") ?? GameObject.Find("Currency");
        if (go != null)
        {
            var t = go.GetComponent<TMP_Text>();
            if (t != null) { tmpText = t; tmpText.text = balance.ToString(); return; }
            var u = go.GetComponent<Text>();
            if (u != null) { uiText = u; uiText.text = balance.ToString(); return; }
        }

        var allTmps = FindObjectsOfType<TMP_Text>();
        foreach (var tt in allTmps)
        {
            if (tt.gameObject.name.IndexOf("currency", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                tmpText = tt;
                tmpText.text = balance.ToString();
                return;
            }
        }
    }

    // instance methods (public)
    public void Add(int amount)
    {
        if (amount <= 0) return;
        balance += amount;
        Notify();
    }

    public bool Spend(int amount)
    {
        if (amount <= 0) return false;
        if (amount > balance) return false;
        balance -= amount;
        Notify();
        return true;
    }

    public void SetBalance(int amount)
    {
        balance = Mathf.Max(0, amount);
        Notify();
    }

    // ensure a singleton exists (creates one if missing)
    private static void EnsureInstance()
    {
        if (Instance != null) return;

        var existing = FindObjectOfType<Currency>();
        if (existing != null)
        {
            Instance = existing;
            return;
        }

        var go = new GameObject("Currency");
        Instance = go.AddComponent<Currency>();

        var t = GameObject.Find("CurrencyText")?.GetComponent<TMP_Text>()
                ?? GameObject.Find("Currency")?.GetComponent<TMP_Text>();

        if (t == null)
        {
            var all = FindObjectsOfType<TMP_Text>();
            foreach (var tt in all)
            {
                if (tt.gameObject.name.IndexOf("currency", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    t = tt;
                    break;
                }
            }
        }

        if (t != null) Instance.tmpText = t;

        // Awake ran and called Notify()
    }

    // static convenience wrappers
    public static int Get()
    {
        EnsureInstance();
        return Instance != null ? Instance.balance : 0;
    }

    public static void AddAmount(int amount)
    {
        EnsureInstance();
        if (Instance != null) Instance.Add(amount);
    }

    public static bool SpendAmount(int amount)
    {
        EnsureInstance();
        if (Instance != null) return Instance.Spend(amount);
        return false;
    }

    public static void Set(int amount)
    {
        EnsureInstance();
        if (Instance != null) Instance.SetBalance(amount);
    }
}