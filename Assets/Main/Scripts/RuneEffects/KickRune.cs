using System.Linq;
using UnityEngine;

public class KickRune : Rune
{
    [SerializeField] private bool kickFilterEnable;
    [SerializeField] private int kickFilter;
    [SerializeField] private int kickItemIndexOffsets;
    [SerializeField] bool enableAlterFilter;
    [SerializeField] int alterFilter;

    public override bool TryBePlaced(int itemIndex, Alter[] alters)
    {
        if (enableAlterFilter == false)
            return true;
        return alters[itemIndex].ValueID == alterFilter;
    }

    public override void TriggerRunePlacement(int alterIndex, Alter[] alters)
    {
        for (int i = 0; i < alters.Length; i++)
        {
            if (i == alterIndex)
                continue;
            if (alters[i].equippedRune == null)
                continue;
            if (kickFilterEnable && kickFilter != alters[i].equippedRune.ValueID)
                continue;
            alters[i].KickItem();
        }

        if (kickItemIndexOffsets == 0)
            return;
        int tryIndex = alterIndex + kickItemIndexOffsets;
        if (tryIndex < 0 || tryIndex >= alters.Length)
            return;
        if (alters[tryIndex].equippedRune == null)
            return;
        if (kickFilterEnable && kickFilter != alters[tryIndex].equippedRune.ValueID)
            return;
        alters[tryIndex].KickItem();
    }

    /*public override void TriggerPillarPlacement(int itemIndex, Alter[] alters)
    {
        if (kickItemIndexOffsets.Length == 0)
        {
            for (int i = 0; i < alters.Length; i++)
            {
                if (i == itemIndex)
                    continue;
                if (alters[i].equippedRune == null)
                    continue;
                if (kickFilterEnable && kickFilter.Contains(alters[i].equippedRune.ValueID) == false)
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
            if (kickFilterEnable && kickFilter.Contains(alters[tryIndex].equippedRune.ValueID) == false)
                continue;
            alters[tryIndex].KickItem();
        }
    }*/
}
