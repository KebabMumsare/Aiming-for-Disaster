using UnityEngine;

public class InventoryWeaponSlot : InventorySlot
{
    public GameObject WeaponPivot;

    private Weapon equippedWeapon;

    public void EquipWeapon(GameObject newWeapon)
    {
        if (WeaponPivot == null || newWeapon == null)
        {
            Debug.LogWarning("InventoryWeaponSlot missing WeaponPivot or weapon prefab.");
            return;
        }

        UnequipWeapon();

        Debug.Log("Equipping weapon: " + newWeapon.name);
        var clone = Instantiate(newWeapon, WeaponPivot.transform);
        equippedWeapon = clone.GetComponent<Weapon>();
        if (equippedWeapon != null)
        {
            equippedWeapon.OnEquipped(WeaponPivot.transform);
        }
        else
        {
            Debug.LogWarning("Equipped object does not contain a Weapon component.");
        }
    }

    public void UnequipWeapon()
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.OnUnequipped();
        }

        if (WeaponPivot != null)
        {
            for (int i = WeaponPivot.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(WeaponPivot.transform.GetChild(i).gameObject);
            }
        }

        equippedWeapon = null;
    }
}

