using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerController playerController;

    //The land the player currently selecting
    Land selectedLand = null;

    //The interactable object the player is selecting
    InteractableObject selectedInteractable = null;

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

        //Check if the player is going to interact with an Item
        if(other.tag == "Item")
        {
            //Set the interactable to the currently selected interactable
            selectedInteractable = other.GetComponent<InteractableObject>();
            return;
        }

        //Deselect the interactable 
        if(selectedInteractable != null)
        {
            selectedInteractable = null; 
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
        //Can't use a tool when an Item in hand
        if(InventoryManager.Instance.SlotEquipped(InvetorySlot.InventoryType.Item))
        {

            return;
        }

        //Check if the player on a farmable land
        if (selectedLand != null)
        {
            selectedLand.Interact();
            return;
        }
        Debug.Log("Not on any land");
    }

    //Triggered on pressing the item interact button
    public void ItemInteract()
    {
        //If the player is holding something, keep it in his inventory
        if (InventoryManager.Instance.SlotEquipped(InvetorySlot.InventoryType.Item))
        {
            InventoryManager.Instance.HandToInventory(InvetorySlot.InventoryType.Item);
            return;
        }

        //If the player isn't holding anything, pick up an item

        //Check if there is an interactable selected
        if(selectedInteractable != null)
        {
            //Pick up
            selectedInteractable.PickUp();

            Debug.Log("PickUp works");
        }

    }
}
