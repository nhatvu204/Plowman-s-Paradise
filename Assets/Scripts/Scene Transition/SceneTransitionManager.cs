using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    //Scenes
    public enum Location
    {
        Farm,
        PlayerHome,
        Town
    }
    public Location currentLocation;

    //List of places indoor
    static readonly Location[] indoor = { Location.PlayerHome };

    //Player's transform
    Transform playerPoint;

    private void Awake()
    {
        //If there is more than 1 instance, destroy GameObject
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }

        //Make the game object persistent across scenes
        DontDestroyOnLoad(gameObject);

        //Call OnLocationLoad on load of the scene
        SceneManager.sceneLoaded += OnLocationLoad;

        //Find player's transform
        playerPoint = FindObjectOfType<PlayerController>().transform;
    }

    //Check if the current location is indoors
    public bool CurrentlyIndoor()
    {
        return indoor.Contains(currentLocation);
    }

    //Switch player to another scene
    public void SwitchLocation(Location loactionToSwitch)
    {
        SceneManager.LoadScene(loactionToSwitch.ToString());
    }

    //Called when a scene is loaded
    public void OnLocationLoad(Scene scene, LoadSceneMode mode)
    {
        //Location the player is coming from when the scene loads
        Location oldLocation = currentLocation;
         
        //Get the new location by converting the string of current scene into a location enum value 
        Location newLocation = (Location)Enum.Parse(typeof(Location), scene.name);

        //Stop function if player not coming from new location
        if (currentLocation == newLocation) return;

        //Find start point
        Transform startPoint = LocationManager.Instance.GetPlayerStartingPosition(oldLocation);

        //If the player object is destroyed, stop execution
        if (playerPoint == null) return;

        //Disable CharacterController component
        CharacterController playerCharacter = playerPoint.GetComponent<CharacterController>();
        playerCharacter.enabled = false;

        //Change player's position to start point
        playerPoint.position = startPoint.position;
        playerPoint.rotation = startPoint.rotation;
        
        //Enable player character controller so the player can move
        playerCharacter.enabled = true;

        //Save current position
        currentLocation = newLocation;
    }
}
