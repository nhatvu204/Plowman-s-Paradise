using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Character")]
public class CharacterData : ScriptableObject
{
    public GameTimestamp birthday;
    public List<ItemData> likes;
    public List<ItemData> dislikes;

    [Header("Dialogue")]
    //Player first meets this character
    public List<DialogueLine> onFirstMeet;
    //What the character says by default
    public List<DialogueLine> defaultDialogue;
}
