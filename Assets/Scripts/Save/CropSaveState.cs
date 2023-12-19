using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CropBehavior;

[System.Serializable]
public struct CropSaveState 
{
    //The index of the land the crop is planted on
    public int landID;

    public string seedToGrow;
    public CropBehavior.CropState cropState;
    public int growth;
    public int health;

    public CropSaveState(int landID, string seedToGrow, CropBehavior.CropState cropState, int growth, int health)
    {
        this.landID = landID;
        this.seedToGrow = seedToGrow;
        this.cropState = cropState;
        this.growth = growth;
        this.health = health;
    }

    public void Grow()
    {
        //Increase growth point
        growth++;

        //seedToGrow string to SeedData
        SeedData seedInfo = (SeedData) InventoryManager.Instance.itemIndex.GetItemFromString(seedToGrow);

        //Get maxHealth and growth from SeedData
        int maxGrowth = GameTimestamp.HoursToMinutes (GameTimestamp.DaysToHours(seedInfo.daysToGrow));
        int maxHealth = GameTimestamp.HoursToMinutes(48);

        //Restore plant's health when watered
        if (health < maxHealth)
        {
            health++;
        }

        //Seed to seedling at 50%
        if (growth >= maxGrowth / 2 && cropState == CropBehavior.CropState.Seed)
        {
            cropState = CropBehavior.CropState.Seedling;
        }

        //Seeling to harvestable
        if (growth >= maxGrowth && cropState == CropBehavior.CropState.Seedling)
        {
            cropState = CropBehavior.CropState.Harvestable;
        }
    }

    public void Wither()
    {
        health--;

        //The crop has germinated when health <= 0
        if (health <= 0 && cropState != CropBehavior.CropState.Seed)
        {
            cropState = CropBehavior.CropState.Wilted;
        }
    }
}
