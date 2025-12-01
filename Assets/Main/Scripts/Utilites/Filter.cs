using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AlterFilter
{
    [Header("Runes")]
    [Tooltip("Describes how the Runes Filter will be used")] public FilterType runeFilterType;
    [Tooltip("Filter all rune tags in the alter cluster")] public string[] runesFilter;
    [Tooltip("Used to select specific rune placements to run the Runes Filter on. IF EMPTY will run all runes. IF NOT EMPTY runs only selected indexes. -1 is one to the left and 2 is two to the right")] public int[] alterIndexOffsets;
    [Header("Alter")]
    [Tooltip("Describes how the Alter Filter will be used")] public FilterType alterFilterType;
    [Tooltip("Filter the alter's tags")] public string[] alterFilter;
    [Header("Alter Cluster")]
    [Tooltip("Describes how the Cluster Filter will be used")] public FilterType clusterFilterType;
    [Tooltip("Filter the cluster alter's tags")] public string[] clusterFilter;

    public bool RunClusterFilter(AlterCluster cluster)
    {
        if (clusterFilterType == FilterType.Disabled)
            return true;
        
        if (clusterFilterType == FilterType.Exclusive && cluster.tags.Contains(clusterFilter) == true)
            return false;
        if (clusterFilterType == FilterType.Inclusive && cluster.tags.Contains(clusterFilter) == false)
            return false;
        return true;
    }

    public bool RunAlterFilter(Alter alter)
    {
        if (alterFilterType == FilterType.Disabled)
            return true;
        
        if (alterFilterType == FilterType.Exclusive && alter.tags.Contains(alterFilter) == true)
            return false;
        if (alterFilterType == FilterType.Inclusive && alter.tags.Contains(alterFilter) == false)
            return false;
        return true;
    }
    
    public bool RunRuneFilter(int alterIndex, Alter[] alters)
    {
        if (runeFilterType == FilterType.Disabled)
            return true;

        bool foundRunes = false;
        
        if (alterIndexOffsets.Length == 0)
        {
            for (int i = 0; i < alters.Length; i++)
            {
                if (i == alterIndex)
                    continue;
                if (alters[i].equippedRune == null)
                    continue;
                foundRunes = true;
                if (runeFilterType == FilterType.Exclusive && alters[i].equippedRune.tags.Contains(runesFilter) == true)
                    continue;
                if (runeFilterType == FilterType.Inclusive && alters[i].equippedRune.tags.Contains(runesFilter) == false)
                    continue;
                return false;
            }
        }

        foreach (var indexOffset in alterIndexOffsets)
        {
            int tryIndex = alterIndex + indexOffset;
            if (tryIndex < 0 || tryIndex >= alters.Length)
                continue;
            if (alters[tryIndex].equippedRune == null)
                continue;
            foundRunes = true;
            if (runeFilterType == FilterType.Exclusive && alters[tryIndex].equippedRune.tags.Contains(runesFilter) == true)
                continue;
            if (runeFilterType == FilterType.Inclusive && alters[tryIndex].equippedRune.tags.Contains(runesFilter) == false)
                continue;
            return false;
        }

        if (foundRunes == false && runeFilterType == FilterType.Inclusive)
            return false;
        return true;
    }
}
public enum FilterType
{
    Disabled,
    Exclusive,
    Inclusive,
}

[System.Serializable]
public class Tags
{
    [SerializeField] string[] tags;
    public HashSet<string> ids;
    public void Init()
    {
        ids = new HashSet<string>();
        ids.UnionWith(tags);
    }
    public bool Contains(IEnumerable<string> keys, int amountMin = -1)
    {
        foreach (string key in keys)
        {
            if (ids.Contains(key))
            {
                if (amountMin <= 0)
                    return true;
                amountMin--;
            }
        }
        return false;
    }
}
