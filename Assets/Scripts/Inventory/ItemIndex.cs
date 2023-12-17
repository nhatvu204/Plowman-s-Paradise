using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Item Index")]
public class ItemIndex : ScriptableObject
{
    public List<ItemData> item;

    public ItemData GetItemFromString(string name)
    {
        return item.Find(i => i.name == name);
    }
}
