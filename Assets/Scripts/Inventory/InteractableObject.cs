using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    //Item info the GameObject present
    public ItemData item;

    public virtual void PickUp()
    {
        //Set player's inventory to the item
        InventoryManager.Instance.equippedItem = item;
        //Update in the scene
        InventoryManager.Instance.RenderHand();
        //Destroy this Instance
        Destroy(gameObject);
    }
}
