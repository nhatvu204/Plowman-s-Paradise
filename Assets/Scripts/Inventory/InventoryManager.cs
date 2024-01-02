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

    //Full list of items
    public ItemIndex itemIndex;

    [Header("Tools")]
    //Tool Slots
    [SerializeField] 
    private ItemSlotData[] toolSlots = new ItemSlotData[8];
    //Tool in hand
    [SerializeField] 
    private ItemSlotData equippedToolSlot = null;

    [Header("Items")]
    //Item Slots
    [SerializeField] 
    private ItemSlotData[] itemSlots = new ItemSlotData[8];
    //Item in hand
    [SerializeField] 
    private ItemSlotData equippedItemSlot = null;

    //The transform for the player to hold item in the scene
    public Transform handPoint;

    //Load the inventory from a save
    public void LoadInventory(ItemSlotData[] toolSlots, ItemSlotData equippedToolSlot, ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot)
    {
        this.toolSlots = toolSlots;
        this.itemSlots = itemSlots;
        this.equippedItemSlot = equippedItemSlot;
        this.equippedToolSlot = equippedToolSlot;

        //Update in the UI and the scene
        UIManager.Instance.RenderInventory();
        RenderHand();
    }

    //Equipping
    //Handles movement of item from Inventory to Hand
    public void InventoryToHand(int slotIndex, InvetorySlot.InventoryType inventoryType)
    {
        //The slot to equip (Tool by default)
        ItemSlotData handToEquip = equippedToolSlot;
        //The array to change
        ItemSlotData[] inventoryToAlter = toolSlots;

        if (inventoryType == InvetorySlot.InventoryType.Item)
        {
            //Change the slot to item
            handToEquip = equippedItemSlot;
            inventoryToAlter = itemSlots;
        }

        //Check if stackable
        if (handToEquip.Stackable(inventoryToAlter[slotIndex]))
        {
            ItemSlotData slotToAlter = inventoryToAlter[slotIndex];

            //Add to the hand slot
            handToEquip.AddQuantity(slotToAlter.quantity);

            //Empty the inventory slot
            slotToAlter.Empty();
        }
        else
        {
            //Not stackable
            //Cache the Inventory ItemSlotData
            ItemSlotData slotToEquip = new ItemSlotData(inventoryToAlter[slotIndex]);

            //Change the inventory slot to the hands'
            inventoryToAlter[slotIndex] = new ItemSlotData(handToEquip);

            EquipHandSlot(slotToEquip);
        }
        //Update the changes in the scene
        if (inventoryType == InvetorySlot.InventoryType.Item)
        {
            RenderHand();
        }

        //Update the channges to the UI
        UIManager.Instance.RenderInventory();

    }

    //Handles movement of item from Hand to Inventory
    public void HandToInventory (InvetorySlot.InventoryType inventoryType)
    {
        //The slot to move from (Tool by default)
        ItemSlotData handSlot = equippedToolSlot;
        //The array to change
        ItemSlotData[] inventoryToAlter = toolSlots;

        if (inventoryType == InvetorySlot.InventoryType.Item)
        {
            handSlot = equippedItemSlot;
            inventoryToAlter = itemSlots;
        }

        //Try stacking the hand slot
        //Check if can't stack
        if (!StackItemToInventory(handSlot, inventoryToAlter))
        {
            //Find an empty slot to put
            //Iterate through each inventory slot and find an empty slot
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    //Send the equipped item
                    inventoryToAlter[i] = new ItemSlotData(handSlot);

                    //Remove item from Hand
                    handSlot.Empty();
                    break;
                }
            }
        }

        //Update the changes in the scene
        if (inventoryType == InvetorySlot.InventoryType.Item)
        {
            RenderHand();
        }

        //Update the channges to the UI
        UIManager.Instance.RenderInventory();
    }

    //Iterate through each of the items in the inventory to see if it can be stacked 
    public bool StackItemToInventory(ItemSlotData itemSlot, ItemSlotData[] inventoryArray)
    {
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            if (inventoryArray[i].Stackable(itemSlot))
            {
                //Add to the inventory slot's stack
                inventoryArray[i].AddQuantity(itemSlot.quantity);
                //Empty the item slot
                itemSlot.Empty();
                return true;
            }
        }

        //Can't find slot to stack
        return false;
    }

    //Handles movement of item from Shop to Inventory
    public void ShopToInventory(ItemSlotData itemSlotToMove)
    {
        //The inventory array to change
        ItemSlotData[] inventoryToAlter = IsTool(itemSlotToMove.itemData) ? toolSlots : itemSlots;

        //Try stacking the hand slot
        //Check if can't stack
        if (!StackItemToInventory(itemSlotToMove, inventoryToAlter))
        {
            //Find an empty slot to put
            //Iterate through each inventory slot and find an empty slot
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    //Send the equipped item
                    inventoryToAlter[i] = new ItemSlotData(itemSlotToMove);
                    break;
                }
            }
        }

        //Update the channges to the UI and the scene
        UIManager.Instance.RenderInventory();
        RenderHand();
    }

    //Render equipped item in the scene
    public void RenderHand()
    {
        //Reset objects in hand
        if (handPoint.childCount > 0)
        {
            Destroy(handPoint.GetChild(0).gameObject);
        }

        //Check if anything equipped
        if (SlotEquipped(InvetorySlot.InventoryType.Item))
        {
            Instantiate(GetEquippedSlotItem(InvetorySlot.InventoryType.Item).gameModel, handPoint);
        }
    }

    //Inventory slot data

    #region Gets and Checks
    //Get the slot item (ItemData)
    public ItemData GetEquippedSlotItem(InvetorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InvetorySlot.InventoryType.Item)
        {
            return equippedItemSlot.itemData;
        }
        return equippedToolSlot.itemData;
    }

    //Get function for the slots (ItemSlotData)
    public ItemSlotData GetEquippedSlot(InvetorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InvetorySlot.InventoryType.Item)
        {
            return equippedItemSlot;
        }
        return equippedToolSlot;
    }

    //Get function for the Inventory slots
    public ItemSlotData[] GetInventorySlots(InvetorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InvetorySlot.InventoryType.Item)
        {
            return itemSlots;
        }
        return toolSlots;
    }

    //Check if a hand slot has an item
    public bool SlotEquipped(InvetorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InvetorySlot.InventoryType.Item)
        {
            return !equippedItemSlot.IsEmpty();
        }
        return !equippedToolSlot.IsEmpty();
    }

    //Check if the item is a tool
    public bool IsTool(ItemData item)
    {
        //Try to cast it as equipment
        EquipmentData equipment = item as EquipmentData;
        if (equipment != null)
        {
            return true;
        }

        //Try to cast it as a seed
        SeedData seed = item as SeedData;
        //If not null it's a seed
        return seed != null;
    }
    #endregion

    //Equip the hand slot with an ItemData (Will overwrite the slot)
    public void EquipHandSlot(ItemData item)
    {
        if (IsTool(item))
        {
            equippedToolSlot = new ItemSlotData(item);
        }
        else
        {
            equippedItemSlot = new ItemSlotData(item);
        }
    }

    //Equip the hand slot with an ItemSlotData (Will overwrite the slot)
    public void EquipHandSlot(ItemSlotData itemSlot)
    {
        //Get the item data from the slot
        ItemData item = itemSlot.itemData;
        if (IsTool(item))
        {
            equippedToolSlot = new ItemSlotData(itemSlot);
        }
        else
        {
            equippedItemSlot = new ItemSlotData(itemSlot);
        }
    }

    public void ConsumeItem(ItemSlotData itemSlot)
    {
        if (itemSlot.IsEmpty())
        {
            Debug.LogError("There is nothing to consume!");
            return;
        }
        
        //Use up one of the item slots
        itemSlot.Remove();
        //Refresh inventory
        RenderHand();
        UIManager.Instance.RenderInventory();
    }

    #region Inventory slot validation
    public void OnValidate()
    {
        //Validate hand slots
        ValidateInventorySlot(equippedItemSlot);
        ValidateInventorySlot(equippedToolSlot);

        //Validate the slots in the inventory
        ValidateInventorySlots(itemSlots);
        ValidateInventorySlots(toolSlots);
    }

    //Automatically set the quality to 1 when giving the ItemData in the Inspector
    void ValidateInventorySlot(ItemSlotData slot)
    {
        if (slot.itemData != null && slot.quantity == 0)
        {
            slot.quantity = 1;
        }
    }

    //Validate arrays
    void ValidateInventorySlots(ItemSlotData[] array)
    {
        foreach (ItemSlotData slot in array)
        {
            ValidateInventorySlot(slot);
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
