using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Land;

[System.Serializable]
public struct LandSaveState 
{
    public Land.LandStatus landStatus;
    public GameTimestamp lastWatered;

    public LandSaveState(Land.LandStatus landStatus, GameTimestamp lastWatered)
    {
        this.landStatus = landStatus;
        this.lastWatered = lastWatered;
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        //Check if 24h passed since last watered
        if (landStatus == Land.LandStatus.Watered)
        {
            int hourElapsed = GameTimestamp.CompareTimestamps(lastWatered, timestamp);
            Debug.Log(hourElapsed);

            if (hourElapsed > 24)
            {
                //Dry up after 24h
               landStatus = Land.LandStatus.Farmland;
            }
        }
    }
}
