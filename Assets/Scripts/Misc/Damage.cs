using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float GetDamage()
    {
        return damage;
    }
}
