[System.Serializable]
public struct AlterFilter
{
    public FilterType alterFilterType;
    public int[] alterFillter;
    public FilterType clusterFilterType;
    public int[] clusterFilter;

}
public enum FilterType
{
    Disabled,
    Exclusive,
    Inclusive,
}
