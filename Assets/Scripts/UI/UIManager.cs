using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {  get; private set; }

    [Header("Inventory System")]
    public GameObject inventoryPanel;
    public InvetorySlot[] toolSlots;
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
