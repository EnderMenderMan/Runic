using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct Filter
{
    [Tooltip("Describes how the filter will be used. INCLUSIVE: return true when at least one of the tags of this filter is included. EXCLUSIVE: return true if none of the tags is included")] public FilterType mode;
    [Tooltip("Describes what tags the filter will target")] public string[] tags;
}
[System.Serializable]
public struct AlterFilter
{
    [Tooltip("Filter the cluster alter's tags")] public Filter[] clusterFilters;
    [Tooltip("Filter the alter's tags")] public Filter[] alterFilters;
    [Header("Runes")]
    //[Tooltip("Describes how the Runes Filter will be used")] public FilterType runeFilterType;
    //[Tooltip("Filter all rune tags in the alter cluster")] public string[] runesFilter;
    [Tooltip("Filter all rune tags in the alter cluster")] public Filter[] runesFilters;
    [Tooltip("Used to select specific rune placements to run the Runes Filter on. IF EMPTY will run all runes. IF NOT EMPTY runs only selected indexes. -1 is one to the left and 2 is two to the right")] public int[] alterIndexOffsets;
    //[Header("Alter")]
    //[Tooltip("Describes how the Alter Filter will be used")] public FilterType alterFilterType;
    //[Tooltip("Filter the alter's tags")] public string[] alterFilter;
    //[Header("Alter Cluster")]
    //[Tooltip("Describes how the Cluster Filter will be used")] public FilterType clusterFilterType;
    //[Tooltip("Filter the cluster alter's tags")] public string[] clusterFilter;

    public bool RunClusterFilter(AlterCluster cluster)
    {
        foreach (var filter in clusterFilters)
        {
            if (filter.mode == FilterType.Disabled)
                continue;
        
            if (filter.mode == FilterType.Exclusive && cluster.tags.Contains(filter.tags) == true)
                return false;
            if (filter.mode == FilterType.Inclusive && cluster.tags.Contains(filter.tags) == false)
                return false;
        }
        return true;
    }

    public bool RunAlterFilter(Alter alter)
    {
        foreach (var filter in alterFilters)
        {
            if (filter.mode == FilterType.Disabled)
                continue;
            if (filter.mode == FilterType.Exclusive && alter.tags.Contains(filter.tags) == true)
                return false;
            if (filter.mode == FilterType.Inclusive && alter.tags.Contains(filter.tags) == false)
                return false;
        }
        return true;
    }
    
    public bool RunRuneFilter(int alterIndex, Alter[] alters)
    {
        foreach (var filter in runesFilters)
        {
            if (filter.mode == FilterType.Disabled)
                continue;
            
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
                    if (filter.mode == FilterType.Exclusive && alters[i].equippedRune.tags.Contains(filter.tags) == false)
                        continue;
                    if (filter.mode == FilterType.Inclusive && alters[i].equippedRune.tags.Contains(filter.tags) == true)
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
                if (filter.mode == FilterType.Exclusive && alters[tryIndex].equippedRune.tags.Contains(filter.tags) == false)
                    continue;
                if (filter.mode == FilterType.Inclusive && alters[tryIndex].equippedRune.tags.Contains(filter.tags) == true)
                    continue;
                return false;
            }
        
            if (foundRunes == false && filter.mode == FilterType.Inclusive)
                return false;
        }
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
