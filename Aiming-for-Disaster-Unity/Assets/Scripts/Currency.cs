// filepath: Aiming-for-Disaster-Unity/Aiming-for-Disaster-Unity/Assets/Scripts/Currency.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    private static int _balance = 0;
    private static HashSet<Action<int>> _listeners = new HashSet<Action<int>>();

    private static void Notify()
    {
        foreach (var listener in _listeners)
        {
            try
            {
                listener(_balance);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        // auto-update a UI element with tag "Currency" if present (optional)
        var currencyText = GameObject.FindWithTag("Currency")?.GetComponent<UnityEngine.UI.Text>();
        if (currencyText != null)
        {
            currencyText.text = _balance.ToString();
        }
    }

    public static int GetBalance()
    {
        return _balance;
    }

    public static void Add(int amount = 0)
    {
        amount = Mathf.FloorToInt(Mathf.Max(0, amount));
        if (amount <= 0) return;
        _balance += amount;
        Notify();
    }

    public static bool Spend(int amount = 0)
    {
        amount = Mathf.FloorToInt(Mathf.Max(0, amount));
        if (amount <= 0) return false;
        if (amount > _balance) return false;
        _balance -= amount;
        Notify();
        return true;
    }

    public static void SetBalance(int amount = 0)
    {
        _balance = Mathf.Max(0, Mathf.FloorToInt(amount));
        Notify();
    }

    public static Action<int> OnChange(Action<int> fn)
    {
        if (fn != null)
        {
            _listeners.Add(fn);
            // call immediately with current value
            fn(_balance);
            return fn;
        }
        return null;
    }
}