using UnityEngine;

public class InventoryWeaponSlot : InventorySlot
{
    public GameObject WeaponPivot;

    public void EquipWeapon(GameObject newWeapon)
{
   Debug.Log("Equipping weapon: " + newWeapon.name);
    var clone = Instantiate(newWeapon, WeaponPivot.transform);
    
}   
    public void UnequipWeapon()
    {
        Destroy(WeaponPivot.transform.GetChild(0).gameObject);
    }
}
