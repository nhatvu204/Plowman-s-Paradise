using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {  get; private set; }

    [Header("Status Bar")]
    public Image toolEquipSlot;


    [Header("Inventory System")]
    public GameObject inventoryPanel;

    //The tool equip slot UI on the Inventory Panel
    public HandInventorySlot toolHandSlot;
    public InvetorySlot[] toolSlots;

    //The item equip slot UI on the Inventory Panel
    public HandInventorySlot itemHandSlot;
    public InvetorySlot[] itemSlots;

    //Item info box
    public Text itemNameText;
    public Text itemDescriptionText;

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

    private void Start()
    {
        RenderInventory();
        AssignSlotIndexes();
    }

    //Iterate through the slot UI elements and assign it its reference slot index
    public void AssignSlotIndexes()
    {
        for (int i = 0; i < toolSlots.Length; i++)
        {
            toolSlots[i].AssignIndex(i);
            itemSlots[i].AssignIndex(i);
        }
    }

    //Render the inventory screen to reflect hte Player's Inventory
    public void RenderInventory()
    {
        //Get inventory tool slots from Inventory Manager 
        ItemData[] inventoryToolSlots = InventoryManager.Instance.tools;

        //Get inventory item slots from Inventory Manager
        ItemData[] inventoryItemSlots = InventoryManager.Instance.items;

        //Render tool section
        RenderInventoryPanel(inventoryToolSlots, toolSlots);

        //Render item section
        RenderInventoryPanel(inventoryItemSlots, itemSlots);

        //Render the equipped slots
        toolHandSlot.Display(InventoryManager.Instance.equippedTool);
        itemHandSlot.Display(InventoryManager.Instance.equippedItem);

        //Get tool equip from inventory manager
        ItemData equippedTool = InventoryManager.Instance.equippedTool;

        //Check if there is an item to display 
        if (equippedTool != null)
        {
            toolEquipSlot.sprite = equippedTool.thumbnail;

            toolEquipSlot.gameObject.SetActive(true);
            return;
        }

        toolEquipSlot.gameObject.SetActive(false);
    }

    //Interate through a slot in a section and display them in the UI
    void RenderInventoryPanel(ItemData[] slots, InvetorySlot[] uiSlots)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i].Display(slots[i]);
        } 
    }

    public void ToggleInventoryPanel()
    {
        //If the panel is hidden, show it and vice versa
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

        RenderInventory();
    }

    //Display item info
    public void DisplayItemInfo(ItemData data)
    {
        //Reset if null
        if (data == null)
        {
            itemNameText.text = "";
            itemDescriptionText.text = "";

            return;
        }

        itemNameText.text = data.name;
        itemDescriptionText.text = data.description;
    }
}
