using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveState
{
    //Farm data
    public List<LandSaveState> landData;
    public List<CropSaveState> cropData;

    //Inventory
    public ItemSlotSaveData[] toolSlots;
    public ItemSlotSaveData[] itemSlots;

    public ItemSlotSaveData equippedItemSlot;
    public ItemSlotSaveData equippedToolSlot;

    //Time
    public GameTimestamp timestamp;

    //Player Stats
    public int money;

    public GameSaveState(List<LandSaveState> landData, List<CropSaveState> cropData, ItemSlotData[] toolSlots, ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot, ItemSlotData equippedToolSlot, GameTimestamp timestamp, int money)
    {
        this.landData = landData;
        this.cropData = cropData;
        this.toolSlots = ItemSlotData.SerializeArray(toolSlots);
        this.itemSlots = ItemSlotData.SerializeArray(itemSlots);
        this.equippedItemSlot = ItemSlotData.SerializeData(equippedItemSlot);
        this.equippedToolSlot = ItemSlotData.SerializeData(equippedToolSlot);
        this.timestamp = timestamp;
        this.money = money;
    }
}
