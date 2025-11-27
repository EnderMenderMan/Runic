using System.Linq;
using UnityEngine;

public class KickRune : Rune
{
    [SerializeField] private FilterType kickFilterType;
    [SerializeField] private int[] kickFilter;
    [SerializeField] private int[] kickItemIndexOffsets;

    // public override void TriggerRunePlacement(int alterIndex, Alter[] alters)
    // {
    //     for (int i = 0; i < alters.Length; i++)
    //     {
    //         if (i == alterIndex)
    //             continue;
    //         if (alters[i].equippedRune == null)
    //             continue;
    //         if (kickFilterEnable && kickFilter != alters[i].equippedRune.ValueID)
    //             continue;
    //         alters[i].KickItem();
    //     }
    //
    //     if (kickItemIndexOffsets == 0)
    //         return;
    //     int tryIndex = alterIndex + kickItemIndexOffsets;
    //     if (tryIndex < 0 || tryIndex >= alters.Length)
    //         return;
    //     if (alters[tryIndex].equippedRune == null)
    //         return;
    //     if (kickFilterEnable && kickFilter != alters[tryIndex].equippedRune.ValueID)
    //         return;
    //     alters[tryIndex].KickItem();
    // }

    public override void TriggerRunePlacement(int itemIndex, Alter[] alters)
    {
        if (kickItemIndexOffsets.Length == 0)
        {
            for (int i = 0; i < alters.Length; i++)
            {
                if (i == itemIndex)
                    continue;
                if (alters[i].equippedRune == null)
                    continue;
                if (kickFilterType == FilterType.Exclusive && kickFilter.Contains(alters[i].equippedRune.ValueID) == false)
                    continue;
                if (kickFilterType == FilterType.Inclusive && kickFilter.Contains(alters[i].equippedRune.ValueID) == true)
                    continue;
                alters[i].KickItem();
            }

            return;
        }

        for (int i = 0; i < kickItemIndexOffsets.Length; i++)
        {
            int tryIndex = itemIndex + kickItemIndexOffsets[i];
            if (tryIndex < 0 || tryIndex >= alters.Length)
                continue;
            if (alters[tryIndex].equippedRune == null)
                continue;
            if (kickFilterType == FilterType.Exclusive && kickFilter.Contains(alters[tryIndex].equippedRune.ValueID) == false)
                continue;
            if (kickFilterType == FilterType.Inclusive && kickFilter.Contains(alters[tryIndex].equippedRune.ValueID) == true)
                continue;
            alters[tryIndex].KickItem();
        }
    }
}
