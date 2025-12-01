using UnityEngine;

public class CurrencyItem : MonoBehaviour
{
    // in this script i will only have logic for picking up currency items that are on the ground
    // the currency items will be destroyed when picked up
    // the currency will be added to the player's "balance" (bullets &or magazines)

    public float bulletValue = 0;
    public float magazineValue = 0;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Currencies>().AddBullets(bulletValue);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Currencies>().AddMagazines(magazineValue);
        }
    }
}
