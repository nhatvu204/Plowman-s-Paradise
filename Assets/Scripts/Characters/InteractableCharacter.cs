using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacter : InteractableObject
{
    public CharacterData characterData;

    //Cache the relationship of the NPC
    NPCRelationshipStats relationship;

    //The default rotation
    Quaternion defaultRotation;
    //Check if the LookAt coroutine is executing
    bool isTurning = false;

    private void Start()
    {
        relationship = RelationshipStats.GetRelationship(characterData);

        //Cache the original rotation of character
        defaultRotation = transform.rotation;
    }

    public override void PickUp()
    {
        LookAtPlayer();
        TriggerDialogue();
    }


    #region Rotation
    void LookAtPlayer()
    {
        //Get player's transform
        Transform player = FindObjectOfType<PlayerController>().transform;

        //Get vector for the direction towards the player
        Vector3 dir = player.position - transform.position;
        //lock the y axis of the vector so the npc doesn't look up or down to face the player
        dir.y = 0;
        //Convert the direction vector into a quaternion
        Quaternion lookRot = Quaternion.LookRotation(dir);

        //Look at the player
        StartCoroutine(LookAt(lookRot));
    }

    //Character progressively turn towards the player
    IEnumerator LookAt(Quaternion lookRot)
    {
        //Check if the coroutine is already running
        if (isTurning)
        {
            //Stop the coroutine
            isTurning = false;
        }
        else
        {
            isTurning = true;
        }

        while (transform.rotation != lookRot)
        {
            if (!isTurning)
            {
                //Stop coroutine execution
                yield break;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot,720 * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        isTurning = false;
    }

    //Rotate back to default rotation
    void ResetRotation()
    {
        StartCoroutine(LookAt(defaultRotation));
    }
    #endregion

    #region Conversations and Interactions
    void TriggerDialogue()
    {
        //Check if the player is holding anything
        if (InventoryManager.Instance.SlotEquipped(InvetorySlot.InventoryType.Item))
        {
            GiftDialogue();
            return;
        }

        List<DialogueLine> dialogueToHave = characterData.defaultDialogue;

        System.Action onDialogueEnd = null;

        //Character reset rotation when conversation is over
        onDialogueEnd += ResetRotation;

        //Check to determine which dialogue to have
        //First meeting?
        if (RelationshipStats.FirstMeeting(characterData))
        {
            dialogueToHave = characterData.onFirstMeet;

            onDialogueEnd += OnFirstMeeting;
        }

        if (RelationshipStats.IsFirstConversationOfTheDay(characterData))
        {
            onDialogueEnd += OnFirstConversation;
        }

        DialogueManager.Instance.StartDialogue(dialogueToHave, onDialogueEnd);
    }

    //Handle gift giving
    void GiftDialogue()
    {
        if (!EligibleForGift()) return;

        //Get the ItemSlotData of what the player is holding
        ItemSlotData handSlot = InventoryManager.Instance.GetEquippedSlot(InvetorySlot.InventoryType.Item);

        List<DialogueLine> dialogueToHave = characterData.neutralGiftDialogue;

        System.Action onDialogueEnd = () =>
        {
            relationship.giftGivenToday = true;

            //Remove item from player's hand
            InventoryManager.Instance.ConsumeItem(handSlot);
        };

        //Character reset rotation when conversation is over
        onDialogueEnd += ResetRotation;

        bool isBirthday = RelationshipStats.IsBirthday(characterData);
        
        //Friendship points to add from the gift
        int pointsToAdd = 0;

        //Check to determine which dialogue to put out
        switch (RelationshipStats.GetReactionToGift(characterData, handSlot.itemData))
        {
            case RelationshipStats.GiftReaction.Like:
                dialogueToHave = characterData.likedGiftDialogue;
                pointsToAdd = 80;
                if (isBirthday)
                {
                    dialogueToHave = characterData.birthdayLikedGiftDialogue;
                }
                break;
            case RelationshipStats.GiftReaction.Dislike:
                dialogueToHave = characterData.dislikedGiftDialogue;
                pointsToAdd = -20;
                if (isBirthday)
                {
                    dialogueToHave = characterData.birthdayDislikedGiftDialogue;
                }
                break;
            case RelationshipStats.GiftReaction.Neutral:
                dialogueToHave = characterData.neutralGiftDialogue;
                pointsToAdd = 20;
                if (isBirthday)
                {
                    dialogueToHave = characterData.birthdayNeutralGiftDialogue;
                }
                break;
        }

        //Birthday multiplier
        if (isBirthday)
        {
            pointsToAdd *= 8;
        }

        RelationshipStats.AddFriendPoints(characterData, pointsToAdd);

        DialogueManager.Instance.StartDialogue(dialogueToHave, onDialogueEnd);
    }
    
    bool EligibleForGift()
    {
        //Player has not unlocked this character
        if (RelationshipStats.FirstMeeting(characterData))
        {
            DialogueManager.Instance.StartDialogue(DialogueManager.CreateSimpleMessage("You have not unlocked this character yet."));
            return false;
        }

        //Player has already given this character a gift today
        if (RelationshipStats.GiftGivenToday(characterData))
        {
            DialogueManager.Instance.StartDialogue(DialogueManager.CreateSimpleMessage($"You have already given {characterData.name} a gift today."));
            return false;
        }

        return true;
    }

    void OnFirstMeeting()
    {
        //Unlock the character on the relationship
        RelationshipStats.UnlockCharacter(characterData);
        //Update relationship data
        relationship = RelationshipStats.GetRelationship(characterData);
    }

    void OnFirstConversation()
    {
        Debug.Log("First conversation");
        //Add 20 friend points
        RelationshipStats.AddFriendPoints(characterData, 20);

        relationship.hasTalkedToday = true;
    }
    #endregion
}
