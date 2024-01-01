using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ITimeTracker
{
    public static UIManager Instance {  get; private set; }

    [Header("Status Bar")]
    public Image toolEquipSlot;
    //Tool Quantity text on the status bar
    public Text toolQuantityText;

    //Time UI
    public Text timeText;
    public Text dateText;

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

    [Header("Screen Transition")]
    public GameObject fadeIn;
    public GameObject fadeOut;

    [Header("Yes No Prompt")]
    public YesNoPrompt yesNoPrompt;

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

        //Add UIManager to the list of objects TimeManager will notify when the time updates
        TimeManager.Instance.RegisterTracker(this);
    }

    public void TriggerYesNoPrompt(string message, System.Action onYesCallback)
    {
        //Set active yes no prompt
        yesNoPrompt.gameObject.SetActive(true);

        yesNoPrompt.CreatePrompt(message, onYesCallback);
    } 

    #region FadeIn and FadeOut Transition
    public void OnFadeInComplete()
    {
        
    }

    public void OnFadeOutComplete()
    {

    }
    #endregion

    #region Inventory
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
        //Get the respective slots to process 
        ItemSlotData[] inventoryToolSlots = InventoryManager.Instance.GetInventorySlots(InvetorySlot.InventoryType.Tool);
        ItemSlotData[] inventoryItemSlots = InventoryManager.Instance.GetInventorySlots(InvetorySlot.InventoryType.Item);

        //Render tool section
        RenderInventoryPanel(inventoryToolSlots, toolSlots);

        //Render item section
        RenderInventoryPanel(inventoryItemSlots, itemSlots);

        //Render the equipped slots
        toolHandSlot.Display(InventoryManager.Instance.GetEquippedSlot(InvetorySlot.InventoryType.Tool));
        itemHandSlot.Display(InventoryManager.Instance.GetEquippedSlot(InvetorySlot.InventoryType.Item));

        //Get tool equip from inventory manager
        ItemData equippedTool = InventoryManager.Instance.GetEquippedSlotItem(InvetorySlot.InventoryType.Tool);

        //Text empty by default
        toolQuantityText.text = "";

        //Check if there is an item to display 
        if (equippedTool != null)
        {
            toolEquipSlot.sprite = equippedTool.thumbnail;

            toolEquipSlot.gameObject.SetActive(true);

            //Get quantity
            int quantity = InventoryManager.Instance.GetEquippedSlot(InvetorySlot.InventoryType.Tool).quantity;
            if (quantity > 1)
            {
                toolQuantityText.text = quantity.ToString();
            }
            return;
        }

        toolEquipSlot.gameObject.SetActive(false);
    }

    //Interate through a slot in a section and display them in the UI
    void RenderInventoryPanel(ItemSlotData[] slots, InvetorySlot[] uiSlots)
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
    #endregion

    #region Time
    //Callback to handle the UI for time
    public void ClockUpdate(GameTimestamp timestamp)
    {
        //Handle time
        int hour = timestamp.hour;
        int minute = timestamp.minute;

        string prefix = "AM";

        if (hour > 12)
        {
            prefix = "PM";
            hour -= 12;
        }

        //Format
        timeText.text = prefix + " " + hour + ":" + minute.ToString("00");

        //Handle date
        int day = timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfTheWeek = timestamp.GetDayOfTheWeek().ToString();

        //Format
        dateText.text = season + " " + day + " (" + dayOfTheWeek + ")";
    }
    #endregion


}
