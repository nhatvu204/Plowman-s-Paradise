using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RelationshipListingManager : ListingManager<NPCRelationshipStats>
{
    //Full database of characters
    List<CharacterData> characters;
    protected override void DisplayListing(NPCRelationshipStats relationship, GameObject listingGameObject)
    {
        //Load and cache all characters if it hasn't been loaded
        if (characters == null)
        {
            LoadAllCharacters();
        }
        CharacterData characterData = GetCharacterDataFromString(relationship.name);
        listingGameObject.GetComponent<NPCRelationshipListing>().Display(characterData, relationship);
    }

    public CharacterData GetCharacterDataFromString(string name)
    {
        return characters.Find(i => i.name == name);
    }

    //Load all characters from Resource folder
    void LoadAllCharacters()
    {
        CharacterData[] characterDatabase = Resources.LoadAll<CharacterData>("Characters"); 
        characters = characterDatabase.ToList();
    }
}
