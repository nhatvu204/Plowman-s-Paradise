using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Items/Item")]
public class ItemData : ScriptableObject
{
    public string description;

    //Icon
    public Sprite thumbnail;

    //GameObject shown in the scene
    public GameObject gameModel;

    public int cost;
}
