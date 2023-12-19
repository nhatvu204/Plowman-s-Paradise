using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameStateManager : MonoBehaviour, ITimeTracker
{
    public static GameStateManager Instance { get; private set; }

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

    // Start is called before the first frame update
    void Start()
    {
        //Add this to TimeManager tracker list
        TimeManager.Instance.RegisterTracker(this);
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        //Update the Land and Crop Save States as long as the player is outside of the Farm scene
        if (SceneTransitionManager.Instance.currentLocation != SceneTransitionManager.Location.Farm)
        {
            //Retrieve the Land and Farm data from the static variable
            List<LandSaveState> landData = LandManager.farmData.Item1;
            List<CropSaveState> cropData = LandManager.farmData.Item2;

            //If no crop planted, don't update
            if (cropData.Count == 0) return;

            for (int i = 0; i < cropData.Count; i++)
            {
                //Get the crop and corresponding land data
                CropSaveState crop = cropData[i];
                LandSaveState land = landData[crop.landID];

                //Check if crop is wilted
                if (crop.cropState == CropBehavior.CropState.Wilted) continue;

                //Update Land's state
                land.ClockUpdate(timestamp);

                //Update Crop's state based on the Land's state 
                if(land.landStatus == Land.LandStatus.Watered)
                {
                    crop.Grow();
                }
                else if (crop.cropState != CropBehavior.CropState.Seed)
                {
                    crop.Wither();
                }

                //Update the element in the array
                cropData[i] = crop;
                landData[crop.landID] = land;

            }
        }
    }

    public void Sleep()
    {
        //Calculate how many ticks to advance the time to 6am

        //Get the time stamp of 6am the next day
        GameTimestamp timestampOfNextDay = TimeManager.Instance.GetGameTimestamp();
        timestampOfNextDay.day += 1;
        timestampOfNextDay.hour = 6;
        timestampOfNextDay.minute = 0;

        TimeManager.Instance.SkipTime(timestampOfNextDay);

        //Save
        SaveManager.Save(ExportSaveState());
    }

    public GameSaveState ExportSaveState()
    {
        //Retrieve farm data
        List<LandSaveState> landData = LandManager.farmData.Item1;
        List<CropSaveState> cropData = LandManager.farmData.Item2;

        //Retrieve inventory data
        ItemSlotData[] toolSlots = InventoryManager.Instance.GetInventorySlots(InvetorySlot.InventoryType.Tool);
        ItemSlotData[] itemSlots = InventoryManager.Instance.GetInventorySlots(InvetorySlot.InventoryType.Item);

        ItemSlotData equippedToolSlot = InventoryManager.Instance.GetEquippedSlot(InvetorySlot.InventoryType.Tool);
        ItemSlotData equippedItemSlot = InventoryManager.Instance.GetEquippedSlot(InvetorySlot.InventoryType.Item);

        //Time
        GameTimestamp timestamp = TimeManager.Instance.GetGameTimestamp();

        return new GameSaveState(landData, cropData, toolSlots, itemSlots, equippedItemSlot, equippedToolSlot, timestamp);
    }

    public void LoadSave()
    {
        //Set the scene to player home
        SceneTransitionManager.Instance.SwitchLocation(SceneTransitionManager.Location.PlayerHome);
        //Retrieve the loaded save
        GameSaveState save = SaveManager.Load();

        //Load up the parts
        //Time
        TimeManager.Instance.LoadTime(save.timestamp);

        //Inventory
        ItemSlotData[] toolSlots = ItemSlotData.DeserializeArray(save.toolSlots);
        ItemSlotData equippedToolSlot = ItemSlotData.DeserializeData(save.equippedToolSlot);
        ItemSlotData[] itemSlots = ItemSlotData.DeserializeArray(save.itemSlots);
        ItemSlotData equippedItemSlot = ItemSlotData.DeserializeData(save.equippedItemSlot);
        InventoryManager.Instance.LoadInventory(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);

        //Farming data
        LandManager.farmData = new System.Tuple<List<LandSaveState>, List<CropSaveState>>(save.landData, save.cropData);
    }
}