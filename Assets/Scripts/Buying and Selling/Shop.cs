using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Shop : InteractableObject
{
    public List<ItemData> shopItems;

    public static void Purchase(ItemData item, int quantity)
    {
        int totalCost = item.cost * quantity;

        if (PlayerStats.Money >= totalCost)
        {
            //Reduict from player's money
            PlayerStats.Spend(totalCost);
            //Create an ItemSlotData for the purchased item
            ItemSlotData purchasedItem = new ItemSlotData(item, quantity);

            //Send item to player's inventory
            InventoryManager.Instance.ShopToInventory(purchasedItem);
        }
    } 

    public override void PickUp()
    {
        Debug.Log("Purchasing");
        Purchase(item, 2);
    }
}
