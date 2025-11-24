using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;
    private InventoryManager inventoryManager;
    private int slotIndex = -1;

    private void Awake()
    {
        Deselect();
    }
    public void Initialise(InventoryManager manager, int index)
    {
        inventoryManager = manager;
        slotIndex = index;
    }

    public void Select()
    {
        image.color = selectedColor;
    }

    public void Deselect()
    {
        image.color = notSelectedColor;
    }
    public virtual void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            InventoryItem draggableItem = dropped.GetComponent<InventoryItem>();

            // Check if this is NOT a weapon slot and the item IS a weapon
            if (!(this is InventoryWeaponSlot) && draggableItem.item.type == ItemType.Weapon)
            {
                return; // Do not allow drop
            }

            draggableItem.parentAfterDrag = transform;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (inventoryManager != null && slotIndex >= 0)
        {
            inventoryManager.ChangeSelectedSlot(slotIndex);
        }
    }
}
