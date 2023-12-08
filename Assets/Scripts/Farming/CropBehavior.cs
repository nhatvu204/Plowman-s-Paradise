using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehavior : MonoBehaviour
{
    //What the crop will grow into
    SeedData seedToGrow;

    [Header("Stages of life")]
    public GameObject seed;
    private GameObject seedling;
    private GameObject harvestable;

    //Growth points of crops
    public int growth;
    //Growth point to become harvestable
    public int maxGrowth;

    public enum CropState
    {
        Seed, Seedling, Harvestable
    }
    //Current stage of crop
    public CropState cropState;

    //Call when plant a seed
    public void Plant(SeedData seedToGrow)
    {
        //Save seed info
        this.seedToGrow = seedToGrow;

        //Instantiate the seedling GameObject
        seedling = Instantiate(seedToGrow.seedling, transform);

        //Access the crop item data
        ItemData cropToYield = seedToGrow.cropToYield;

        //Instantiate the harvestable GameObject
        harvestable = Instantiate(cropToYield.gameModel, transform);

        //daysToGrow to hours
        int hoursToGrow = GameTimestamp.DaysToHours(seedToGrow.daysToGrow);
        //daysToGrow to minutes
        maxGrowth = GameTimestamp.HoursToMinutes(hoursToGrow);

        //Set initial state to seed
        SwitchState(CropState.Seed);
    }

    //Grow when watered
    public void Grow()
    {
        //Increase growth point
        growth++;

        //Seed to seedling at 50%
        if (growth >= maxGrowth / 2 && cropState == CropState.Seed)
        {
            SwitchState(CropState.Seedling);
        }

        //Seeling to harvestable
        if(growth >= maxGrowth && cropState == CropState.Seedling)
        {
            SwitchState(CropState.Harvestable);
        }
    }

    //Handle state changes
    void SwitchState(CropState stateToSwitch)
    {
        //Reset
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);

        //Enable GameObject
        switch (stateToSwitch)
        {
            case CropState.Seed: 
                seed.SetActive(true);
                break;
            case CropState.Seedling: 
                seedling.SetActive(true);
                break;
            case CropState.Harvestable:
                harvestable.SetActive(true);
                //Unparent crop
                harvestable.transform.parent = null;
                
                Destroy(gameObject);
                break;
        }

        //Set the current crpo state to the state switching to
        cropState = stateToSwitch;
    }
}
