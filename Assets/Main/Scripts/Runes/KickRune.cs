using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KickRune : Rune
{
    [Tooltip("Filter and kick all runes that matches the filter (At least one Kick Filter returns true)")][SerializeField] private Filter[] kickFilters;
    [Tooltip("Used to select specific rune placements to run the Kick Filters on. IF EMPTY will run all runes. IF NOT EMPTY runs only selected indexes. -1 is one to the left and 2 is two to the right")][SerializeField] private int[] kickItemIndexOffsets;

    public override void TriggerRunePlacement(int itemIndex, Alter[] alters)
    {
        foreach (Alter alter in GetKickFilterAlters(itemIndex, alters))
            alter.TryKickItem(false);
    }
    public override void OnKicked()
    {
        foreach (Alter alter in GetKickFilterAlters(alter.clusterIndex, alter.alterCluster.alters))
            alter.StopKickCorutine();
        base.OnKicked();
    }

    List<Alter> GetKickFilterAlters(int alterIndex, Alter[] alters)
    {
        List<Alter> selectedAlters = new List<Alter>();
        foreach (var filter in kickFilters)
        {
            if (kickItemIndexOffsets.Length == 0)
            {
                for (int i = 0; i < alters.Length; i++)
                {
                    if (i == alterIndex)
                        continue;
                    if (alters[i].equippedRune == null)
                        continue;
                    if (filter.RunFilter(alters[i].equippedRune.tags) == false)
                        continue;
                    selectedAlters.Add(alters[i]);
                }
                continue;
            }

            for (int i = 0; i < kickItemIndexOffsets.Length; i++)
            {
                int tryIndex = alterIndex + kickItemIndexOffsets[i];
                if (tryIndex < 0 || tryIndex >= alters.Length)
                    continue;
                if (alters[tryIndex].equippedRune == null)
                    continue;
                if (filter.RunFilter(alters[tryIndex].equippedRune.tags) == false)
                    continue;
                selectedAlters.Add(alters[i]);
            }
        }

        if (kickFilters.Length == 0)
        {
            for (int i = 0; i < kickItemIndexOffsets.Length; i++)
            {
                int tryIndex = alterIndex + kickItemIndexOffsets[i];
                if (tryIndex < 0 || tryIndex >= alters.Length)
                    continue;
                if (alters[tryIndex].equippedRune == null)
                    continue;
                selectedAlters.Add(alters[i]);
            }
        }
        return selectedAlters;
    }
}
