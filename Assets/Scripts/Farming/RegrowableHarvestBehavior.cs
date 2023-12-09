using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegrowableHarvestBehavior : InteractableObject
{
    CropBehavior parentCrop;

    //Set the parent crop
    public void SetParent(CropBehavior parentCrop)
    {
        this.parentCrop = parentCrop;
    }

    public override void PickUp()
    {
        //Set player's inventory to the item
        InventoryManager.Instance.equippedItem = item;
        //Update in the scene
        InventoryManager.Instance.RenderHand();

        //Set the parent crop back to seedling
        parentCrop.Regrow();
    }
}
