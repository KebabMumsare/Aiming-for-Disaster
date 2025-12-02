using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    private float damageMultiplier = 1f;

    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
        Debug.Log($"Damage component multiplier set to {damageMultiplier}. Base damage: {damage}");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float GetDamage()
    {
        float finalDamage = damage * damageMultiplier;
        Debug.Log($"GetDamage called. Base: {damage}, Multiplier: {damageMultiplier}, Final: {finalDamage}");
        return finalDamage;
    }
}
