using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerController playerController;

    //The land the player currently selecting
    Land selectedLand = null;

    // Start is called before the first frame update
    void Start()
    {
        //Get access to PlayerController coomponent
        playerController = transform.parent.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {
            OnInteractableHit(hit);
        }

    }

    //Interaction raycast hit something interactable
    void OnInteractableHit(RaycastHit hit)
    {
        //Identify what the player hit
        Collider other = hit.collider;

        //Check if the player is going to interact with land
        if(other.tag == "Land")
        {
            //Get Land component
            Land land = other.GetComponent<Land>();
            SelectLand(land);
            return;
        }

        //Unselect the land when the player walk off
        if(selectedLand != null)
        {
            selectedLand.Select(false);
            selectedLand = null;
        }
    }

    //Handle selection of land
    void SelectLand(Land land)
    {
        if (selectedLand != null)
        {
            selectedLand.Select(false);
        }

        //Set new selected Land as the Land the player is selecting 
        selectedLand = land;
        land.Select(true);
    }

    public void Interact()
    {
        //Check if the player on a farmable land
        if (selectedLand != null)
        {
            selectedLand.Interact();
            return;
        }
        Debug.Log("Not on any land");
    }
}
