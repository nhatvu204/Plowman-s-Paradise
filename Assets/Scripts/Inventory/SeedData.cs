using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Seed")]
public class SeedData : ItemData
{
    public int daysToGrow;

    //The crop the seed will yield
    public ItemData cropToYield;

    //The seedling GameObject
    public GameObject seedling;
}
