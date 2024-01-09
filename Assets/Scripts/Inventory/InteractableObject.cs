using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    //Item info the GameObject present
    public ItemData item;
    public UnityEvent onInteract = new UnityEvent();

    public virtual void PickUp()
    {
        //Call the onInteract callback
        onInteract?.Invoke();

        //Set player's inventory to the item
        InventoryManager.Instance.EquipHandSlot(item);

        //Update in the scene
        InventoryManager.Instance.RenderHand();
        //Destroy this Instance
        Destroy(gameObject);
    }
}
