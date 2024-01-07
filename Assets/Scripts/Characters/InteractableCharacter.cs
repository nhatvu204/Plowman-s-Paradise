using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacter : InteractableObject
{
    public CharacterData characterData;

    //Cache the relationship of the NPC
    NPCRelationshipStats relationship;

    private void Start()
    {
        
    }

    public override void PickUp()
    {
        List<DialogueLine> dialogueToHave = characterData.defaultDialogue;

        System.Action onDialogueEnd = null;

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
}
