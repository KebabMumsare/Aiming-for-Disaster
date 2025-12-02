using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    private float damageMultiplier = 1f;

    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }

    public float GetDamage()
    {
        float finalDamage = damage * damageMultiplier;
        return finalDamage;
    }
}
