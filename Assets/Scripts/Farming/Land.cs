using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour, ITimeTracker
{
    public int id;
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

    //Obstacles
    public enum FarmObstacleStatus
    {
        None,
        Rock,
        Wood,
        Weeds
    }
    [Header("Obstacles")]
    public FarmObstacleStatus obstacleStatus;
    public GameObject rockPrefab, woodPrefab, weedsPrefab;

    //Store instantiated obstacle as a variable
    GameObject obstacleObject;

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

    public void LoadLandData(LandStatus LandStatusToSwitch, GameTimestamp lastWatered, FarmObstacleStatus obstacleStatusToSwitch)
    {
        //Set land status accordingly
        landStatus = LandStatusToSwitch;
        timeWatered = lastWatered;

        Material materialToSwitch = soilMat;
        switch (LandStatusToSwitch)
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

        switch (obstacleStatusToSwitch)
        {
            case FarmObstacleStatus.None:
                //Destroy obstacleObj
                if (obstacleObject != null) Destroy(obstacleObject);
                break;
            case FarmObstacleStatus.Rock:
                //Instatiate the prefab on land and assign to obstacleObj
                obstacleObject = Instantiate(rockPrefab, transform);
                break;
            case FarmObstacleStatus.Wood:
                //Instatiate the prefab on land and assign to obstacleObj
                obstacleObject = Instantiate(woodPrefab, transform);
                break;
            case FarmObstacleStatus.Weeds:
                //Instatiate the prefab on land and assign to obstacleObj
                obstacleObject = Instantiate(weedsPrefab, transform);
                break;
        }

        //Move the obstacle object to the top of the land
        if (obstacleObject != null) obstacleObject.transform.position = new Vector3(transform.position.x, 0.02f, transform.position.z);

        //Set the status accordingly
        obstacleStatus = obstacleStatusToSwitch;
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

                //Cache the time it was watered
                timeWatered = TimeManager.Instance.GetGameTimestamp();
                break;
        }

        renderer.material = materialToSwitch;

        LandManager.Instance.OnLandStateChange(id, landStatus, timeWatered, obstacleStatus);
    }

    public void SetObstacleStatus(FarmObstacleStatus statusToSwitch)
    {
        switch (statusToSwitch)
        {
            case FarmObstacleStatus.None:
                //Destroy obstacleObj
                if (obstacleObject != null) Destroy(obstacleObject);
                break;
            case FarmObstacleStatus.Rock:
                //Instatiate the prefab on land and assign to obstacleObj
                obstacleObject = Instantiate(rockPrefab, transform);
                break;
            case FarmObstacleStatus.Wood:
                //Instatiate the prefab on land and assign to obstacleObj
                obstacleObject = Instantiate(woodPrefab, transform);
                break;
            case FarmObstacleStatus.Weeds:
                //Instatiate the prefab on land and assign to obstacleObj
                obstacleObject = Instantiate(weedsPrefab, transform);
                break;
        }

        //Move the obstacle object to the top of the land
        if(obstacleObject != null) obstacleObject.transform.position = new Vector3(transform.position.x, 0.02f, transform.position.z);

        //Set the status accordingly
        obstacleStatus = statusToSwitch;

        LandManager.Instance.OnLandStateChange(id, landStatus, timeWatered, obstacleStatus);
    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }

    //When player press Interact button while selecting the land
    public void Interact()
    {
        //Check player's tool slot
        ItemData toolSlot = InventoryManager.Instance.GetEquippedSlotItem(InvetorySlot.InventoryType.Tool);

        //If there's nothing equipped, return
        if (!InventoryManager.Instance.SlotEquipped(InvetorySlot.InventoryType.Tool))
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
                    //Land must be tilled before watering
                    if (landStatus != LandStatus.Soil)
                    {
                        SwitchLandStatus(LandStatus.Watered);
                    }           
                    break;
                case EquipmentData.ToolType.Shovel:
                    //Remove crop from the land
                    if(cropPlanted != null)
                    {
                        cropPlanted.RemoveCrop();
                    }

                    //Remove weeds obstacle
                    if (obstacleStatus == FarmObstacleStatus.Weeds) SetObstacleStatus(FarmObstacleStatus.None);

                    break;
                case EquipmentData.ToolType.Axe:

                    //Remove wood obstacle
                    if (obstacleStatus == FarmObstacleStatus.Wood) SetObstacleStatus(FarmObstacleStatus.None);

                    break;
                case EquipmentData.ToolType.PickAxe:

                    //Remove rock obstacle
                    if (obstacleStatus == FarmObstacleStatus.Rock) SetObstacleStatus(FarmObstacleStatus.None);

                    break;
            }

            return;
        }

        //Try casting the itemData in the toolSlot as SeedData
        SeedData seedTool = toolSlot as SeedData;

        //Conditions to be able to plant a seed
        if (seedTool != null && landStatus != LandStatus.Soil && cropPlanted == null && obstacleStatus == FarmObstacleStatus.None)
        {
            SpawnCrop();
            //Plant
            cropPlanted.Plant(id, seedTool);

            //Consume the item
            InventoryManager.Instance.ConsumeItem(InventoryManager.Instance.GetEquippedSlot(InvetorySlot.InventoryType.Tool));
        }
    }

    public CropBehavior SpawnCrop()
    {
        //Instantiate the crop object parented to the land
        GameObject cropObject = Instantiate(cropPrefab, transform);
        //Move the crop object to the top of the land
        cropObject.transform.position = new Vector3(transform.position.x, 0.02f, transform.position.z);

        //Access the crop behavior of the crop going to be planted
        cropPlanted = cropObject.GetComponent<CropBehavior>();

        return cropPlanted;
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        //Check if 24h passed since last watered
        if (landStatus == LandStatus.Watered)
        {
            int hourElapsed = GameTimestamp.CompareTimestamps(timeWatered, timestamp);
            Debug.Log(hourElapsed);

            //Grow the planted crop
            if (cropPlanted != null)
            {
                cropPlanted.Grow();
            }

            if (hourElapsed > 24)
            {
                //Dry up after 24h
                SwitchLandStatus(LandStatus.Farmland);
            }
        }

        //Handle wilting of the plant when not watered
        if (landStatus != LandStatus.Watered && cropPlanted != null)
        {
            //If the crop has already germinated, start the withering
            if (cropPlanted.cropState != CropBehavior.CropState.Seed)
            {
                cropPlanted.Wither();
            }
        }
    }

    private void OnDestroy()
    {
        //Unsubscribe from the list on destroy
        TimeManager.Instance.UnregisterTracker(this);
    }
}
