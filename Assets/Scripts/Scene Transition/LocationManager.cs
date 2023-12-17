using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    public static LocationManager Instance { get; private set; }

    public List<StartPoint> startPoints;

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

    //Find starting location based on where the player is from
    public Transform GetPlayerStartingPosition(SceneTransitionManager.Location enteringFrom)
    {
        //Find the matching startpoint based on the location given 
        StartPoint startingPoint = startPoints.Find(x => x.enteringFrom == enteringFrom);
        
        //Return transform
        return startingPoint.playerStart;
    }
}
