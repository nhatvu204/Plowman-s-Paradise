using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipStats : MonoBehaviour
{
    //Relationship data of all the NPCs player has met in the game
    public static List<NPCRelationshipStats> relationships = new List<NPCRelationshipStats>();

    public enum GiftReaction
    {
        Like, Dislike, Neutral
    }

    public static void LoadStats(List<NPCRelationshipStats> relationshipsToLoad)
    {
        if (relationshipsToLoad == null)
        {
            relationships = new List<NPCRelationshipStats>();
            return;
        }
        relationships = relationshipsToLoad;
    }

    //Check if the player has met this NPC
    public static bool FirstMeeting(CharacterData character)
    {
        return !relationships.Exists(i => i.name == character.name);
    }

    //Get relationship info about a character
    public static NPCRelationshipStats GetRelationship(CharacterData character)
    {
        //Check if it's the first meeting
        if (FirstMeeting(character)) return null;

        return relationships.Find(i => i.name == character.name);
    }

    //Add character to relationship data
    public static void UnlockCharacter(CharacterData character)
    {
        relationships.Add(new NPCRelationshipStats(character.name));
    }

    //Improve relationship with an NPC
    public static void AddFriendPoints(CharacterData character, int points)
    {
        if (FirstMeeting(character))
        {
            Debug.LogError("Never met this one");
            return;
        }

        GetRelationship(character).friendshipPoints += points;
    }

    //Check if this is the first conversation the player have with the NPC today
    public static bool IsFirstConversationOfTheDay(CharacterData character)
    {
        if (FirstMeeting(character)) return true;

        NPCRelationshipStats npc = GetRelationship(character);
        return !npc.hasTalkedToday;
    }

    //Check if the player has already given this character a gift today
    public static bool GiftGivenToday(CharacterData character)
    {
        NPCRelationshipStats npc = GetRelationship(character);
        return npc.giftGivenToday;
    }

    public static GiftReaction GetReactionToGift(CharacterData character, ItemData item)
    {
        //Check if the item is in the like or dislike or not in any list
        if (character.likes.Contains(item)) return GiftReaction.Like;
        if (character.dislikes.Contains(item)) return GiftReaction.Dislike;

        return GiftReaction.Neutral;
    }

    //Check if it's the character's birthday
    public static bool IsBirthday(CharacterData character)
    {
        GameTimestamp birthday = character.birthday;
        GameTimestamp today = TimeManager.Instance.GetGameTimestamp();

        return (today.day == birthday.day) && (today.season == birthday.season);
    }
}
