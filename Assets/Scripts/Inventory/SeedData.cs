using System;
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

    [Header("Regrowable")]
    //Plant can regrow the crop after being harvested?
    public bool regrowable;
    //Time taken before yields another crop
    public int daysToRegrow;
}
