using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvetorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    ItemData itemToDisplay;
    int quantity;

    public Image itemDisplayImage;
    public Text quantityText;

    public enum InventoryType
    {
        Item, Tool
    }
    //Determine which inventory section this slot is a part of
    public InventoryType inventoryType;

    int slotIndex;

    public void Display(ItemSlotData itemSlot)
    {
        itemToDisplay = itemSlot.itemData;
        quantity = itemSlot.quantity;

        //Quantity text doesn't display by default
        quantityText.text = "";

        //Check if there is an item to display 
        if (itemToDisplay != null)
        {
            itemDisplayImage.sprite = itemToDisplay.thumbnail;

            //Display stack quantity if more than 1
            if(quantity > 1)
            {
                quantityText.text = quantity.ToString();
            }

            itemDisplayImage.gameObject.SetActive(true);
            return;
        }

        itemDisplayImage.gameObject.SetActive(false);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //Move item from inventory to hand
        InventoryManager.Instance.InventoryToHand(slotIndex, inventoryType);
    }

    //Set the Slot index
    public void AssignIndex(int slotIndex)
    {
        this.slotIndex = slotIndex;
    }

    //Display item info on mouse over
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(itemToDisplay);
    }

    //Reset item info on mouse leave
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(null);
    }
}
