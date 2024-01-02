using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : InteractableObject
{
    public List<ItemData> shopItems;

    public static void Purchase(ItemData item, int quantity)
    {
        int totalCost = item.cost * quantity;

        if(PlayerStats.Money >= totalCost)
        {
            //Deduct player's money
            PlayerStats.Spend(totalCost);
            //Create an ItemSlotData for purchased items
            ItemSlotData purchasedItem = new ItemSlotData(item, quantity);

            //Send item to player's inventory
            InventoryManager.Instance.ShopToInventory(purchasedItem);
        }
    }

    public override void PickUp()
    {
        UIManager.Instance.OpenShop(shopItems);
    }
}
