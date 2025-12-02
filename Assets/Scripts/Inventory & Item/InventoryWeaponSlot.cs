using UnityEngine;

public class InventoryWeaponSlot : InventorySlot
{
    public GameObject WeaponPivot;

    private Weapon equippedWeapon;
    public Weapon EquippedWeapon => equippedWeapon;

    public void EquipWeapon(GameObject newWeapon)
    {
        if (WeaponPivot == null || newWeapon == null)
        {
            Debug.LogWarning("InventoryWeaponSlot missing WeaponPivot or weapon prefab.");
            return;
        }

        UnequipWeapon();

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

    public override void OnDrop(UnityEngine.EventSystems.PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        InventoryItem draggableItem = dropped.GetComponent<InventoryItem>();

        if (draggableItem != null && draggableItem.item != null)
        {
            if (draggableItem.item.type != ItemType.Weapon)
            {
                return; // Do not allow drop
            }
        }

        base.OnDrop(eventData);
    }
}

