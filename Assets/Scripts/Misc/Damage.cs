using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    private float damageMultiplier = 1f;

    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
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
        return damage * damageMultiplier;
    }
}
