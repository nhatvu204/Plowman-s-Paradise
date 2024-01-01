using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShippingBin : InteractableObject
{
    public static int hourToShip = 18;
    public static List<ItemSlotData> itemsToShip = new List<ItemSlotData>();

    public override void PickUp()
    {
        //Get item data 
        ItemData handSlotItem = InventoryManager.Instance.GetEquippedSlotItem(InvetorySlot.InventoryType.Item);

        //Check if the player holding something
        if (handSlotItem == null) return;

        //Confirm selling by prompt
        UIManager.Instance.TriggerYesNoPrompt($"Do you want to sell {handSlotItem.name} ?", PlaceItemsInShippingBin);

        PlaceItemsInShippingBin();
    }

    void PlaceItemsInShippingBin()
    {
        //Get ItemSlotData of what is in player's hand
        ItemSlotData handSlot = InventoryManager.Instance.GetEquippedSlot(InvetorySlot.InventoryType.Item);

        //Add item to itemsToShipList
        itemsToShip.Add(new ItemSlotData(handSlot));

        //Remove item from hand
        handSlot.Empty();

        //Update the changes
        InventoryManager.Instance.RenderHand();

        foreach(ItemSlotData item in itemsToShip)
        {
            Debug.Log($"In the bin: {item.itemData.name} x {item.quantity}");
        }
    }

    public static void ShipItems()
    {
        //Calculate the money to receive after shipping items in bin
        int moneyToReceive = TallyItems(itemsToShip);
        //Give player money
        PlayerStats.Earn(moneyToReceive);
        //Empty the shipping bin
        itemsToShip.Clear();
    }

    public static int TallyItems(List<ItemSlotData> items)
    {
        int total = 0;
        foreach(ItemSlotData item in items)
        {
            //Get total cost
            total += item.quantity * item.itemData.cost;
        }

        return total;
    }
}
