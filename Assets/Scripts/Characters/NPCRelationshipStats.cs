using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCRelationshipStats 
{
    public string name;
    public int friendshipPoints;

    public bool hasTalkedToday;
    public bool giftGivenToday;

    public NPCRelationshipStats(string name, int friendshipPoints)
    {
        this.name = name;
        this.friendshipPoints = friendshipPoints;
    }

    public NPCRelationshipStats(string name)
    {
        this.name = name;
        friendshipPoints = 0;
    }

    //250 friendship points = 1 heart
    public float Hearts()
    {
        return friendshipPoints / 250;
    }
}
 