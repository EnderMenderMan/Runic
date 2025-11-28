using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AlterFilter
{
    public FilterType alterFilterType;
    public string[] alterFillter;
    public FilterType clusterFilterType;
    public string[] clusterFilter;

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
