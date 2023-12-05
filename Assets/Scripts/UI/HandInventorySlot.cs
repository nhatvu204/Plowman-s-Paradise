using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandInventorySlot : InvetorySlot
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        //Move item from Hand to Inventory
        InventoryManager.Instance.HandToInventory(inventoryType);
    }
}
