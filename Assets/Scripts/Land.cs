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

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();

        //Default land status
        SwitchLandStatus(LandStatus.Soil);

        //Deselect the land by default
        Select(false);
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
                    break;
            }
        }
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        //Check if 24h is passed since last watered
        if (landStatus == LandStatus.Watered)
        {
            //Hours since last watered
            int hoursElapsed = GameTimestamp.CompareTimestamp(timeWatered, timestamp);
            Debug.Log(hoursElapsed);
        }
    }
}
