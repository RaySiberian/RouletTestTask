using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chest", menuName = "ScriptableObjects/Chest")]
public class BoxScriptable : ScriptableObject
{
    public List<BoxItem> ItemsToDrop;
    public int TotalWeight;
    
    private void OnEnable()
    {
        if (ItemsToDrop == null || ItemsToDrop.Count < 1)
        {
            return;
        }

        int listTotalWeight = 0;
        
        foreach (var item in ItemsToDrop)
        {
            listTotalWeight += item.DropChance;
        }

        if (TotalWeight != listTotalWeight)
        {
            Debug.LogWarning("Chest Total Weight is not equal to sum of items");
        }
    }
}
