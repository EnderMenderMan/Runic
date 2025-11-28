using System;
using System.Linq;
using UnityEngine;

public class TransformRune : Rune
{
    [SerializeField] private FilterType transformFilterType;
    [SerializeField] private string[] transformFilter;
    [SerializeField] private int[] transformItemIndexOffsets;

    public override void TriggerRunePlacement(int itemIndex, Alter[] alters)
    {
        if (transformItemIndexOffsets.Length == 0)
        {
            for (int i = 0; i < alters.Length; i++)
            {
                if (transformFilterType == FilterType.Exclusive &&
                    alters[i].equippedRune.tags.Contains(transformFilter) == true)
                    continue;
                if (transformFilterType == FilterType.Inclusive &&
                    alters[i].equippedRune.tags.Contains(transformFilter) == false)
                    continue;
                Debug.Log("Transformrune");
                
            }
            
        }
    }
}
