using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;              // public & serialized
    public int hp;                           // public & serialized so you can watch it in Inspector

    void Awake() => hp = maxHealth;

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0) Kill();
    }

    public void Kill()
    {
        var drop = GetComponent<Enemydrop>();
        if (drop != null) drop.Die();
        else
        {
            Debug.LogWarning("Enemydrop missing, adding currency directly.");
            Currency.AddAmount(10);
            Destroy(gameObject);
        }
    }
}