using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        //If there is more than 1 instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }
    }

    [Header("Tools")]
    //Tool Slots
    public ItemData[] tools = new ItemData[8];
    //Tool in hand
    public ItemData equippedTool = null;

    [Header("Items")]
    //Item Slots
    public ItemData[] items = new ItemData[8];
    //Item in hand
    public ItemData equippedItem = null;

    //Equipping

    //Handles movement of item from Inventory to Hand
    public void InventoryToHand(int slotIndex, InvetorySlot.InventoryType inventoryType)
    {
        if(inventoryType == InvetorySlot.InventoryType.Item)
        {
            //Cache the Inventory slot ItemData from InventoryManager 
            ItemData itemToEquip = items[slotIndex];

            //Change the Inventory Slot to Hand's
            items[slotIndex] = equippedItem;

            //Change the Hand's Slot to the Inventory Slot's
            equippedItem = itemToEquip;
        }
        else
        {
            //Cache the Inventory slot ItemData from InventoryManager 
            ItemData toolToEquip = tools[slotIndex];

            //Change the Inventory Slot to Hand's
            tools[slotIndex] = equippedTool;

            //Change the Hand's Slot to the Inventory Slot's
            equippedTool = toolToEquip;
        }

        //Update the channges to the UI
        UIManager.Instance.RenderInventory();
    }

    //Handles movement of item from Hand to Inventory
    public void HandToInventory (InvetorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InvetorySlot.InventoryType.Item)
        {
            //Iterate through each inventory slot and find an empty slot
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    //Send the equipped item
                    items[i] = equippedItem;

                    //Remove item from Hand
                    equippedItem = null;
                    break;
                }
            }
        }
        else
        {
            //Iterate through each inventory slot and find an empty slot
            for (int i = 0; i < tools.Length; i++)
            {
                if (tools[i] == null)
                {
                    //Send the equipped tool
                    tools[i] = equippedTool;

                    //Remove tool from Hand
                    equippedTool = null;
                    break;
                }
            }
        }

        //Update changes
        UIManager.Instance.RenderInventory();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
