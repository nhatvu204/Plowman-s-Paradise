using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour, ITimeTracker
{
    public enum LandStatus
    {
        Soil, Farmland, Watered
    }

    public LandStatus landStatus;

    public Material soilMat, farmlandMat, wateredMat;
    new Renderer renderer;

    //The selection gameobject to enable when the player selects the land
    public GameObject select;

    //Cache the time the land was watered
    GameTimestamp timeWatered;

    [Header("Crops")]
    //Crop prefab to instantiate
    public GameObject cropPrefab;

    //Crop currently planted on the land
    CropBehavior cropPlanted = null;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();

        //Default land status
        SwitchLandStatus(LandStatus.Soil);

        //Deselect the land by default
        Select(false);

        //Add Land to TimeManager's listener list
        TimeManager.Instance.RegisterTracker(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchLandStatus(LandStatus statusToSwitch)
    {
        landStatus = statusToSwitch;
        Material materialToSwitch = soilMat;
        switch (statusToSwitch)
        {
            case LandStatus.Soil:
                materialToSwitch = soilMat;
                break;
            case LandStatus.Farmland: 
                materialToSwitch = farmlandMat;
                break;
            case LandStatus.Watered: 
                materialToSwitch = wateredMat;
                break;
        }

        renderer.material = materialToSwitch;
    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }

    //When player press Interact button while selecting the land
    public void Interact()
    {
        //Check player's tool slot
        ItemData toolSlot = InventoryManager.Instance.equippedTool;

        //If there's nothing equipped, return
        if (toolSlot == null)
        {
            return;
        }

        //Try casting the itemdata in the toolslot as EquipmentData
        EquipmentData equipmentTool = toolSlot as EquipmentData;

        //Check if it is of type EquipmentData
        if( equipmentTool != null )
        {
            //Get the tool type
            EquipmentData.ToolType toolType = equipmentTool.toolType;

            switch(toolType)
            {
                case EquipmentData.ToolType.Hoe:
                    SwitchLandStatus(LandStatus.Farmland);
                    break;
                case EquipmentData.ToolType.WateringCan:
                    SwitchLandStatus(LandStatus.Watered); 

                    //Cache the time it was watered
                    timeWatered = TimeManager.Instance.GetGameTimestamp();
                    break;
            }

            return;
        }

        //Try casting the itemdata in the toolslot as SeedData
        SeedData seedTool = toolSlot as SeedData;

        //Conditions to be able to plant a seed
        if( seedTool != null && landStatus != LandStatus.Soil && cropPlanted != null)
        {
            //Instantiate the crop parented to the land
            GameObject cropObject = Instantiate(cropPrefab, transform);

            //Move the crop object to top of land object
            cropObject.transform.position = new Vector3(transform.position.x, 0, transform.position.z);

            //Access the CropBehavior of the crop going to be planted
            cropPlanted = cropObject.GetComponent<CropBehavior>();

            //Plant
            cropPlanted.Plant(seedTool);
        }
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        //Check if 24h passed since last watered
        if (landStatus == LandStatus.Watered)
        {
            int hourElapsed = GameTimestamp.CompareTimestamps(timeWatered, timestamp);
            Debug.Log(hourElapsed);

            if (hourElapsed > 24)
            {
                //Dry up after 24h
                SwitchLandStatus(LandStatus.Farmland);
            }
        }
    }
}