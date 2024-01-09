using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehavior : MonoBehaviour
{
    //Land id crop planted on
    int landID;

    //What the crop will grow into
    SeedData seedToGrow;

    [Header("Stages of life")]
    public GameObject seed;
    public GameObject wilted;
    private GameObject seedling;
    private GameObject harvestable;

    //Growth points of crops
    public int growth;
    //Growth point to become harvestable
    public int maxGrowth;

    //Plant health without watering
    int maxHealth = GameTimestamp.HoursToMinutes(48);
    public int health;

    public enum CropState
    {
        Seed, Seedling, Harvestable, Wilted
    }
    //Current stage of crop
    public CropState cropState;

    //Call when plant a seed
    public void Plant(int landID, SeedData seedToGrow)
    {
        LoadCrop(landID, seedToGrow, CropState.Seed, 0, 0);

        LandManager.Instance.RegisterCrop(landID, seedToGrow, cropState, growth, health);
    }

    public void LoadCrop(int landID, SeedData seedToGrow, CropState cropState, int growth, int health)
    {
        this.landID = landID;

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

        //Set the growth and health accordingly
        this.growth = growth;
        this.health = health;

        //Check if the plant is regrowable
        if (seedToGrow.regrowable)
        {
            //Get RHB from the GameObject
            RegrowableHarvestBehavior regrowableHarvest = harvestable.GetComponent<RegrowableHarvestBehavior>();

            //Initialise the harvestable
            regrowableHarvest.SetParent(this);
        }

        //Set initial state to seed
        SwitchState(cropState);
    }

    //Grow when watered
    public void Grow()
    {
        //Increase growth point
        growth++;

        //Restore plant's health when watered
        if(health < maxHealth)
        {
            health++;
        }

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

        //Inform LandManager on changes
        LandManager.Instance.OnCropStateChange(landID, cropState, growth, health);
    }

    //Plant progressively wither when soil dry
    public void Wither()
    {
        health--;

        //The crop has germinated when health <= 0
        if (health <= 0 && cropState != CropState.Seed)
        {
            SwitchState(CropState.Wilted);
        }

        //Inform LandManager on changes
        LandManager.Instance.OnCropStateChange(landID, cropState, growth, health);
    }

    //Handle state changes
    void SwitchState(CropState stateToSwitch)
    {
        //Reset
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);
        wilted.SetActive(false);

        //Enable GameObject
        switch (stateToSwitch)
        {
            case CropState.Seed: 
                seed.SetActive(true);
                break;
            case CropState.Seedling: 
                seedling.SetActive(true);

                //Give the seed  health
                health = maxHealth;
                break;
            case CropState.Harvestable:
                harvestable.SetActive(true);

                //If the seed is not regrowable, detach the harvestable, destroy crop GameObject
                if (!seedToGrow.regrowable)
                {
                    //Unparent crop
                    harvestable.transform.parent = null;

                    //Pass the RemoveCrop func to InteractableObject so that it can be called when the player harvest
                    harvestable.GetComponent<InteractableObject>().onInteract.AddListener(RemoveCrop);
                }
                
                break;
            case CropState.Wilted: 
                wilted.SetActive(true);
                break;
        }

        //Set the current crpo state to the state switching to
        cropState = stateToSwitch;
    }

    //Destroy and Deregister crop
    public void RemoveCrop()
    {
        LandManager.Instance.DeregisterCrop(landID);
        Destroy(gameObject);
    }

    //Called when harvest a regrowable crop, reset the state to seedling 
    public void Regrow()
    {
        //Reset growth point
        //Regrowth time in hour
        int hoursToRegrow = GameTimestamp.DaysToHours(seedToGrow.daysToRegrow);
        growth = maxGrowth - GameTimestamp.HoursToMinutes(hoursToRegrow);

        //State back to seedling
        SwitchState(CropState.Seedling);
    }
}
