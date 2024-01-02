using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShippingBin : InteractableObject
{
    public static int hourToShip = 18;
    public static List<ItemSlotData> itemsToShip = new List<ItemSlotData>();

    public override void PickUp()
    {
        //Get ItemData from hand
        ItemData handSlotItem = InventoryManager.Instance.GetEquippedSlotItem(InvetorySlot.InventoryType.Item);

        //If nothing in hand, nothing happen
        if (handSlotItem == null) return;

        //Confirm if the player want to sell
        UIManager.Instance.TriggerYesNoPrompt($"Do you want to sell {handSlotItem.name} ?", PlaceItemInShippingBin);

        PlaceItemInShippingBin();
    }

    void PlaceItemInShippingBin()
    {
        //Get ItemSlotData from hand
        ItemSlotData handSlot = InventoryManager.Instance.GetEquippedSlot(InvetorySlot.InventoryType.Item);

        //Add item to itemsToShip list
        itemsToShip.Add(new ItemSlotData(handSlot));

        //Remove item from hand
        handSlot.Empty();

        //Update changes
        InventoryManager.Instance.RenderHand();

        foreach (ItemSlotData item in itemsToShip)
        {
            Debug.Log($"In the bin: {item.itemData.name} x {item.quantity}");
        }
    }

    public static void ShipItems()
    {
        //Calculate how much player earn after shipping
        int moneyToReceive = TallyItems(itemsToShip);

        PlayerStats.Earn(moneyToReceive);

        //Empty shipping bin
        itemsToShip.Clear();
    }

    static int TallyItems(List<ItemSlotData> items)
    {
        int total = 0;
        foreach(ItemSlotData item in items)
        {
            total = item.quantity * item.itemData.cost;
        }
        return total;
    } 
}
