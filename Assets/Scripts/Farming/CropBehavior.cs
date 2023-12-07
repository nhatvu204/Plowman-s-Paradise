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

    public enum CropState
    {
        Seed, Seedling, Harvestable
    }

    //Current state
    public CropState cropState;

    //Call when plant a seed
    public void Plant(SeedData seedToGrow)
    {
        //Save seed info
        this.seedToGrow = seedToGrow;

        //Instantiate seedling GameObject
        seedling = Instantiate(seedToGrow.seedling, transform);
        
        //Access crop item data
        ItemData cropToYield = seedToGrow.cropToYield;

        //Instantiate harvestable crop
        harvestable = Instantiate(cropToYield.gameModel, transform);

        //Set initial state to Seed
        SwitchState(CropState.Seed);
    }

    public void Grow()
    {

    }

    //Function to handle state changes
    void SwitchState(CropState stateToSwitch)
    {
        //Reset and set all GameOblects to Inactive
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);

        switch (stateToSwitch)
        {
            case CropState.Seed:
                //Enable GameObject
                seed.SetActive(true);
                break;
            case CropState.Seedling:
                //Enable GameObject
                seedling.SetActive(true);
                break;
            case CropState.Harvestable:
                //Enable GameObject
                harvestable.SetActive(true);
                break;
        }

        //Set current crop state to the state to switch
        cropState = stateToSwitch;
    }
}
