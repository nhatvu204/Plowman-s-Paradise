using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class IncubationManager : MonoBehaviour
{
    //The list of eggs currently being incubated
    public static List<EggIncubationSaveState> eggsIncubating = new List<EggIncubationSaveState>();

    //Days it take for an egg to incubate
    public const int daysToIncubate = 3;

    public List<Incubator> incubators;

    public static UnityEvent onEggUpdate = new UnityEvent();

    //Call the functions required when the player is in the scene
    private void OnEnable()
    {
        //Assign each incubator an ID
        RegisterIncubator();
        //Load up incubator information
        LoadIncubatorData();
        //Load the data on every update
        onEggUpdate.AddListener(LoadIncubatorData);
    }

    private void OnDestroy()
    {
        onEggUpdate.RemoveListener(LoadIncubatorData);
    }

    //Function to call on clock update
    public static void UpdateEggs()
    {
        //Eggs must be incubating
        if (eggsIncubating.Count == 0) return;

        foreach(EggIncubationSaveState egg in eggsIncubating.ToList())
        {
            //Update the egg
            egg.Tick();
            onEggUpdate?.Invoke();
            if(egg.timeToIncubate <= 0)
            {
                eggsIncubating.Remove(egg);

                //Handle chicken spawning
                Debug.Log("New chick");
            }
        }
    }

    //Assign an ID to each incubator
    void RegisterIncubator()
    {
        for (int i = 0; i <incubators.Count; i++)
        {
            incubators[i].incubationID = i;
        }
    }

    void LoadIncubatorData()
    {
        if (eggsIncubating.Count == 0) return;

        foreach(EggIncubationSaveState egg in eggsIncubating)
        {
            //Get the incubator to load
            Incubator incubatorToLoad = incubators[egg.incubatorID];

            bool isIncubating = true;
            //Check if the egg is hatching/has hatched
            if(egg.timeToIncubate <= 0)
            {
                isIncubating = false;
            }

            incubatorToLoad.SetIncubationState(isIncubating, egg.timeToIncubate);
        }
    }
}
